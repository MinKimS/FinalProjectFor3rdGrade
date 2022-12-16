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

	//ī�޶�
	public GameObject tCam;
	public GameObject fCam;
	Camera cam = Camera.TPS;

	public GameObject[] weapons = new GameObject[6];
    int wpIdx = 0;

    GameManager gm;
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		//���� ����
		for (int i = 0; i < 6; i++)
        {
            weapons[i] = transform.GetChild(2).GetChild(i).gameObject;
        }
        //�⺻ ���� ���
        weapons[wpIdx].SetActive(true);
    }

    void Update()
    {
        //���� ����
		if(cam == Camera.TPS)
			SetWeapon();

        //�÷��̾� �̵�
        if (!gm.isEnter && !gm.isGameOver && !gm.isShop)
        {
            dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");
            float r = Input.GetAxis("Mouse X");

            if (dir.magnitude > 1) dir.Normalize();

            float len = moveSpeed * Time.deltaTime;
            transform.Translate(dir * len);
            transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

			CameraConvert();    //ī�޶� ��ȯ
		}

        if(gm.isShop || gm.isGameOver || Input.GetKey(KeyCode.LeftAlt))
		{
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
		{
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //���⼳��
    void SetWeapon()
    {
        //���� �ٲٱ�
        weapons[wpIdx].SetActive(false);
        wpIdx = gm.weapon;
        weapons[wpIdx].SetActive(true);
    }


	//��Ī ��ȯ
	void CameraConvert()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
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

	//ī�޶� 3��Ī����
	void TPSCamera()
	{
		tCam.SetActive(true);
		fCam.SetActive(false);
		weapons[wpIdx].SetActive(true);
	}

	//ī�޶� 1��Ī����
	void FPSCamera()
	{
		tCam.SetActive(false);
		fCam.SetActive(true);
		weapons[wpIdx].SetActive(false);
	}
}
