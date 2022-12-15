using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    public List<Transform> wayPoints;
    int nextIdx;

    //정찰속도
    public float patrolSpeed = 1.0f;
    //추적속도
    public float traceSpeed = 1.0f;

    NavMeshAgent nma;
    //순찰여부
    bool patrolling;
    //추적대상
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
            nextIdx = Random.Range(0, wayPoints.Count); //다음 목적지인덱스 랜덤설정
        }
        SetPatrolling(true);
    }
    // 다음 목적지로 이동 함수
    void MoveWayPoint()
    {
        if (nma.isPathStale) return; //최단거리 경로 계산이 끝날때까지 다음 코드 수행안함
        //다음 목적지 설정
        nma.destination = wayPoints[nextIdx].position;
        //이동 시작
        nma.isStopped = false;
    }

    // 추적 이동 함수
    void Tracetarget(Vector3 pos)
    {
        // 경로 계산이 끝나면 목적지로 이동
        if (nma.isPathStale) return;
        nma.destination = pos;
        nma.isStopped = false;
    }

    //순찰 및 추적 정지 함수
    public void Stop()
    {
        nma.isStopped = true;
        nma.velocity = Vector3.zero; //바로 정지를 위해 속도를 0으로 설정
        patrolling = false;
    }

    void Update()
    {
        if (!patrolling) return; //순찰 중인 경우에만 아래 코드 실행

        //이동 중이고 목적지 도착 여부 계산
        if (nma.velocity.magnitude >= 0.2f && nma.remainingDistance <= 0.5f)
        {
            nextIdx = Random.Range(0, wayPoints.Count); //다음 목적지인덱스 랜덤설정
            MoveWayPoint(); //다음 목적지로 이동
        }
    }
}
