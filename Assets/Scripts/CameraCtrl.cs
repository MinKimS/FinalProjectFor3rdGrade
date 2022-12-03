using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public GameObject player;
    //Vector3 delta = new Vector3(0.0f, 1.0f, 2.0f);
    public Vector3 delta = new Vector3(-0.2f, 1.0f, -1.4f);
    void LateUpdate()
    {
        RaycastHit hit;
        if(Physics.Raycast(player.transform.position, delta, out hit, delta.magnitude, LayerMask.GetMask("Wall")))
        {
            //float dist = (hit.point - player.transform.position).magnitude * 0.3f;
            //transform.position = player.transform.position + delta.normalized * dist;
            float dist = (hit.point - player.transform.position).magnitude * 0.3f;
            transform.position = player.transform.position + delta.normalized * dist;
        }
        else
        {
            //transform.position = delta;
            //transform.LookAt(player.transform);
        }

        Debug.DrawRay(player.transform.position, transform.position-player.transform.position, Color.red);
    }
}
