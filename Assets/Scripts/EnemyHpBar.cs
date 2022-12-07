using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    Camera uiCamera;
    Canvas canvas;
    RectTransform rectParent;
    RectTransform rectHp;
    public Vector3 offset = Vector3.zero;
    public GameObject target;
    GameManager gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = gameObject.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position + offset);
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }
        Vector2 localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);
        rectHp.localPosition = localPos;
        if(target.GetComponent<EnemyAI>().isDie)
        {
            Destroy(this.gameObject);
        }
    }
}
