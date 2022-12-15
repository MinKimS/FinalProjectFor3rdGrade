using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    public enum Dtype
    {
        //�������� ������ ����
        atkP,
        atkE,
        defP,
        defE,
        //�������� ����
        enMax,
        weapon,
        enMaxHP,
        pMaxHP
    }
    public Dtype dType; //�ֻ����� ������ �޴� ��

    public enum DUse
    {
        enter,
        stage,
        two
    }
    public DUse dUse;

    private int diceNum; //�ֻ��� ��
    private int diceImgIdx = 0;
    public Sprite[] diceImgs; //�ֻ����̹���
    Image diceImg; //�ֻ����̹���
    GameManager gm; //state ������ �ִ� Ŭ����
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
        //�ٽ� �ֻ����� ���ư��� �� ���� ������ �����ϱ� ���� ����
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

	//state �⺻ ������ ���󺹱�
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

    //�ֻ����� ������ �Լ�
    void RollDice()
    {
        //�ֻ��� �� ����
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

    //�������� ���۽��� �ֻ���
    void RollOneTimeDice()
    {
        RollDice();
        //state�� ������ �� ��
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
                //�̺�Ʈ ���̽��� �����
        }
        if (gm.curStage > 1) Invoke("HideDice", 3.0f);
        else Invoke("HideDice", 12.0f);
        //�������� �̵��� �������� ���� �ֻ����� hide�� �ȵǴ� ����
    }

    void HideDice()
    {
        gameObject.SetActive(false);
    }


    //�������� ������ �ֻ���
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
            //state�� ������ �� ��
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
