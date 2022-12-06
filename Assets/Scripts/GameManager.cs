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
    public float atkP = 1;
    public float defP = 0;
    //Enemy setting
    public float atkE = 1;
    public float defE = 0;
    //Stage setting
    public int weapon = 0;
    public float enMax = 4;
    public float pMaxHP = 100;
    public float enMaxHP = 100;

    public bool isEnter = true; //���ο� �������� ���忩��
    public GameObject DicePanel;
    //public bool isRoll = false; //�������� ������ ���ư��� �ֻ����� ���ư����� ����

    void Start()
    {
        //�� ��ȯ
        //points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        //if (points.Length > 0)
        //{
        //    StartCoroutine(CreateEnemy(enMax)); ;
        //}
    }

    private void Update()
    {

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
