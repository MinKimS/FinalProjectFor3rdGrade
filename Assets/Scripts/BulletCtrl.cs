using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float speed = 1000.0f; //�Ѿ� �̵� �ӵ�
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed); //�Ѿ� �̵�
        Destroy(gameObject, 3f);//3���� �ƹ��� �浹�� ������ ����
    }
}
