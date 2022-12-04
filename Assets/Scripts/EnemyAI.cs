using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    public enum Type
    {
        P,
        PTA,
        TA
    }
    public Type type;
    public State state;
    //���� �����Ÿ�
    public float attackDist = 1.0f;
    //���� �����Ÿ�
    public float traceDist = 3.0f;
    //��� ����
    public bool isDie = false;
    //���ΰ� ��ġ
    Transform playerTr;
    //�� ĳ���� ��ġ
    Transform enemyTr;
    //�ڷ�ƾ ��������
    WaitForSeconds ws;
    //�̵����� Ŭ����
    MoveAgent moveAgent;


    private void Awake()
    {
        if (type == Type.TA) state = State.TRACE;
        else state = State.PATROL;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            playerTr = player.transform;
        }
        //�� ��ġ ����
        enemyTr = GetComponent<Transform>();
        //�̵����� Ŭ���� ����
        moveAgent = GetComponent<MoveAgent>();
        ws = new WaitForSeconds(0.3f);
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        while (!isDie)
        {
            if (state == State.DIE) yield break;

            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            if (type == Type.P)
            {
                state = State.PATROL;
            }
            else
            {
                if (dist <= attackDist) state = State.ATTACK;
                else if (dist <= traceDist) state = State.TRACE;
                else if (type == Type.PTA)
                {
                    state = State.PATROL;
                }
            }

            yield return ws;
        }
    }

    IEnumerator Action()
    {
        while(!isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.PATROL:
                    moveAgent.SetPatrolling(true);
                    break;
                case State.TRACE:
                    moveAgent.SetTraceTarget(playerTr.position);
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    break;
                case State.DIE:
                    moveAgent.Stop();
                    break;
            }
        }
    }
}
