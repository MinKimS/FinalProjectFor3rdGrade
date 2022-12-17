using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //스테이지별 소환될 적 설정을 위한 변수
    public enum StageType
    {
        chess,
        card,
        mix,
        boss
    }
    public StageType sType;

    public Transform[] points; //적 스폰포인트
    public GameObject[] enemys; //소환될 적들
    private float createTime = 1.0f; //적 소환 딜레이 타임
    private int limitEnemy = 5;   // 너무 많은 적이 소환되지 않도록 방지하기 위한 변수
    public bool isGameOver = false; //게임오버여부
    public bool isGameClear = false; //게임클리어여부
    public bool isShop = false;

    //Player setting
    public float atkP = 1.0f;
    public float defP = 0.1f;
    //Enemy setting
    public float atkE = 5.0f;
    public float defE = 0.1f;
    //Stage setting
    public int weapon = 0;
    public float enMax = 4.0f;
    public float pMaxHP = 100;
    public float enMaxHP = 100;
    public float curHp; //플레이어 현재 체력

    public bool isEnter = true; //새로운 스테이지 입장여부
    public bool isSpawnOk = false;  //스폰체크
    public int enemyCount = 0;
    public int killCount = 0;
    
    public int killNum = 0; //적 처치 수
    public int money = 0; //소유한 돈
    public int curStage = 1; //현재 스테이지

    //아이템으로 인해 변화되는 능력치
    public float chgAtk = 0; //변화되는 공격력
    public float chgDef = 0; //변화되는 방어력
    public float chgMaxHP = 0; //변화되는 최대체력
    public float recoverHP = 0; //체력회복되는 수치
    public int weaponUp = 0; //무기강화횟수
    public float diceNum;   //주사위 값
    private Vector3 pSpawnPos; //플레이어 스폰 위치
    private bool stageUp = false;

    public GameObject gmOverUI; //게임오버창
    public GameObject gmClearUI; //게임클리어창
    public GameObject shopUI; //상점UI
    public GameObject pPos; //플레이어
    public GameObject diceUI; //주사위UI
    public Text stage; //스테이지 텍스트
    public GameObject boss; //보스
    public Text st; //플레이어 능력치
    public GameObject[] spawnPos; //스폰될 위치 표시 오브젝트
    public GameObject[] dices;
    public Text moneyText; //돈 텍스트
    public AudioClip[] BGMs; //배경음악
    public AudioSource ads;
    public AudioClip[] aClip; //효과음
    private void Awake()
    {
        ads = GetComponent<AudioSource>();
        Setstate();//초기 능력치 설정
    }
    void Start()
    {
        ads.clip = BGMs[1];//스테이지 BGM 설정
        ads.Play();//BGM 플레이
        curHp = pMaxHP;//현재 hp를 최대체력으로 설정
        pSpawnPos = new Vector3(0,0.26f,0); //플레이어 리스폰 장소
        isEnter = true;
        //적 소환위치 저장
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>(); //소환표시 저장
    }

    private void Update()
    {
        //게임이 시작하면 몹 스폰
        if (!isEnter && isSpawnOk)
        {
            isSpawnOk = false;
            //적소환
            if (points.Length > 0)
            {
                //StartCoroutine(CreateEnemy(enMax));
            }
        }

        //죽인 적의 수 == 스폰될 적의 수이면 상점 UI show
        //플레이어, 공격 정지
        if (killCount == enMax)
        {
            print("test");
            curStage++;

            if (curStage < 16)
            {
                Shop();
            }
            else //게임 클리어
            {
                isGameClear = true;
                gmClearUI.SetActive(true);
                StopAllCoroutines();
            }
        }
        //보스가 죽은경우
        //게임클리어(위에있는거 가져오기)

        if (isShop)
        {
            //돈 텍스트 설정
            moneyText.text = "돈 : " + money;

            //상점의 플레이어 능력치 표시
            st.text = "공격력 : " + (1.0f + chgAtk) + "\n방어: " + (0.1f + chgDef) +
                "\n무기강화: " + weaponUp + "\n최대체력: " + (100 + pMaxHP) + "\n현재체력: " + curHp;
        }
    }

    //적 생성
    IEnumerator CreateEnemy(float maxSpawn)
    {
        while (!isGameOver && !isGameClear && maxSpawn > 0)
        {
            //최대소환 가능횟수안에서 일정시간마다 적소환
            if (enemyCount < limitEnemy)
            {
                yield return new WaitForSeconds(createTime);
                int idx = Random.Range(1, points.Length); //스폰포인트 인덱스
                int enmIdx = 0; //적 소환 인덱스
                //소환될 적 선택
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
                //스폰위치표시 활성화하기
                yield return new WaitForSeconds(1f);
                //스폰위치표시 비활성화하기
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
        ads.clip = BGMs[2];
        ads.volume = 0.3f;
        killCount = 0;
        isShop = true;
        diceUI.SetActive(false);
        shopUI.SetActive(true);
        //상점의 플레이어 능력치 표시
        st.text = "공격력 : " + (atkP + chgAtk) + "\n방어: " + (defP + chgDef) + 
            "\n무기강화: " + weaponUp + "\n최대체력: " + pMaxHP + "\n현재체력: " + curHp;
        moneyText.text = "돈 : " + money;//돈 텍스트 설정
        ads.Play();
    }

    public void NextStage()
    {
        //주사위굴리기
        //주사위가 통과가능값이상인 경우
        isShop = false;
        //플레이어 위치 초기화
        pPos.transform.position = pSpawnPos;
        shopUI.SetActive(false);
        diceUI.SetActive(true);
        //능력치랑 무기 등을 설정
        Setstate();
        if (curStage == 15)
        {
            ads.clip = BGMs[0];//스테이지 BGM 설정
            ads.volume = 0.4f;
            stage.text = "BOSS STAGE";
            sType = StageType.boss;
            boss.SetActive(true);
        }
        else
        {
            ads.clip = BGMs[1];//스테이지 BGM 설정
            ads.volume = 0.15f;
            stage.text = "STAGE " + curStage;

            //소환될 몹 스테이지별 설정
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
        ads.Play();//BGM 플레이
        ShowDice();
        isEnter = true; //총변경시 총알도 설정
        //아닌경우
        //실패텍스트 또는 소리 내기
        //
    }

    //숨겨놨던 스테이지 주사위 보이기
    void ShowDice()
    {
        for(int i = 0; i<dices.Length; i++)
            dices[i].SetActive(true);
    }

    //state 기본 값으로 원상복구
    void Setstate()
    {
        //Player setting
        atkP = 1.0f + chgAtk;
        defP = 0.1f + chgDef;
        curHp += recoverHP;
        //Enemy setting
        atkE = 5.0f;
        defE = 0.1f;
        //Stage setting
        weapon = 0;
        enMax = 4.0f;
        pMaxHP = 100 + chgMaxHP;
        enMaxHP = 100;
    }
}
