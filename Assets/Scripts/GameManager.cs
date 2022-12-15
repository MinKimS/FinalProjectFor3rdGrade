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
    public float atkE = 1.0f;
    public float defE = 0.1f;
    //Stage setting
    public int weapon = 0;
    public float enMax = 4.0f;
    public float pMaxHP = 100;
    public float enMaxHP = 100;

    public bool isEnter = true; //새로운 스테이지 입장여부
    public bool isSpawnOk = false;  //스폰체크
    public int enemyCount = 0;
    public int killCount = 0;

    //게임오버창
    public GameObject gmOverUI;
    //적 처치 수
    public int killNum = 0;
    //소유한 돈
    public int money = 0;
    //현재 스테이지
    public int curStage = 1;

    public GameObject shopUI; //상점UI
    public GameObject pPos; //플레이어
    public GameObject diceUI; //주사위UI
    public GameObject oneDiceUI; //한번굴리는 주사위UI
    public Text stage; //스테이지 텍스트
    public GameObject boss; //보스
    public GameObject ClearUI; //게임 클리어
    public GameObject OverUI; //게임 오버
    public Text st; //플레이어 능력치
    public GameObject[] spawnPos; //스폰될 위치 표시 오브젝트
    public GameObject[] dices;
    private Vector3 pSpawnPos; //플레이어 스폰 위치

    void Start()
    {
        pSpawnPos = new Vector3(0,0.26f,0);
        isEnter = true;
        //적 소환위치 저장
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        //소환표시 저장
    }

    private void Update()
    {
        //게임이 시작하면 몹 스폰
        if(!isEnter && isSpawnOk)
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
            if(curStage < 16)
                Shop();
            else //게임 클리어
			{

			}
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
        killCount = 0;
        isShop = true;
        diceUI.SetActive(false);
        shopUI.SetActive(true);
        //상점의 플레이어 능력치 표시
        //판매할아이템 설정
        //아이템 중에서 랜덤으로 하나를 선택
        //
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
        if(curStage == 15)
		{
            stage.text = "BOSS STAGE";
            sType = StageType.boss;
            boss.SetActive(true);
        }
        else
		{
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
        ShowDice();
        isEnter = true; //총변경시 총알도 설정
        //아닌경우
        //실패텍스트 또는 소리 내기
        //
    }

    //아이템 구매 함수
    public void Buy(int num)
    {
        //주사위 돌리기
        //주사위 돌리기 성공
        //아이템 구매함


        //주사위 돌리기 실패
        //실패텍스트 또는 소리 내기
    }

    //숨겨놨던 스테이지 주사위 보이기
    void ShowDice()
    {
        for(int i = 0; i<dices.Length; i++)
            dices[i].SetActive(true);
    }
}
