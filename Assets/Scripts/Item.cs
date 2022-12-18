using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        weapon,
        etc
    }
    public ItemType itemType; //무기강화인지 아닌지 구분하기 위한 용도
    public string itemName; //아이템 이름
    public Sprite itemImg; //아이템 이미지
    [TextArea] public string itemDesc; //아이템 설명
    public string[] chgitem; //변경되는 능력치를 적용할 때 사용하기 위한 변수
    public float[] value; //변경되는 능력치의 값
    public int itmePrice; //아이템 가격
}
