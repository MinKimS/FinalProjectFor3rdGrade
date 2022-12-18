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
    private int limitEnemy = 7;   // �ʹ� ���� ���� ��ȯ���� �ʵ��� �����ϱ� ���� ����
    public bool isGameOver = false; //���ӿ�������
    public bool isGameClear = false; //����Ŭ�����
    public bool isShop = false; //�������� ����
    int stageUp = 0; //�������� ����ϸ鼭 �ö󰡴� �ִ� �� ������

    //Player setting
    public float atkP = 2.0f;
    public float defP = 1.0f;
    //Enemy setting
    public float atkE = 1.0f;
    public float defE = 0.01f;
    //Stage setting
    public int weapon = 0;
    public float enMax = 10.0f;
    public float pMaxHP = 100;
    public float enMaxHP = 100;
    public float curHp; //�÷��̾� ���� ü��

    public bool isEnter = true; //���ο� �������� ���忩��
    public bool isSpawnOk = false;  //����üũ
    public int enemyCount = 0;
    public int killCount = 0;
    
    public int killNum = 0; //�� óġ ��
    public int money = 0; //������ ��
    public int curStage = 1; //���� ��������

    //���������� ���� ��ȭ�Ǵ� �ɷ�ġ
    public float chgAtk = 0; //��ȭ�Ǵ� ���ݷ�
    public float chgDef = 0; //��ȭ�Ǵ� ����
    public float chgMaxHp = 0; //��ȭ�Ǵ� �ִ�ü��
    public float chgHp = 0; //ü��ȸ���Ǵ� ��ġ
    public int weaponUp = 0; //���ⰭȭȽ��
    public float diceNum;   //�ֻ��� ��
    private Vector3 pSpawnPos; //�÷��̾� ���� ��ġ
    public bool isAnim = false; //�ִϸ��̼� �۵�����
    public bool isShowState = false; //�ɷ�ġ â�� �����ְ� �ִ��� ����

    public GameObject gmOverUI; //���ӿ���â
    public GameObject gmClearUI; //����Ŭ����â
    public GameObject shopUI; //����UI
    public GameObject pPos; //�÷��̾�
    public GameObject diceUI; //�ֻ���UI
    public Text stage; //�������� �ؽ�Ʈ
    public Text st; //�������� �� �� �ִ� �÷��̾� �ɷ�ġ
    public Text st2; //�÷��� ���߿� Ȯ���ϱ� ���� �ɷ�ġ
    public GameObject[] spawnPos; //������ ��ġ ǥ�� ������Ʈ
    public GameObject[] dices;
    public Text moneyText; //�� �ؽ�Ʈ
    public AudioClip[] BGMs; //�������
    public AudioSource ads;
    public AudioClip[] aClip; //ȿ����
    public Mesh[] cardMesh;

    private void Awake()
    {
        Setstate();//�ʱ� �ɷ�ġ ����
        ads = GetComponent<AudioSource>();
    }
    void Start()
    {
        ads.clip = BGMs[1];//�������� BGM ����
        ads.Play();//BGM �÷���
        curHp = pMaxHP;//���� hp�� �ִ�ü������ ����
        pSpawnPos = new Vector3(0,0.26f,0); //�÷��̾� ������ ���
        isEnter = true;
        //�� ��ȯ��ġ ����
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>(); //��ȯǥ�� ����
    }

    private void Update()
    {
        if (curHp > pMaxHP) //�ִ�ü�� �ʰ� ����
            curHp = pMaxHP;
        //������ �����ϸ� �� ����
        if (!isEnter && isSpawnOk)
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
            stageUp += 3;

            if (curStage < 16)
            {
                Shop();
            }
        }
        //������ �������
        //���� Ŭ����
        if (isGameClear) //�ֻ��� �����, ��� �ڷ�ƾ ����, ���� Ŭ���� â ����
        {
            gmClearUI.SetActive(true);
            StopAllCoroutines();
            diceUI.SetActive(false);
        }

        //���ӿ���
        if(isGameOver)
        {
            //��� �ڷ�ƾ ����, �ֻ��� �����
            StopAllCoroutines();
            diceUI.SetActive(false);
        }

        //�����ΰ��
        if (isShop)
        {
            //�� �ؽ�Ʈ ����
            moneyText.text = "�� : " + money;

            //������ �÷��̾� �ɷ�ġ ǥ��
            st.text = "���ݷ� : " + (2.0f + chgAtk) + "\n���: " + (1.0f + chgDef) +
                "\n���Ⱝȭ: " + weaponUp + "\n�ִ�ü��: " + (200 + chgMaxHp);
        }
        else
        {
            //�÷��̵��� �÷��̾� �ɷ�ġ ǥ��
            if (isShowState)
            {
                st2.text = "���ݷ� : " + atkP + "\n���: " + defP +
                    "\n���Ⱝȭ: " + weaponUp + "\n�ִ�ü��: " + pMaxHP + "\n����ü��: " + curHp;
            }
        }
    }

    //�� ����
    IEnumerator CreateEnemy(float maxSpawn)
    {
        while (!isGameOver && !isGameClear && maxSpawn > 0 && !isEnter)
        {
            //�ִ��ȯ ����Ƚ���ȿ��� �����ð����� ����ȯ
            if (enemyCount < limitEnemy)
            {
                yield return new WaitForSeconds(createTime);
                int idx = Random.Range(1, points.Length); //��������Ʈ �ε���
                int enmIdx = 0; //�� ��ȯ �ε���
                //�������� Ÿ�Ժ� ��ȯ�� �� ����
                switch(sType)
                {
                    case StageType.chess:
                        enmIdx = Random.Range(0, 3);
                        break;
                    case StageType.card:
                        if (curStage > 8)
                            enmIdx = Random.Range(4, enemys.Length - 1);
                        else
                            enmIdx = 4;
                        break;
                    case StageType.mix:
                    case StageType.boss:
                        enmIdx = Random.Range(0, enemys.Length-1);
                        break;
                }
                yield return new WaitForSeconds(1f);
                // �� ��ȯ
                GameObject enmObj = Instantiate(enemys[enmIdx], points[idx].position, points[idx].rotation);
                //�Ϲ�ī�尡 ��ȯ�ɶ����� ī�� ������ �޶���
                if (enmIdx == 4)
                {
                    MeshFilter mFilter = enmObj.GetComponent<MeshFilter>();
                    mFilter.sharedMesh = cardMesh[Random.Range(0, cardMesh.Length)];
                }
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
        ads.clip = BGMs[2]; //���� bgm����
        ads.volume = 0.3f; //���� ����
        killCount = 0;
        isShop = true;
        diceUI.SetActive(false);
        Invoke("ShopShow", 0.5f); //������ ���ڸ��� ���� Ŭ�� ����
        //������ �÷��̾� �ɷ�ġ ǥ��
        st.text = "���ݷ� : " + (atkP + chgAtk) + "\n���: " + (defP + chgDef) + 
            "\n���Ⱝȭ: " + weaponUp + "\n�ִ�ü��: " + pMaxHP + "\n����ü��: " + curHp + chgHp;
        moneyText.text = "�� : " + money;//�� �ؽ�Ʈ ����
        ads.Play(); //bgm����
    }

    //���� Ȱ��ȭ
    void ShopShow()
    {
        shopUI.SetActive(true);
    }

    //���� ���������� �̵�
    public void NextStage()
    {
        isShop = false;//���� ��Ȱ��ȭ
        //�÷��̾� ��ġ �ʱ�ȭ
        pPos.transform.position = pSpawnPos;
        shopUI.SetActive(false);
        diceUI.SetActive(true);
        //�ɷ�ġ�� ���� ���� ����
        Setstate();
        //���� �������� �� ���
        if (curStage == 15)
        {
            ads.clip = BGMs[0];//�������� BGM ����
            ads.volume = 0.4f;
            stage.text = "BOSS STAGE"; //�������� �� �������������� ����
            sType = StageType.boss;
            Invoke("SpawnBoss", 3.0f); //��������
        }
        else
        {
            ads.clip = BGMs[1];//�������� BGM ����
            ads.volume = 0.15f;
            stage.text = "STAGE " + curStage; //�������� �� ����

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
        ads.Play();//BGM �÷���
        ShowDice(); //���ܳ��� �������� �ֻ��� Ȱ��ȭ
        isEnter = true; //�Ѻ���� �Ѿ˵� ����
    }

    //������ȯ
    void SpawnBoss()
    {
        Instantiate(enemys[7], new Vector3(-0.1f, 0, 0.5f), Quaternion.Euler(0, 180, 0));
    }

    //���ܳ��� �������� �ֻ��� ���̱�
    void ShowDice()
    {
        for(int i = 0; i<dices.Length; i++)
            dices[i].SetActive(true);
    }

    //state �⺻ ������ ���󺹱�
    void Setstate()
    {
        atkP = 2.0f + chgAtk;
        defP = 1.0f + chgDef;
        atkE = 1.0f;
        defE = 0.1f;
        weapon = 0;
        enMax = 10.0f + stageUp;
        pMaxHP = 200 + chgMaxHp;
        SetHP();
        enMaxHP = 100;
    }


    //hp���� �Լ�
    void SetHP()
    {
        if (curHp > pMaxHP || chgHp + curHp > pMaxHP) //�ִ�ü�� �ʰ� ����
            curHp = pMaxHP;
        else
        {
            if (curHp + curHp < 0) //���������� ���� ü�� 0 �Ǵ� �� ����
                curHp = 1;
            else curHp += chgHp;
        }
    }
}
