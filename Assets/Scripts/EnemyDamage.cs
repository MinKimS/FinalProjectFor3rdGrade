using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    public float defaultMaxHp = 100.0f; //적 체력
    public float curHp = 100;
    Renderer c;
    Color curColor; //현재 색상 (색 변경용)
    public GameObject hpBarPrefab; //hp바
    public Vector3 hpBarOffset = new Vector3(0, 0.6f, 0);
    private Canvas uiCanvas;
    private Image hpBarImage;
    GameManager gm;

    public float gDamage = 1.0f; //총 데미지

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        c = gameObject.GetComponentInChildren<Renderer>();
        curColor = c.material.color;
        StartCoroutine(SetHPAndWeapon());
    }
    IEnumerator SetHPAndWeapon()
    {
        SetHpBar();
        SetWeaponDamage();
        yield return null;//다음스테이지될때까지 대기(null 말고 조건그걸로 바꾸기)
    }

    void SetWeaponDamage()//총 데미지 설정
	{
        switch (gm.weapon)
        {
            //권총
            case 0:
                gDamage = 21;
                break;
            //라이플
            case 1:
                gDamage = 14;
                break;
            //리볼버
            case 2:
                gDamage = 28;
                break;
            //샷건
            case 3:
                gDamage = 50;
                break;
            //SMG
            case 4:
                gDamage = 7;
                break;
            //스나이퍼
            case 5:
                gDamage = 35;
                break;
        }
    }
    void SetHpBar()
    {
        //hp설정
        curHp = defaultMaxHp+gm.enMaxHP;
        
        if(gameObject.GetComponent<EDamageData>().etype != EDamageData.EType.BOSS)
        {
            //hp ui 설정
            uiCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
            GameObject hpBar = Instantiate(hpBarPrefab, uiCanvas.transform);
            hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];
            EnemyHpBar bar = hpBar.GetComponent<EnemyHpBar>();
            bar.target = gameObject;
            bar.offset = hpBarOffset;
        }
        else
        {

        }
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
                //적이 입는 데미지
                curHp -= (gm.atkP * gDamage) * (1 - gm.defE);
                //Debug.Log("Enemy Damage = " + gm.atkP + "x" + gDamage + "=" + (gm.atkP * gDamage));
                hpBarImage.fillAmount = curHp / (defaultMaxHp+gm.enMaxHP);    //적 hp ui에 반영
            }

            if (curHp <= 0.0f)
            {
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
                hpBarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
            }
        }
    }

    void ColorChange()
    {
        c.material.color = curColor;
    }
}
