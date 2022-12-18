using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Bullet")
        {
            ShowEffect(collision);
            Destroy(collision.gameObject);
        }
    }

    void ShowEffect(Collision coll)
    {
        ContactPoint contact = coll.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.back, contact.normal);
        GameObject spark = Instantiate(sparkEffect, contact.point - (contact.normal * 0.05f), rot);
        spark.transform.SetParent(this.transform);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyBullet")
        {
            Destroy(other.gameObject);
        }
    }
}
