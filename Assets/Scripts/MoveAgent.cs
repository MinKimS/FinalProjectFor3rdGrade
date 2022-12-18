using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    public List<Transform> wayPoints; //순찰 시 이동할 포인트
    int nextIdx;

    //정찰속도
    public float patrolSpeed = 1.0f;
    //추적속도
    public float traceSpeed = 1.0f;
    public float anSpeed = 120; //회전속도

    NavMeshAgent nma;
    //순찰여부
    bool patrolling;
    //추적대상
    Vector3 traceTarget;

    //순찰 설정
    public void SetPatrolling(bool patrol)
    {
        patrolling = patrol;
        nma.speed = patrolSpeed; //순찰 속도 설정
        nma.angularSpeed = anSpeed; //회전 속도 설정
        MoveWayPoint();
    }

    //추적으로 설정
    public void SetTraceTarget(Vector3 pos)
    {
        traceTarget = pos;
        nma.speed = traceSpeed; //이동속도 설정
        nma.angularSpeed = anSpeed; //회전속도 설정
        Tracetarget(traceTarget); //추적 타겟 설정
    }
    void Start()
    {
        //순찰, 추적 속도 적마다 다르게 설정
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
        //추적 시작
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
