using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect; //����ũ ������

    private void OnCollisionEnter(Collision collision)
    {
        //�Ѿ˰� �浹 �� ����Ʈ �Լ� ȣ�� �� �Ѿ� ����
        if(collision.collider.tag == "Bullet")
        {
            ShowEffect(collision);
            Destroy(collision.gameObject);
        }
    }

    void ShowEffect(Collision coll)
    {
        ContactPoint contact = coll.contacts[0]; //�浹 ���� ���� ����
        Quaternion rot = Quaternion.FromToRotation(Vector3.back, contact.normal); //���� ���Ͱ� �̷�� ȸ������ ����
        GameObject spark = Instantiate(sparkEffect, contact.point - (contact.normal * 0.05f), rot); //����ũ ȿ�� ����
        spark.transform.SetParent(this.transform); //�浹�� ������Ʈ�� �θ�� ����
    }
    private void OnTriggerEnter(Collider other)
    {
        //�� �Ѿ��� �浹�� �� ����
        if (other.tag == "EnemyBullet")
        {
            Destroy(other.gameObject);
        }
    }
}
