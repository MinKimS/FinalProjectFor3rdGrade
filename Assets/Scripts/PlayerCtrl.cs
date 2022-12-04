using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    Vector3 dir;
    public float rotSpeed = 50.0f;
    public GameObject tCam;
    public GameObject fCam;
    void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        if (dir.magnitude > 1) dir.Normalize();

        float len = moveSpeed * Time.deltaTime;
        transform.Translate(dir * len);
        //transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

        CameraConvert();
    }

    void CameraConvert()
    {
        if (Input.GetKeyDown("1")) TPSCamera();
        if (Input.GetKeyDown("2")) FPSCamera();
    }

    void TPSCamera()
    {
        tCam.SetActive(true);
        fCam.SetActive(false);
    }
    void FPSCamera()
    {
        tCam.SetActive(false);
        fCam.SetActive(true);
    }
}
