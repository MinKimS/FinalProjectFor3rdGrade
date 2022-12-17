using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Item[] items;//������ ����Ʈ
    public Image itemImg;//������ �̹���
    public Text itemName;//������ �̸�
    public Text itemDesc;//������ ����
    public int itemIdx = 0;//�������ε���
    GameManager gm;
    public Sprite soldoutImg;//�����̹���
    private int sell=0;//������ �Ǹ� Ƚ��
    private int sellLimit = 3;  //�ִ� ������ �ǸŰ���

    public GameObject oneDiceUI; //�ѹ������� �ֻ���UI
    public GameObject successText; //�ֻ��� ���� �ؽ�Ʈ
    public GameObject failText; //�ֻ��� ���� �ؽ�Ʈ
    private Button btn; //��ư ������Ʈ
    private ColorBlock cb; //��ư ����

    private void Awake()
    {
        btn = GetComponent<Button>();
        cb = btn.colors;
    }
    private void OnEnable()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        itemIdx = Random.Range(0, items.Length);
        itemImg.sprite = items[itemIdx].itemImg;
        itemName.text = items[itemIdx].itemName;
        itemDesc.text = items[itemIdx].itemDesc;

        //���Ű� �����ϸ�
        if (gm.money >= items[itemIdx].itmePrice && gm.weaponUp < 4)
        {
            ColorWhite();
        }
        //���������� ���Ű� �Ұ����ϸ�
        else
        {
            ColorRed();
        }
    }

    private void Update()
    {
        //�� ������ ���źҰ�
        if(gm.money < items[itemIdx].itmePrice)
        {
            ColorRed();
        }
        //���Ⱝȭ �� 3���� �� ���źҰ�
        if (gm.weaponUp > 3)
        {
            for (int i = 0; i < items[itemIdx].chgitem.Length; i++)
            {
                if (items[i].itemType == Item.ItemType.weapon)
                {
                    ColorRed();
                }
            }
        }
    }

    void ColorRed()
    {
        //��ư �� ���������� ����
        btn.interactable = false;
        cb.disabledColor = Color.red;
        btn.colors = cb;
        //������ �̹��� ���� ����
        Color color = itemImg.color;
        color.a = 0.5f;
        itemImg.color = color;
    }
    void ColorWhite()
    {
        //��ư�� ������� ����
        btn.interactable = true;
        cb.normalColor = Color.white;
        btn.colors = cb;
    }

    public void Buy()
    {
        oneDiceUI.SetActive(true);
        StartCoroutine(DiceRoll());
    }
    IEnumerator DiceRoll()
    {
        yield return new WaitForSeconds(2.0f);
        //�ֻ��� ������ ����
        if (gm.diceNum > 2)
        {
            successText.SetActive(true);
            //������ ����
            for (int i = 0; i < items[itemIdx].chgitem.Length; i++)
            {
                FieldInfo field = gm.GetType().GetField(items[itemIdx].chgitem[i]); //����� �����ۺ��� ��������
                object fieldValue = field.GetValue(gm); //���� �� ��������
                field.SetValue(gm, (float)fieldValue + items[itemIdx].value[i]); //�� ����
                gm.money -= items[itemIdx].itmePrice;
            }
            if (items[itemIdx].itemType == Item.ItemType.weapon)
            {
                gm.weaponUp++;
            }
        }
        //�ֻ��� ������ ����
        else
        {
            failText.SetActive(true);
            //�����ؽ�Ʈ �Ǵ� �Ҹ� ����
        }
        Invoke("DiceClose", 1.0f);
    }

    void DiceClose()
    {
        sell++;
        Bought();
        successText.SetActive(false);
        failText.SetActive(false);
        oneDiceUI.SetActive(false);
    }

    //���ſϷ� �Լ�
    void Bought()
    {
        if (sell == sellLimit)
        {
            btn.interactable = false;
            itemImg.sprite = soldoutImg;
            itemName.text = "����";
            itemDesc.text = "";
        }
        else
        {
            itemIdx = Random.Range(0, items.Length);
            itemImg.sprite = items[itemIdx].itemImg;
            itemName.text = items[itemIdx].itemName;
            itemDesc.text = items[itemIdx].itemDesc;
        }
    }
}
