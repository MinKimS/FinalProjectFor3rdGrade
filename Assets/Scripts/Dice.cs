using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public enum Dtype
    {
        //스테이지 진행중 설정
        atkP,
        atkE,
        defP,
        defE,
        //스테이지 설정
        enMax,
        weapon,
        enMaxHP,
        pMaxHP
    }
    public Dtype dType; //주사위의 영향을 받는 것

    public enum DUse
    {
        enter,
        stage,
        one
    }
    public DUse dUse;

    private int diceNum; //주사위 값
    private int diceIdx = 0;
    GameManager gm; //state 변수가 있는 클래스
    public float value; //주사위 값에 따른 영향을 주는 값

    //Player setting
    public float atkP;
    public float defP;
    //Enemy setting
    public float atkE;
    public float defE;
    //Stage setting
    public int weapon;
    public float enMax;
    public float pMaxHP;
    public float enMaxHP;
    
    private Animator diceAnim;//주사위 애니메이션
    public AudioSource ads; //주사위 효과음을 재생할 오디오소스

    private void Awake()
    {
        diceAnim = GetComponent<Animator>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        ads = GetComponentInParent<AudioSource>();
    }

    void OnEnable()
    {
        //다시 주사위가 돌아갔을 때 원래 값에서 설정하기 위한 변수
        //Player setting
        atkP = gm.atkP;
        defP = gm.defP;
        //Enemy setting
        atkE = gm.atkE;
        defE = gm.defE;
        //Stage setting
        weapon = gm.weapon;
        enMax = gm.enMax;
        pMaxHP = gm.pMaxHP;
        enMaxHP = gm.enMaxHP;

        //주사위별로 굴리는 방식 선택
        if (dUse == DUse.enter)
        {
            RollOneTimeDice();//한번만 주사위가 돌아감
        }
        if (dUse == DUse.stage)
        {
            //Roll();
            Invoke("Roll", 3.0f);//일정시간마다 돌아갈 주사위
        }
        if(dUse == DUse.one) //성공과 실패를 확인할 한개의 주사위
		{
            RollDice(); //주사위 굴리기
            SetDiceNum(); //주사위 값 설정
        }
    }

    private void Update()
    {
        //게임 클리어시 모든 주사위 정지
        if(gm.isGameClear)
            StopAllCoroutines();
    }

    //주사위 숫자 설정
    void SetDiceNum()
    {
        gm.diceNum = diceNum;
    }

    void Roll()
    {
        gm.isSpawnOk = true;
        StartCoroutine(RollRepeatDice());
    }

	private void OnDisable()
	{
        StopAllCoroutines();
	}

    //주사위를 돌리는 함수
    void RollDice()
    {
        diceAnim.SetBool("isRoll", true); //애니메이션 실행
        gm.isAnim = true;
        ads.PlayOneShot(gm.aClip[1]);
        //주사위 값 결정
        diceIdx = Random.Range(0, 6);
        diceNum = diceIdx + 1;
        Invoke("stopRollAnim", 1.0f); //애니메이션 정지
    }

    //주사위 애니메이션 설정
    void SetAnimNum()
    {
        switch(diceNum)
        {
            case 1:
                diceAnim.SetInteger("DiceNum", 1);
                break;
            case 2:
                diceAnim.SetInteger("DiceNum", 2);
                break;
            case 3:
                diceAnim.SetInteger("DiceNum", 3);
                break;
            case 4:
                diceAnim.SetInteger("DiceNum", 4);
                break;
            case 5:
                diceAnim.SetInteger("DiceNum", 5);
                break;
            case 6:
                diceAnim.SetInteger("DiceNum", 6);
                break;
        }
    }
    void setValue()
    {
        switch (diceNum)
        {
            case 1:
                value = 0.1f;
                break;
            case 2:
            case 5:
                value = 0.5f;
                break;
            case 3:
                value = 1.0f;
                break;
            case 4:
                value = 0.75f;
                break;
            case 6:
                value = 2.0f;
                break;
        }
    }

    //스테이지 시작시의 주사위
    void RollOneTimeDice()
    {
        RollDice(); //주사위 돌리기
        //state에 영향을 줄 값
        setValue();
        //값 설정
        switch (dType)
        {
            case Dtype.enMax:
                gm.enMax = enMax;
                gm.enMax = Mathf.Floor(enMax * value);
                break;
            case Dtype.weapon:
                gm.weapon = weapon;
                gm.weapon = diceIdx;
                break;
            case Dtype.enMaxHP:
                gm.enMaxHP = enMaxHP;
                gm.enMaxHP = enMaxHP * value;
                break;
            case Dtype.pMaxHP:
                gm.pMaxHP = pMaxHP;
                gm.pMaxHP = pMaxHP * value;
                break;
                //이벤트 다이스도 만들기
        }
        Invoke("HideDice", 3.0f);
        //스테이지 이동시 스테이지 설정 주사위가 hide라 안되는 문제
    }

    //주사위 숨기기(한번 굴리는 주사위 용)
    void HideDice()
    {
        gameObject.SetActive(false);
        gm.isEnter = false;
    }


    //스테이지 진행중 주사위
    IEnumerator RollRepeatDice()
    {
        while (true)
        {
            //게임오버시 모든 주사위 코루틴 정지
            if (gm.isGameOver)
            {
                StopAllCoroutines();
            }
            RollDice();
            //state에 영향을 줄 값
            setValue();

            //값 지정
            switch (dType)
            {
                case Dtype.atkP:
                    gm.atkP = atkP * value;
                    break;
                case Dtype.atkE:
                    gm.atkE = atkE * value;
                    break;
                case Dtype.defP:
                    gm.defP = defP * value;
                    break;
                case Dtype.defE:
                    gm.defE = defE * value;
                    break;
            }
            yield return new WaitForSeconds(5.0f);
        }
    }

    //애니메이션 정지
    void stopRollAnim()
    {
        SetAnimNum();
        diceAnim.SetBool("isRoll", false);
        gm.isAnim = false;
        //주사위 결과가 나오는 효과음
        ads.PlayOneShot(gm.aClip[0]);
    }
}
