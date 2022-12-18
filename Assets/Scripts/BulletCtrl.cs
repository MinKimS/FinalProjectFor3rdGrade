using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float speed = 1000.0f; //총알 이동 속도
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed); //총알 이동
        Destroy(gameObject, 3f);//3초후 아무런 충돌이 없으면 삭제
    }
}
