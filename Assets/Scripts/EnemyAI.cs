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
    public enum Range
    {
        LONG,
        CLOSE
    }
    public Type type;
    public State state;
    public Range range;
    //공격 사정거리
    public float attackDist = 1.0f;
    //추적 사정거리
    public float traceDist = 3.0f;
    //사망 여부
    public bool isDie = false;
    //주인공 위치
    Transform playerTr;
    //적 캐릭터 위치
    Transform enemyTr;
    //코루틴 지연변수
    WaitForSeconds ws;
    //이동제어 클래스
    MoveAgent moveAgent;
    //총발사제어 클래스
    EnemyFire enemyFire;

    private void Awake()
    {
        if (type == Type.TA) state = State.TRACE;
        else state = State.PATROL;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            playerTr = player.transform;
        }
        //적 위치 추출
        enemyTr = GetComponent<Transform>();
        //이동제어 클래스 추출
        moveAgent = GetComponent<MoveAgent>();
        //총발사제어 클래스 추출
        enemyFire = GetComponent<EnemyFire>();
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
                    if(range == Range.LONG) enemyFire.isFire = false;
                    moveAgent.SetPatrolling(true);
                    break;
                case State.TRACE:
                    if (range == Range.LONG) enemyFire.isFire = false;
                    moveAgent.SetTraceTarget(playerTr.position);
                    break;
                case State.ATTACK:
                    if(range == Range.LONG)
                    {
                        //멈춘 상태로 총알 발사
                        if (enemyFire.isFire == false)
                            enemyFire.isFire = true;
                        moveAgent.Stop();
                    }
                    break;
                case State.DIE:
                    isDie = true;
                    if (range == Range.LONG) enemyFire.isFire = false;
                    moveAgent.Stop();
                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
            }
        }
    }

    public void OnPlayerDie()
    {
        if(isDie == false)
        {
            moveAgent.Stop();
            if (range == Range.LONG) enemyFire.isFire = false;
            StopAllCoroutines();
        }
    }
}
