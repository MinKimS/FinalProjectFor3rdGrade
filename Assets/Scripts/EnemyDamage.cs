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

    void Start()
    {
        c = gameObject.GetComponentInChildren<Renderer>();
        curColor = c.material.color;
        SetHpBar();
    }
    void SetHpBar()
    {
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
                hp -= bc.damage;
                hpBarImage.fillAmount = hp / 100.0f;
            }

            if (hp <= 0.0f)
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
