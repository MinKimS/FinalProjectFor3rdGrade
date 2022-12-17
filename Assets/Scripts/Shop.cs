using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Item[] items;//아이템 리스트
    public Image itemImg;//아이템 이미지
    public Text itemName;//아이템 이름
    public Text itemDesc;//아이템 설명
    public int itemIdx = 0;//아이템인덱스
    GameManager gm;
    public Sprite soldoutImg;//매진이미지
    private int sell=0;//아이템 판매 횟수
    private int sellLimit = 3;  //최대 아이템 판매개수

    public GameObject oneDiceUI; //한번굴리는 주사위UI
    public GameObject successText; //주사위 성공 텍스트
    public GameObject failText; //주사위 실패 텍스트
    private Button btn; //버튼 컴포넌트
    private ColorBlock cb; //버튼 색상

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

        //구매가 가능하면
        if (gm.money >= items[itemIdx].itmePrice && gm.weaponUp < 4)
        {
            ColorWhite();
        }
        //돈부족으로 구매가 불가능하면
        else
        {
            ColorRed();
        }
    }

    private void Update()
    {
        //돈 부족시 구매불가
        if(gm.money < items[itemIdx].itmePrice)
        {
            ColorRed();
        }
        //무기강화 총 3번할 시 구매불가
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
        //버튼 색 빨간색으로 설정
        btn.interactable = false;
        cb.disabledColor = Color.red;
        btn.colors = cb;
        //아이템 이미지 투명도 조정
        Color color = itemImg.color;
        color.a = 0.5f;
        itemImg.color = color;
    }
    void ColorWhite()
    {
        //버튼색 흰색으로 설정
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
        //주사위 돌리기 성공
        if (gm.diceNum > 2)
        {
            successText.SetActive(true);
            //아이템 구매
            for (int i = 0; i < items[itemIdx].chgitem.Length; i++)
            {
                FieldInfo field = gm.GetType().GetField(items[itemIdx].chgitem[i]); //변경될 아이템변수 가져오기
                object fieldValue = field.GetValue(gm); //기존 값 가져오기
                field.SetValue(gm, (float)fieldValue + items[itemIdx].value[i]); //값 변경
                gm.money -= items[itemIdx].itmePrice;
            }
            if (items[itemIdx].itemType == Item.ItemType.weapon)
            {
                gm.weaponUp++;
            }
        }
        //주사위 돌리기 실패
        else
        {
            failText.SetActive(true);
            //실패텍스트 또는 소리 내기
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

    //구매완료 함수
    void Bought()
    {
        if (sell == sellLimit)
        {
            btn.interactable = false;
            itemImg.sprite = soldoutImg;
            itemName.text = "매진";
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
