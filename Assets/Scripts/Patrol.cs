using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    public List<Transform> wayPoints;
    int nextIdx;

    float patrolSpeed = 1.5f;

    NavMeshAgent nma;
    void Start()
    {
        nma = GetComponent<NavMeshAgent>();
        GameObject group = GameObject.Find("WayPointGroup");
        if(group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            wayPoints.RemoveAt(0);
        }

        nma.speed = patrolSpeed;

        MoveWayPoint();
    }

    void Update()
    {
        if(nma.velocity.magnitude >= 0.2f && nma.remainingDistance <= 0.5f)
        {
            nextIdx = Random.Range(0, wayPoints.Count);
            MoveWayPoint();
        }
    }
    void MoveWayPoint()
    {
        if (nma.isPathStale) return;
        nma.destination = wayPoints[nextIdx].position;
        nma.isStopped = false;
    }
}
