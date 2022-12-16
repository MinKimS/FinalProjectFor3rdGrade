using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBulletCtrl : MonoBehaviour
{
    public float damage = 20.0f;
    public float speed = 1000.0f;
    Transform playerPos;
    void Start()
    {
        //playerPos = GameObject.Find("Player").GetComponent<Transform>();
        //print(playerPos.position);
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    void Update()
    {
        
    }
}