using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    public float defaultMaxHp = 100.0f; //�� ü��
    public float curHp = 100;
    Renderer c;
    Color curColor; //���� ���� (�� �����)
    public GameObject hpBarPrefab; //hp��
    public Vector3 hpBarOffset = new Vector3(0, 0.6f, 0);
    private Canvas uiCanvas;
    private Image hpBarImage;
    GameManager gm;

    public float gDamage = 1.0f; //�� ������

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
        yield return null;//�������������ɶ����� ���(null ���� ���Ǳװɷ� �ٲٱ�)
    }

    void SetWeaponDamage()//�� ������ ����
	{
        switch (gm.weapon)
        {
            //����
            case 0:
                gDamage = 21;
                break;
            //������
            case 1:
                gDamage = 14;
                break;
            //������
            case 2:
                gDamage = 28;
                break;
            //����
            case 3:
                gDamage = 50;
                break;
            //SMG
            case 4:
                gDamage = 7;
                break;
            //��������
            case 5:
                gDamage = 35;
                break;
        }
    }
    void SetHpBar()
    {
        //hp����
        curHp = defaultMaxHp+gm.enMaxHP;
        
        if(gameObject.GetComponent<EDamageData>().etype != EDamageData.EType.BOSS)
        {
            //hp ui ����
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
                //���� �Դ� ������
                curHp -= (gm.atkP * gDamage) * (1 - gm.defE);
                //Debug.Log("Enemy Damage = " + gm.atkP + "x" + gDamage + "=" + (gm.atkP * gDamage));
                hpBarImage.fillAmount = curHp / (defaultMaxHp+gm.enMaxHP);    //�� hp ui�� �ݿ�
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
