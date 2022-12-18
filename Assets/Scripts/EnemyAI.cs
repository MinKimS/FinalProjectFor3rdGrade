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
    public float attackDist = 1.0f; //���� �����Ÿ�
    public float stopAttackDist = 2.0f; //���缭 ���� �����Ÿ�
    public bool isDie = false; //��� ����
    Transform playerTr; //���ΰ� ��ġ
    Transform enemyTr; //�� ĳ���� ��ġ
    WaitForSeconds ws; //�ڷ�ƾ ��������
    MoveAgent moveAgent; //�̵����� Ŭ����
    EnemyFire enemyFire; //�ѹ߻����� Ŭ����

    GameManager gm;

    public ParticleSystem dieEffect; //���� ����Ʈ
    EDamageData ed;

    private void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

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
            if (state == State.DIE) yield break; //����

            float dist = Vector3.Distance(playerTr.position, enemyTr.position); //�÷��̾���� �Ÿ�

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

            //���¿� ���� �ൿ
            switch (state)
            {
                //�����ϸ鼭 ����
                case State.PA:
                    moveAgent.SetPatrolling(true);
                    if(type == Type.TA_LONG)
                    {
                        //�����̴� ���·� �Ѿ� �߻�
                        if (enemyFire.isFire == false)
                            enemyFire.isFire = true;
                    }
                    break;
                //���Ÿ� ���� ������ ����
                case State.ATTACK:
                    moveAgent.SetTraceTarget(playerTr.position); //�÷��̾ ����
                    if (type == Type.TA_LONG || type == Type.BOSS)
                    {
                        //�����̴� ���·� �Ѿ� �߻�
                        if (enemyFire.isFire == false)
                            enemyFire.isFire = true;
                    }
                    break;
                //���Ÿ� ���� �� ���缭 �����ϴ� ����
                case State.STOP_ATTACK:
                    if (type == Type.TA_LONG || type == Type.BOSS)
                    {
                        //���� ���·� �Ѿ� �߻�
                        if (enemyFire.isFire == false)
                            enemyFire.isFire = true;
                        moveAgent.Stop();
                    }
                    break;
                //�������
                case State.DIE:
                    isDie = true;
                    gm.enemyCount--;
                    gm.killCount++; //óġ�� ���� �� ����
                    if (type == Type.TA_LONG || type == Type.BOSS) enemyFire.isFire = false; //���� ����
                    moveAgent.Stop(); //������ ����
                    dieEffect.Play(); //���� ����Ʈ ���
                    GetComponent<CapsuleCollider>().enabled = false;
                    gameObject.tag = "Untagged";
                    gm.killNum++; //�� óġ �� ����
                    //���� ����&&���罺�������� ���� �����Ǵ� �� ��ȭ
                    //�÷��̾� �� ����
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
                    Invoke("DieEnemy", 1.0f); //1���� �� ����
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

    //�÷��̾ ���� ��� ���� �ൿ ó�� �Լ�
    public void OnPlayerDie()
    {
        if(!isDie)
        {
            moveAgent.Stop(); //������ ����
            if (type == Type.TA_LONG || type == Type.BOSS) enemyFire.isFire = false; //���ݸ���
            StopAllCoroutines();
        }
    }

    private void Update()
    {
        if (gm.isGameClear)
        {
            moveAgent.Stop();
            if (type == Type.TA_LONG || type == Type.BOSS) enemyFire.isFire = false; //���ݸ���
            StopAllCoroutines();
        }
    }
}
