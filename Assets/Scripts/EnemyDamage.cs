using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    float hp = 100.0f;
    Renderer c;
    Color curColor;
    public GameObject hpBarPrefab;
    public Vector3 hpBarOffset = new Vector3(0, 0.6f, 0);
    private Canvas uiCanvas;
    private Image hpBarImage;
    GameManager gm;

    public float gDamage = 1.0f;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        c = gameObject.GetComponentInChildren<Renderer>();
        curColor = c.material.color;
        SetHpBar();
    }
    void SetHpBar()
    {
        //hp설정
        hp = gm.enMaxHP;

        //hp ui 설정
        uiCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
        GameObject hpBar = Instantiate(hpBarPrefab, uiCanvas.transform);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];
        EnemyHpBar bar = hpBar.GetComponent<EnemyHpBar>();
        bar.targetTr = gameObject.transform;
        bar.offset = hpBarOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bullet")
        {
            c.material.color = Color.red;
            Invoke("ColorChange", 0.5f);
            Destroy(other.gameObject);
            BulletCtrl bc = other.gameObject.GetComponent<BulletCtrl>();
            if (bc != null)
            {
                Damage();
            }

            if (hp <= 0.0f)
            {
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
                hpBarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
            }
        }
    }

    //적이 입는 데미지 함수
    void Damage()
    {
        switch(gm.weapon)
        {
            //권총
            case 0:
                gDamage = 3;
                break;
            //라이플
            case 1:
                gDamage = 2;
                break;
            //리볼버
            case 2:
                gDamage = 4;
                break;
            //샷건
            case 3:
                gDamage = 8;
                break;
            //SMG
            case 4:
                gDamage = 1;
                break;
            //스나이퍼
            case 5:
                gDamage = 5;
                break;
        }
        //데미지
        hp -= (gm.atkP* gDamage) * (1 - gm.defE);
        hpBarImage.fillAmount = hp / gm.enMaxHP;    //적 hp ui에 반영
    }

    void ColorChange()
    {
        c.material.color = curColor;
    }
}
