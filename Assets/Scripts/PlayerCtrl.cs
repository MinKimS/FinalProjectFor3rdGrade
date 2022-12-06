using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public enum Camera
    {
        TPS,
        FPS
    }
    public float moveSpeed = 2.0f;
    Vector3 dir;
    public float rotSpeed = 50.0f;

    //카메라
    public GameObject tCam;
    public GameObject fCam;
    Camera cam = Camera.TPS;

    //플레이어 state
    float atk;
    float def;
    float maxHp;
    public GameObject[] weapons = new GameObject[6];
    int wpIdx = 0;

    GameManager gm;
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        //무기 저장
        for (int i = 0; i < 6; i++)
        {
            weapons[i] = transform.GetChild(2).GetChild(i).gameObject;
        }
        //기본 무기 들기
        weapons[wpIdx].SetActive(true);
    }

    void Update()
    {
        //플레이어 이동
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        if (dir.magnitude > 1) dir.Normalize();

        float len = moveSpeed * Time.deltaTime;
        transform.Translate(dir * len);
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

        CameraConvert();    //카메라 전환

        //state 설정
        SetState();
    }

    //state 설정 함수
    void SetState()
    {
        //무기 바꾸기
        weapons[wpIdx].SetActive(false);
        wpIdx = gm.weapon;
        weapons[wpIdx].SetActive(true);
    }


    //인칭 전환
    void CameraConvert()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (cam == Camera.TPS)
            {
                FPSCamera();
                cam = Camera.FPS;
            }
            else
            {
                TPSCamera();
                cam = Camera.TPS;
            }
        }
    }

    //카메라 3인칭설정
    void TPSCamera()
    {
        tCam.SetActive(true);
        fCam.SetActive(false);
    }

    //카메라 1인칭설정
    void FPSCamera()
    {
        tCam.SetActive(false);
        fCam.SetActive(true);
    }
}
