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
    private int limitEnemy = 7;   // 너무 많은 적이 소환되지 않도록 방지하기 위한 변수
    public bool isGameOver = false; //게임오버여부
    public bool isGameClear = false; //게임클리어여부
    public bool isShop = false; //상점인지 여부
    int stageUp = 0; //스테이지 상승하면서 올라가는 최대 적 스폰수

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
    public float chgMaxHp = 0; //변화되는 최대체력
    public float chgHp = 0; //체력회복되는 수치
    public int weaponUp = 0; //무기강화횟수
    public float diceNum;   //주사위 값
    private Vector3 pSpawnPos; //플레이어 스폰 위치
    public bool isAnim = false; //애니메이션 작동여부
    public bool isShowState = false; //능력치 창을 보여주고 있는지 여부

    public GameObject gmOverUI; //게임오버창
    public GameObject gmClearUI; //게임클리어창
    public GameObject shopUI; //상점UI
    public GameObject pPos; //플레이어
    public GameObject diceUI; //주사위UI
    public Text stage; //스테이지 텍스트
    public Text st; //상점에서 볼 수 있는 플레이어 능력치
    public Text st2; //플레이 도중에 확인하기 위한 능력치
    public GameObject[] spawnPos; //스폰될 위치 표시 오브젝트
    public GameObject[] dices;
    public Text moneyText; //돈 텍스트
    public AudioClip[] BGMs; //배경음악
    public AudioSource ads;
    public AudioClip[] aClip; //효과음
    public Mesh[] cardMesh;

    private void Awake()
    {
        Setstate();//초기 능력치 설정
        ads = GetComponent<AudioSource>();
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
        if (curHp > pMaxHP) //최대체력 초과 방지
            curHp = pMaxHP;
        //게임이 시작하면 몹 스폰
        if (!isEnter && isSpawnOk)
        {
            isSpawnOk = false;
            //적소환
            if (points.Length > 0)
            {
                StartCoroutine(CreateEnemy(enMax));
            }
        }

        //죽인 적의 수 == 스폰될 적의 수이면 상점 UI show
        //플레이어, 공격 정지
        if (killCount == enMax)
        {
            curStage++;
            stageUp += 3;

            if (curStage < 16)
            {
                Shop();
            }
        }
        //보스가 죽은경우
        //게임 클리어
        if (isGameClear) //주사위 숨기기, 모든 코루틴 정지, 게임 클리어 창 띄우기
        {
            gmClearUI.SetActive(true);
            StopAllCoroutines();
            diceUI.SetActive(false);
        }

        //게임오버
        if(isGameOver)
        {
            //모든 코루틴 정지, 주사위 숨기기
            StopAllCoroutines();
            diceUI.SetActive(false);
        }

        //상점인경우
        if (isShop)
        {
            //돈 텍스트 설정
            moneyText.text = "돈 : " + money;

            //상점의 플레이어 능력치 표시
            st.text = "공격력 : " + (2.0f + chgAtk) + "\n방어: " + (1.0f + chgDef) +
                "\n무기강화: " + weaponUp + "\n최대체력: " + (200 + chgMaxHp);
        }
        else
        {
            //플레이도중 플레이어 능력치 표시
            if (isShowState)
            {
                st2.text = "공격력 : " + atkP + "\n방어: " + defP +
                    "\n무기강화: " + weaponUp + "\n최대체력: " + pMaxHP + "\n현재체력: " + curHp;
            }
        }
    }

    //적 생성
    IEnumerator CreateEnemy(float maxSpawn)
    {
        while (!isGameOver && !isGameClear && maxSpawn > 0 && !isEnter)
        {
            //최대소환 가능횟수안에서 일정시간마다 적소환
            if (enemyCount < limitEnemy)
            {
                yield return new WaitForSeconds(createTime);
                int idx = Random.Range(1, points.Length); //스폰포인트 인덱스
                int enmIdx = 0; //적 소환 인덱스
                //스테이지 타입별 소환될 적 선택
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
                // 적 소환
                GameObject enmObj = Instantiate(enemys[enmIdx], points[idx].position, points[idx].rotation);
                //일반카드가 소환될때마다 카드 종류가 달라짐
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
        ads.clip = BGMs[2]; //상점 bgm설정
        ads.volume = 0.3f; //볼륨 조절
        killCount = 0;
        isShop = true;
        diceUI.SetActive(false);
        Invoke("ShopShow", 0.5f); //상점이 되자마자 구매 클릭 방지
        //상점의 플레이어 능력치 표시
        st.text = "공격력 : " + (atkP + chgAtk) + "\n방어: " + (defP + chgDef) + 
            "\n무기강화: " + weaponUp + "\n최대체력: " + pMaxHP + "\n현재체력: " + curHp + chgHp;
        moneyText.text = "돈 : " + money;//돈 텍스트 설정
        ads.Play(); //bgm실행
    }

    //상점 활성화
    void ShopShow()
    {
        shopUI.SetActive(true);
    }

    //다음 스테이지로 이동
    public void NextStage()
    {
        isShop = false;//상점 비활성화
        //플레이어 위치 초기화
        pPos.transform.position = pSpawnPos;
        shopUI.SetActive(false);
        diceUI.SetActive(true);
        //능력치랑 무기 등을 설정
        Setstate();
        //보스 스테이지 인 경우
        if (curStage == 15)
        {
            ads.clip = BGMs[0];//스테이지 BGM 설정
            ads.volume = 0.4f;
            stage.text = "BOSS STAGE"; //스테이지 명 보스스테이지로 설정
            sType = StageType.boss;
            Invoke("SpawnBoss", 3.0f); //보스스폰
        }
        else
        {
            ads.clip = BGMs[1];//스테이지 BGM 설정
            ads.volume = 0.15f;
            stage.text = "STAGE " + curStage; //스테이지 명 설정

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
        ShowDice(); //숨겨놨던 스테이지 주사위 활성화
        isEnter = true; //총변경시 총알도 설정
    }

    //보스소환
    void SpawnBoss()
    {
        Instantiate(enemys[7], new Vector3(-0.1f, 0, 0.5f), Quaternion.Euler(0, 180, 0));
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


    //hp설정 함수
    void SetHP()
    {
        if (curHp > pMaxHP || chgHp + curHp > pMaxHP) //최대체력 초과 방지
            curHp = pMaxHP;
        else
        {
            if (curHp + curHp < 0) //아이템으로 인한 체력 0 되는 것 방지
                curHp = 1;
            else curHp += chgHp;
        }
    }
}
