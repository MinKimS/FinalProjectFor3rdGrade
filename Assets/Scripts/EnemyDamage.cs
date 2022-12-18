using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    public float defaultMaxHp = 100.0f; //�� �ִ� ü��
    public float eHp = 100; //�� ü��
    Renderer c;
    Color curColor; //���� ���� (�� �����)
    public GameObject hpBarPrefab; //hp�� ������
    public Vector3 hpBarOffset = new Vector3(0, 0.6f, 0); //����
    private Canvas uiCanvas; //�θ� �� canvas
    private Image hpBarImage; //����� hp��
    GameManager gm;

    public float gDamage = 1.0f; //�� ������

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        c = gameObject.GetComponentInChildren<Renderer>();
        curColor = c.material.color;
        StartCoroutine(SetHPAndWeapon()); //ü�°� �� ������ ����
    }
    IEnumerator SetHPAndWeapon()
    {
        //ü�� ����
        SetHpBar();
        //�� ������ ����
        SetWeaponDamage();
        yield return null;//�������������ɶ����� ���(null ���� ���Ǳװɷ� �ٲٱ�)
    }

    void SetWeaponDamage()//�� ������ ���� ������ ����
	{
        switch (gm.weapon)
        {
            //����
            case 0:
                gDamage = 42;
                break;
            //������
            case 1:
                gDamage = 28;
                break;
            //������
            case 2:
                gDamage = 56;
                break;
            //����
            case 3:
                gDamage = 100;
                break;
            //SMG
            case 4:
                gDamage = 14;
                break;
            //��������
            case 5:
                gDamage = 70;
                break;
        }
    }
    void SetHpBar()
    {
        //hp����
        eHp = defaultMaxHp + gm.enMaxHP;

        //hp ui ����
        uiCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
        // ui canvas ������ ���� �������� ����
        GameObject hpBar = Instantiate(hpBarPrefab, uiCanvas.transform);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];
        //hp�ٰ� ���󰡾��� ���� ������ �� ����
        EnemyHpBar bar = hpBar.GetComponent<EnemyHpBar>();
        bar.target = gameObject;
        bar.offset = hpBarOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        //�Ѿ˰� �浹�� ���
        if(other.tag == "Bullet")
        {
            //���� ���� ���������� �����ϰ� �ٽ� ������� ���ƿ���
            c.material.color = Color.red;
            Invoke("ColorChange", 0.5f);
            //�Ѿ� ����
            Destroy(other.gameObject);
            BulletCtrl bc = other.gameObject.GetComponent<BulletCtrl>();
            if (bc != null)
            {
                //���� �Դ� ������
                eHp -= (gm.atkP * gDamage) * (1 - gm.defE);
                //Debug.Log("Enemy Damage = " + gm.atkP + "x" + gDamage + "=" + (gm.atkP * gDamage));
                hpBarImage.fillAmount = eHp / (defaultMaxHp+gm.enMaxHP);    //�� hp ui�� �ݿ�
            }

            //�� ü���� 0���ϸ� ���� ó��
            if (eHp <= 0.0f)
            {
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
                hpBarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
            }
        }
    }

    //���� �������� ����
    void ColorChange()
    {
        c.material.color = curColor;
    }
}
