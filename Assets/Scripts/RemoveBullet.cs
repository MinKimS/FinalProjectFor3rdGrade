using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect; //스파크 프리팹

    private void OnCollisionEnter(Collision collision)
    {
        //총알과 충돌 시 이펙트 함수 호출 후 총알 삭제
        if(collision.collider.tag == "Bullet")
        {
            ShowEffect(collision);
            Destroy(collision.gameObject);
        }
    }

    void ShowEffect(Collision coll)
    {
        ContactPoint contact = coll.contacts[0]; //충돌 지점 정보 추출
        Quaternion rot = Quaternion.FromToRotation(Vector3.back, contact.normal); //법선 벡터가 이루는 회전각도 추출
        GameObject spark = Instantiate(sparkEffect, contact.point - (contact.normal * 0.05f), rot); //스파크 효과 생성
        spark.transform.SetParent(this.transform); //충돌한 오브젝트를 부모로 설정
    }
    private void OnTriggerEnter(Collider other)
    {
        //적 총알이 충돌할 시 삭제
        if (other.tag == "EnemyBullet")
        {
            Destroy(other.gameObject);
        }
    }
}
