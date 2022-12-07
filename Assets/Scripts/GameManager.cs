using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //스테이지별 소환될 적 설정을 위한 변수
    public enum StageType
    {
        chess,
        card,
        mix
    }
    public StageType sType;
    public Transform[] points; //적 스폰포인트
    public GameObject[] enemys; //소환될 적들
    private float createTime = 2.0f; //적 소환 딜레이 타임
    private int limitEnemy = 5;   // 너무 많은 적이 소환되지 않도록 방지하기 위한 변수
    public bool isGameOver = false;

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
    public GameObject DicePanel;
    public bool isSpawnOk = false;  //스폰체크
    public int enemyCount = 0;

    //게임오버창
    public GameObject gmOverUI;
    //적 처치 수
    public int killNum = 0;
    //소유한 돈
    public int money = 0;
    //현재 스테이지
    public int curStage = 1;

    void Start()
    {
        isEnter = true;
        //적 소환위치 저장
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
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
    }

    //적 생성
    IEnumerator CreateEnemy(float maxSpawn)
    {
        while (!isGameOver && maxSpawn > 0)
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
                        enmIdx = Random.Range(4, enemys.Length);
                        break;
                    case StageType.mix:
                        enmIdx = Random.Range(0, enemys.Length);
                        break;
                }
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

}
