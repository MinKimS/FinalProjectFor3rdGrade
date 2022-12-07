using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //���������� ��ȯ�� �� ������ ���� ����
    public enum StageType
    {
        chess,
        card,
        mix
    }
    public StageType sType;
    public Transform[] points; //�� ��������Ʈ
    public GameObject[] enemys; //��ȯ�� ����
    private float createTime = 2.0f; //�� ��ȯ ������ Ÿ��
    private int limitEnemy = 5;   // �ʹ� ���� ���� ��ȯ���� �ʵ��� �����ϱ� ���� ����
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

    public bool isEnter = true; //���ο� �������� ���忩��
    public GameObject DicePanel;
    public bool isSpawnOk = false;  //����üũ
    public int enemyCount = 0;

    //���ӿ���â
    public GameObject gmOverUI;
    //�� óġ ��
    public int killNum = 0;
    //������ ��
    public int money = 0;
    //���� ��������
    public int curStage = 1;

    void Start()
    {
        isEnter = true;
        //�� ��ȯ��ġ ����
        points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
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
    }

    //�� ����
    IEnumerator CreateEnemy(float maxSpawn)
    {
        while (!isGameOver && maxSpawn > 0)
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
