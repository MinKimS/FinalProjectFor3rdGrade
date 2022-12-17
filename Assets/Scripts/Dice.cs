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
        one
    }
    public DUse dUse;

    private int diceNum; //�ֻ��� ��
    private int diceIdx = 0;
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
    private Animator diceAnim;
    public AudioSource ads;

    private void Awake()
    {
        diceAnim = GetComponent<Animator>();
        diceImg = GetComponent<Image>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        ads = GetComponentInParent<AudioSource>();
    }

    void OnEnable()
    {
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

        if (dUse == DUse.enter)
        {
            RollOneTimeDice();
        }
        if (dUse == DUse.stage)
        {
            //Roll();
            Invoke("Roll", 3.0f);
        }
        if(dUse == DUse.one)
		{
            RollDice();
            setDiceNum();
        }
    }

    private void Update()
    {
        if(gm.isGameClear)
            StopAllCoroutines();
    }

    void setDiceNum()
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

    //�ֻ����� ������ �Լ�
    void RollDice()
    {
        diceAnim.SetBool("isRoll", true);
        ads.PlayOneShot(gm.aClip[1]);
        //�ֻ��� �� ����
        diceIdx = Random.Range(0, 6);
        diceNum = diceIdx + 1;
        Invoke("stopRollAnim", 1.0f);
    }
    void setAnimNum()
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
                //�̺�Ʈ ���̽��� �����
        }
        Invoke("HideDice", 3.0f);
        //�������� �̵��� �������� ���� �ֻ����� hide�� �ȵǴ� ����
    }

    void HideDice()
    {
        gameObject.SetActive(false);
        gm.isEnter = false;
    }


    //�������� ������ �ֻ���
    IEnumerator RollRepeatDice()
    {
        while (true)
        {
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
            yield return new WaitForSeconds(5.0f);
        }
    }

    void stopRollAnim()
    {
        setAnimNum();
        diceAnim.SetBool("isRoll", false);
        ads.PlayOneShot(gm.aClip[0]);
    }
}
