using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //���������� ��ȯ�� �� ������ ���� ����
    public enum StageType
    {
        chess,
        card,
        mix,
        boss
    }
    public StageType sType;
    public Transform[] points; //�� ��������Ʈ
    public GameObject[] enemys; //��ȯ�� ����
    private float createTime = 1.0f; //�� ��ȯ ������ Ÿ��
    private int limitEnemy = 5;   // �ʹ� ���� ���� ��ȯ���� �ʵ��� �����ϱ� ���� ����
    public bool isGameOver = false; //���ӿ�������
    public bool isGameClear = false; //����Ŭ�����
    public bool isShop = false;

    //Player setting
    public float atkP = 1.0f;
    public float defP = 0.1f;
    //Enemy setting
    public float atkE = 1.0f;
    public float defE = 0.1f;
    //Stage setting
    public int weapon = 0;
    public float enMax = 4.0f;
    public float pMaxHP = 100;
    public float enMaxHP = 100;

    public bool isEnter = true; //���ο� �������� ���忩��
    public bool isSpawnOk = false;  //����üũ
    public int enemyCount = 0;
    public int killCount = 0;

    //���ӿ���â
    public GameObject gmOverUI;
    //�� óġ ��
    public int killNum = 0;
    //������ ��
    public int money = 0;
    //���� ��������
    public int curStage = 1;

    public GameObject shopUI; //����UI
    public GameObject pPos; //�÷��̾�
    public GameObject diceUI; //�ֻ���UI
    public GameObject oneDiceUI; //�ѹ������� �ֻ���UI
    public Text stage; //�������� �ؽ�Ʈ
    public GameObject boss; //����
    public GameObject ClearUI; //���� Ŭ����
    public GameObject OverUI; //���� ����
    public Text st; //�÷��̾� �ɷ�ġ
    public GameObject[] spawnPos; //������ ��ġ ǥ�� ������Ʈ
    public GameObject[] dices;
    private Vector3 pSpawnPos; //�÷��̾� ���� ��ġ

    void Start()
    {
        pSpawnPos = new Vector3(0,0.26f,0);
        isEnter = true;
        //�� ��ȯ��ġ ����
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        //��ȯǥ�� ����
    }

    private void Update()
    {
        //������ �����ϸ� �� ����
        if(!isEnter && isSpawnOk)
        {
            isSpawnOk = false;
            //����ȯ
            if (points.Length > 0)
            {
                StartCoroutine(CreateEnemy(enMax));
            }
        }

        //���� ���� �� == ������ ���� ���̸� ���� UI show
        //�÷��̾�, ���� ����
        if (killCount == enMax)
        {
            curStage++;
            if(curStage < 16)
                Shop();
            else //���� Ŭ����
			{

			}
        }
    }

    //�� ����
    IEnumerator CreateEnemy(float maxSpawn)
    {
        while (!isGameOver && !isGameClear && maxSpawn > 0)
        {
            //�ִ��ȯ ����Ƚ���ȿ��� �����ð����� ����ȯ
            if (enemyCount < limitEnemy)
            {
                yield return new WaitForSeconds(createTime);
                int idx = Random.Range(1, points.Length); //��������Ʈ �ε���
                int enmIdx = 0; //�� ��ȯ �ε���
                //��ȯ�� �� ����
                switch(sType)
                {
                    case StageType.chess:
                        enmIdx = Random.Range(0, 3);
                        break;
                    case StageType.card:
                        enmIdx = Random.Range(4, enemys.Length-1);
                        break;
                    case StageType.mix:
                    case StageType.boss:
                        enmIdx = Random.Range(0, enemys.Length-1);
                        break;
                }
                //������ġǥ�� Ȱ��ȭ�ϱ�
                yield return new WaitForSeconds(1f);
                //������ġǥ�� ��Ȱ��ȭ�ϱ�
                Instantiate(enemys[enmIdx], points[idx].position, points[idx].rotation);
                enemyCount++;
                maxSpawn--;
            }
            else
            {
                yield return null;
            }
        }
    }
    
    void Shop()
    {
        killCount = 0;
        isShop = true;
        diceUI.SetActive(false);
        shopUI.SetActive(true);
        //������ �÷��̾� �ɷ�ġ ǥ��
        //�Ǹ��Ҿ����� ����
        //������ �߿��� �������� �ϳ��� ����
        //
    }

    public void NextStage()
    {
        //�ֻ���������
        //�ֻ����� ������ɰ��̻��� ���
        isShop = false;
        //�÷��̾� ��ġ �ʱ�ȭ
        pPos.transform.position = pSpawnPos;
        shopUI.SetActive(false);
        diceUI.SetActive(true);
        if(curStage == 15)
		{
            stage.text = "BOSS STAGE";
            sType = StageType.boss;
            boss.SetActive(true);
        }
        else
		{
            stage.text = "STAGE " + curStage;

            //��ȯ�� �� ���������� ����
            if (curStage > 10)
            {
                print("mix");
                sType = StageType.mix;
            }
            else if (curStage > 5)
            {
                print("card");
                sType = StageType.card;
            }
            else
            {
                print("chess");
                sType = StageType.chess;
            }
        }
        ShowDice();
        isEnter = true; //�Ѻ���� �Ѿ˵� ����
        //�ƴѰ��
        //�����ؽ�Ʈ �Ǵ� �Ҹ� ����
        //
    }

    //������ ���� �Լ�
    public void Buy(int num)
    {
        //�ֻ��� ������
        //�ֻ��� ������ ����
        //������ ������


        //�ֻ��� ������ ����
        //�����ؽ�Ʈ �Ǵ� �Ҹ� ����
    }

    //���ܳ��� �������� �ֻ��� ���̱�
    void ShowDice()
    {
        for(int i = 0; i<dices.Length; i++)
            dices[i].SetActive(true);
    }
}
