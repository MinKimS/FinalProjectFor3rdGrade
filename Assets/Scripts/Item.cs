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
    public ItemType itemType;
    public string itemName;
    public Sprite itemImg;
    [TextArea] public string itemDesc;
    public string[] chgitem;
    public float[] value;
    public int itmePrice;
}
