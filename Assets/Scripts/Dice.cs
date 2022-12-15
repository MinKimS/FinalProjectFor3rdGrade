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
        two
    }
    public DUse dUse;

    private int diceNum; //주사위 값
    private int diceImgIdx = 0;
    public Sprite[] diceImgs; //주사위이미지
    Image diceImg; //주사위이미지
    GameManager gm; //state 변수가 있는 클래스
    public float value;

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

    private void Awake()
    {
        diceImg = GetComponent<Image>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
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
    }
    void OnEnable()
    {
        if (dUse == DUse.enter)
        {
            RollOneTimeDice();
        }
        if (dUse == DUse.stage)
        {
            Invoke("Roll", 3.0f);
        }
        if(dUse == DUse.two)
		{
            RollDice();

        }
    }

    private void Update()
    {

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

	//state 기본 값으로 원상복구
	void stateClear()
    {
        //Player setting
        gm.atkP = atkP;
        gm.defP = defP;
        //Enemy setting
        gm.atkE = atkE;
        gm.defE = defE;
        //Stage setting
        gm.weapon = weapon;
        gm.enMax = enMax;
        gm.pMaxHP = pMaxHP;
        gm.enMaxHP = enMaxHP;
    }

    //주사위를 돌리는 함수
    void RollDice()
    {
        //주사위 값 결정
        diceImgIdx = Random.Range(0, 6);
        diceNum = diceImgIdx + 1;
        diceImg.sprite = diceImgs[diceImgIdx];
    }

    void setValue()
	{
        switch (diceNum)
        {
            case 1:
                value = 0.5f;
                break;
            case 2:
            case 5:
                value = 1.5f;
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
        RollDice();
        //state에 영향을 줄 값
        setValue();
        switch (dType)
        {
            case Dtype.enMax:
                gm.enMax = enMax;
                gm.enMax = enMax * value;
                break;
            case Dtype.weapon:
                gm.weapon = weapon;
                gm.weapon = diceImgIdx;
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
        if (gm.curStage > 1) Invoke("HideDice", 3.0f);
        else Invoke("HideDice", 12.0f);
        //스테이지 이동시 스테이지 설정 주사위가 hide라 안되는 문제
    }

    void HideDice()
    {
        gameObject.SetActive(false);
    }


    //스테이지 진행중 주사위
    IEnumerator RollRepeatDice()
    {
        gm.isEnter = false;
        while (true)
        {
            yield return new WaitForSeconds(8.0f);
            if (gm.isGameOver)
            {
                StopAllCoroutines();
            }
            RollDice();
            //state에 영향을 줄 값
            setValue();
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
        }
    }
}
