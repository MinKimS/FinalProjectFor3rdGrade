using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    public float defaultMaxHp = 100.0f; //적 최대 체력
    public float eHp = 100; //적 체력
    Renderer c;
    Color curColor; //현재 색상 (색 변경용)
    public GameObject hpBarPrefab; //hp바 프리셋
    public Vector3 hpBarOffset = new Vector3(0, 0.6f, 0); //간격
    private Canvas uiCanvas; //부모가 될 canvas
    private Image hpBarImage; //변경될 hp바
    GameManager gm;

    public float gDamage = 1.0f; //총 데미지

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        c = gameObject.GetComponentInChildren<Renderer>();
        curColor = c.material.color;
        StartCoroutine(SetHPAndWeapon()); //체력과 총 데미지 설정
    }
    IEnumerator SetHPAndWeapon()
    {
        //체력 설정
        SetHpBar();
        //총 데미지 설정
        SetWeaponDamage();
        yield return null;//다음스테이지될때까지 대기(null 말고 조건그걸로 바꾸기)
    }

    void SetWeaponDamage()//총 종류에 따른 데미지 설정
	{
        switch (gm.weapon)
        {
            //권총
            case 0:
                gDamage = 42;
                break;
            //라이플
            case 1:
                gDamage = 28;
                break;
            //리볼버
            case 2:
                gDamage = 56;
                break;
            //샷건
            case 3:
                gDamage = 100;
                break;
            //SMG
            case 4:
                gDamage = 14;
                break;
            //스나이퍼
            case 5:
                gDamage = 70;
                break;
        }
    }
    void SetHpBar()
    {
        //hp설정
        eHp = defaultMaxHp + gm.enMaxHP;

        //hp ui 설정
        uiCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
        // ui canvas 하위로 생명 게이지를 생성
        GameObject hpBar = Instantiate(hpBarPrefab, uiCanvas.transform);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];
        //hp바가 따라가야할 대상과 오프셋 값 설정
        EnemyHpBar bar = hpBar.GetComponent<EnemyHpBar>();
        bar.target = gameObject;
        bar.offset = hpBarOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        //총알과 충돌한 경우
        if(other.tag == "Bullet")
        {
            //적의 색을 빨간색으로 변경하고 다시 원래대로 돌아오기
            c.material.color = Color.red;
            Invoke("ColorChange", 0.5f);
            //총알 삭제
            Destroy(other.gameObject);
            BulletCtrl bc = other.gameObject.GetComponent<BulletCtrl>();
            if (bc != null)
            {
                //적이 입는 데미지
                eHp -= (gm.atkP * gDamage) * (1 - gm.defE);
                //Debug.Log("Enemy Damage = " + gm.atkP + "x" + gDamage + "=" + (gm.atkP * gDamage));
                hpBarImage.fillAmount = eHp / (defaultMaxHp+gm.enMaxHP);    //적 hp ui에 반영
            }

            //적 체력이 0이하면 죽음 처리
            if (eHp <= 0.0f)
            {
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
                hpBarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
            }
        }
    }

    //원래 색상으로 변경
    void ColorChange()
    {
        c.material.color = curColor;
    }
}
