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
        BOSS,
        DIE
    }
    public enum Type
    {
        PA,
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
    //�ѹ߻����� Ŭ����
    EnemyFire enemyFire;

    GameManager gm;

    public ParticleSystem dieEffect; //���� ����Ʈ

    private void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        //��ȯ�� ������� ����
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
        //�ѹ߻����� Ŭ���� ����
        enemyFire = GetComponent<EnemyFire>();
        ws = new WaitForSeconds(0.3f);
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());

        if(state == State.BOSS)
		{
            //�����ð����� ���ϱ�(�ڷ�ƾ)
            //�����ð����� ���� ���� �߻�(�ڷ�ƾ)
        }
    }


    IEnumerator CheckState()
    {
        while (!isDie)
        {
            if (state == State.DIE) yield break;

            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            if (type == Type.PA)
            {
                state = State.BOSS;
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
                case State.BOSS:
                    moveAgent.SetPatrolling(true);
                    enemyFire.isFire = true;
                    break;
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
                        //���� ���·� �Ѿ� �߻�
                        if (enemyFire.isFire == false)
                            enemyFire.isFire = true;
                        moveAgent.Stop();
                    }
                    break;
                case State.DIE:
                    isDie = true;
                    gm.enemyCount--;
                    gm.killCount++;
                    if (range == Range.LONG) enemyFire.isFire = false;
                    moveAgent.Stop();
                    dieEffect.Play();
                    GetComponent<CapsuleCollider>().enabled = false;
                    gameObject.tag = "Untagged";
                    //�� óġ �� ����
                    gm.killNum++;
                    //�÷��̾� �� ����
                    gm.money += 5;
                    Invoke("DieColor", 1.0f);
                    break;
            }
        }
    }
    
    void DieColor()
    {
        Destroy(this.gameObject);
    }

    public void OnPlayerDie()
    {
        if(!isDie)
        {
            moveAgent.Stop();
            if (range == Range.LONG) enemyFire.isFire = false;
            StopAllCoroutines();
        }
    }
}
