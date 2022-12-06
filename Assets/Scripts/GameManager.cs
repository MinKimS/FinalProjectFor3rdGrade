using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform[] points;
    public GameObject enemy;
    public float createTime = 2.0f;
    public int maxEnemy = 10;
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
    //public bool isRoll = false; //스테이지 진행중 돌아가는 주사위가 돌아가는지 여부
    public bool isSpawnOk = false;

    void Start()
    {
        isEnter = true;
        //적 소환위치 저장
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
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

    IEnumerator CreateEnemy(float maxSpawn)
    {
        while (!isGameOver && maxSpawn > 0)
        {
            int enemyCount = (int)GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (enemyCount < maxEnemy)
            {
                yield return new WaitForSeconds(createTime);
                int idx = Random.Range(1, points.Length);
                Instantiate(enemy, points[idx].position, points[idx].rotation);
            }
            else
            {
                yield return null;
            }
            maxSpawn--;
        }
    }

}
