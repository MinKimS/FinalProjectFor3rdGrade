using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    private Vector3 dist; //�÷��̾�� ī�޶� �Ÿ�
    public Transform player; //�÷��̾�
    public MeshRenderer[] walls; //ī�޶�� �����ϸ� �Ⱥ��� ��

    private void Start()
    {
        player = transform.parent; //�÷��̾� transform ����
    }

    private void Update()
    {
        dist = player.transform.position - transform.position; //�÷��̾�� ī�޶� �Ÿ�
        RaycastHit hit;
        MeshRenderer md;
        int layermask = 1 << 6; //�� ���̾�

        //ī�޶�� �� �浹 �� �� �����ϰ�
        if (Physics.Raycast(transform.position, dist, out hit, 10, layermask))
        {
            md = hit.collider.GetComponent<MeshRenderer>();
            md.enabled = false;
        }
        //�浹 ���� �� �ٽ� ���󺹱�
        else
        {
            for(int i = 0; i < walls.Length; i++)
                walls[i].enabled = true;
        }

        //ī�޶�� �÷��̾� ���� �Ÿ�
        Debug.DrawRay(transform.position, dist, Color.red);
    }
}
