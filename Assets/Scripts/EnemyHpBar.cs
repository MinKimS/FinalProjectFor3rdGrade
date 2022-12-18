using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    Camera uiCamera; //Canvas ������ ī�޶�
    Canvas canvas; //UI�� ĵ����
    RectTransform rectParent; //�θ� RectTransform ������Ʈ
    RectTransform rectHp; //RectTransform ������Ʈ
    public Vector3 offset = Vector3.zero; //Hp�� �̹����� ��ġ�� ������ ������
    public GameObject target; //������ �� ĳ��

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = gameObject.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position + offset); //���� ��ǥ�� ��ũ���� ��ǥ�� ��ȯ
        //ī�޶��� ���� �����϶� ��ǩ�� ����
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }
        Vector2 localPos = Vector2.zero; //��ǥ���� ���޹��� ����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos); //��ũ�� ��ǥ�� RectTransform ������ ��ǥ�� ��ȯ
        //Hp �� �̹����� ��ġ�� ����
        rectHp.localPosition = localPos;
        //�� ĳ���� ���� �� ����
        if(target.GetComponent<EnemyAI>().isDie)
        {
            Destroy(this.gameObject);
        }
    }
}
