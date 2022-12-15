using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    public List<Transform> wayPoints;
    int nextIdx;

    //�����ӵ�
    public float patrolSpeed = 1.0f;
    //�����ӵ�
    public float traceSpeed = 1.0f;

    NavMeshAgent nma;
    //��������
    bool patrolling;
    //�������
    Vector3 traceTarget;

    public void SetPatrolling(bool patrol)
    {
        patrolling = patrol;
        nma.speed = patrolSpeed;
        nma.angularSpeed = 120;
        MoveWayPoint();
    }
    public void SetTraceTarget(Vector3 pos)
    {
        traceTarget = pos;
        nma.speed = traceSpeed;
        nma.angularSpeed = 360;
        Tracetarget(traceTarget);
    }
    void Start()
    {
        if(gameObject.tag == "Boss")
            patrolSpeed = 1.0f;
        else
            patrolSpeed += Random.Range(0.5f, 1.0f);
        traceSpeed += Random.Range(0.5f, 1.0f);
        nma = GetComponent<NavMeshAgent>();
        nma.speed = patrolSpeed;

        GameObject group = GameObject.Find("WayPointGroup");
        if(group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            wayPoints.RemoveAt(0);
            nextIdx = Random.Range(0, wayPoints.Count); //���� �������ε��� ��������
        }
        SetPatrolling(true);
    }
    // ���� �������� �̵� �Լ�
    void MoveWayPoint()
    {
        if (nma.isPathStale) return; //�ִܰŸ� ��� ����� ���������� ���� �ڵ� �������
        //���� ������ ����
        nma.destination = wayPoints[nextIdx].position;
        //�̵� ����
        nma.isStopped = false;
    }

    // ���� �̵� �Լ�
    void Tracetarget(Vector3 pos)
    {
        // ��� ����� ������ �������� �̵�
        if (nma.isPathStale) return;
        nma.destination = pos;
        nma.isStopped = false;
    }

    //���� �� ���� ���� �Լ�
    public void Stop()
    {
        nma.isStopped = true;
        nma.velocity = Vector3.zero; //�ٷ� ������ ���� �ӵ��� 0���� ����
        patrolling = false;
    }

    void Update()
    {
        if (!patrolling) return; //���� ���� ��쿡�� �Ʒ� �ڵ� ����

        //�̵� ���̰� ������ ���� ���� ���
        if (nma.velocity.magnitude >= 0.2f && nma.remainingDistance <= 0.5f)
        {
            nextIdx = Random.Range(0, wayPoints.Count); //���� �������ε��� ��������
            MoveWayPoint(); //���� �������� �̵�
        }
    }
}
