using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    private Vector3 dist; //플레이어와 카메라 거리
    public Transform player; //플레이어
    public MeshRenderer[] walls; //카메라와 접촉하면 안보일 벽

    private void Start()
    {
        player = transform.parent; //플레이어 transform 저장
    }

    private void Update()
    {
        dist = player.transform.position - transform.position; //플레이어와 카메라 거리
        RaycastHit hit;
        MeshRenderer md;
        int layermask = 1 << 6; //벽 레이어

        //카메라와 벽 충돌 시 벽 투명하게
        if (Physics.Raycast(transform.position, dist, out hit, 10, layermask))
        {
            md = hit.collider.GetComponent<MeshRenderer>();
            md.enabled = false;
        }
        //충돌 안할 시 다시 원상복구
        else
        {
            for(int i = 0; i < walls.Length; i++)
                walls[i].enabled = true;
        }

        //카메라와 플레이어 사이 거리
        Debug.DrawRay(transform.position, dist, Color.red);
    }
}
