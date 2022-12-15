using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float damage = 20.0f;
    public float speed = 1000.0f;
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        
    }

    //�Ѿ� ����
    void BulletStop()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    //�Ѿ� �ٽ� ������
    void BulletGo()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }
}
