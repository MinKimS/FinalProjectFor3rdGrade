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
    public ItemType itemType; //���Ⱝȭ���� �ƴ��� �����ϱ� ���� �뵵
    public string itemName; //������ �̸�
    public Sprite itemImg; //������ �̹���
    [TextArea] public string itemDesc; //������ ����
    public string[] chgitem; //����Ǵ� �ɷ�ġ�� ������ �� ����ϱ� ���� ����
    public float[] value; //����Ǵ� �ɷ�ġ�� ��
    public int itmePrice; //������ ����
}
