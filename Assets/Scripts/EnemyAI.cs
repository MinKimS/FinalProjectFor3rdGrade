using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        PA,
        ATTACK,
        STOP_ATTACK,
        DIE
    }
    public enum Type
    {
        BOSS,
        TA_CLOSE,
        TA_LONG
    }
    public Type type;
    public State state;
    public float attackDist = 1.0f; //공격 사정거리
    public float stopAttackDist = 2.0f; //멈춰서 공격 사정거리
    public bool isDie = false; //사망 여부
    Transform playerTr; //주인공 위치
    Transform enemyTr; //적 캐릭터 위치
    WaitForSeconds ws; //코루틴 지연변수
    MoveAgent moveAgent; //이동제어 클래스
    EnemyFire enemyFire; //총발사제어 클래스

    GameManager gm;

    public ParticleSystem dieEffect; //죽음 이펙트
    EDamageData ed;

    private void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

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
        ed = GetComponent<EDamageData>();
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }


    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(1.0f);
        while (!isDie)
        {
            if (state == State.DIE) yield break; //죽음

            float dist = Vector3.Distance(playerTr.position, enemyTr.position); //플레이어와의 거리

            if(state != State.PA)
            {
                if (dist <= stopAttackDist) state = State.STOP_ATTACK;
                else state = State.ATTACK;
            }

            yield return ws;
        }
    }

    IEnumerator Action()
    {
        while(!isDie)
        {
            yield return ws;

            //상태에 따른 행동
            switch (state)
            {
                //순찰하면서 공격
                case State.PA:
                    moveAgent.SetPatrolling(true);
                    if(type == Type.TA_LONG)
                    {
                        //움직이는 상태로 총알 발사
                        if (enemyFire.isFire == false)
                            enemyFire.isFire = true;
                    }
                    break;
                //원거리 공격 가능한 상태
                case State.ATTACK:
                    moveAgent.SetTraceTarget(playerTr.position); //플레이어를 추적
                    if (type == Type.TA_LONG || type == Type.BOSS)
                    {
                        //움직이는 상태로 총알 발사
                        if (enemyFire.isFire == false)
                            enemyFire.isFire = true;
                    }
                    break;
                //원거리 공격 시 멈춰서 공격하는 상태
                case State.STOP_ATTACK:
                    if (type == Type.TA_LONG || type == Type.BOSS)
                    {
                        //멈춘 상태로 총알 발사
                        if (enemyFire.isFire == false)
                            enemyFire.isFire = true;
                        moveAgent.Stop();
                    }
                    break;
                //죽은경우
                case State.DIE:
                    isDie = true;
                    gm.enemyCount--;
                    gm.killCount++; //처치한 적의 수 증가
                    if (type == Type.TA_LONG || type == Type.BOSS) enemyFire.isFire = false; //공격 멈춤
                    moveAgent.Stop(); //움직임 멈춤
                    dieEffect.Play(); //죽은 이펙트 재생
                    GetComponent<CapsuleCollider>().enabled = false;
                    gameObject.tag = "Untagged";
                    gm.killNum++; //적 처치 수 증가
                    //적의 종류&&현재스테이지에 따라 증가되는 돈 변화
                    //플레이어 돈 증가
                    switch(ed.etype)
                    {
                        case EDamageData.EType.PAWN:
                            gm.money += 3;
                            break;
                        case EDamageData.EType.KNIGHT:
                            gm.money += 5;
                            break;
                        case EDamageData.EType.BISHOP:
                            gm.money += 6;
                            break;
                        case EDamageData.EType.ROOK:
                            gm.money += 7;
                            break;
                        case EDamageData.EType.CARD:
                            gm.money += 10;
                            break;
                        case EDamageData.EType.QCARD:
                            gm.money += 20;
                            break;
                        case EDamageData.EType.JCARD:
                            gm.money += 30;
                            break;
                    }
                    Invoke("DieEnemy", 1.0f); //1초후 적 삭제
                    break;
            }
        }
    }
    
    void DieEnemy()
    {
        if (ed.etype == EDamageData.EType.BOSS)
        {
            gm.isGameClear = true;
        }
        Destroy(this.gameObject);
    }

    //플레이어가 죽은 경우 적의 행동 처리 함수
    public void OnPlayerDie()
    {
        if(!isDie)
        {
            moveAgent.Stop(); //움직임 멈춤
            if (type == Type.TA_LONG || type == Type.BOSS) enemyFire.isFire = false; //공격멈춤
            StopAllCoroutines();
        }
    }

    private void Update()
    {
        if (gm.isGameClear)
        {
            moveAgent.Stop();
            if (type == Type.TA_LONG || type == Type.BOSS) enemyFire.isFire = false; //공격멈춤
            StopAllCoroutines();
        }
    }
}
