using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Trace : MonoBehaviour
{
    Transform playerPos;
    NavMeshAgent nma;
    public float stopNum = 1.0f;
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerPos = player.transform;
        nma = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float dist = (playerPos.position - transform.position).magnitude;

        if(dist<stopNum)
        {
            nma.isStopped = true;
        }
        else
        {
            nma.isStopped = false;
            nma.SetDestination(playerPos.position);
            Debug.DrawLine(playerPos.position, transform.position, Color.red);
        }
    }
}
