using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    Camera uiCamera; //Canvas 렌더링 카메라
    Canvas canvas; //UI용 캔버스
    RectTransform rectParent; //부모 RectTransform 컴포넌트
    RectTransform rectHp; //RectTransform 컴포넌트
    public Vector3 offset = Vector3.zero; //Hp바 이미지의 위치를 조절할 오프셋
    public GameObject target; //추적할 적 캐릭

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = canvas.GetComponent<RectTransform>();
        rectHp = gameObject.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position + offset); //월드 좌표를 스크린의 좌표로 변환
        //카메라의 뒷쪽 영역일때 좌푯값 보정
        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }
        Vector2 localPos = Vector2.zero; //좌표값을 전달받을 변수
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos); //스크린 좌표를 RectTransform 기준의 좌표로 변환
        //Hp 바 이미지의 위치를 변경
        rectHp.localPosition = localPos;
        //적 캐릭이 죽을 시 삭제
        if(target.GetComponent<EnemyAI>().isDie)
        {
            Destroy(this.gameObject);
        }
    }
}
