using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
	public float moveSpeed = 2.0f; //이동 속도
    Vector3 dir;
    public float rotSpeed = 50.0f;//회전 속도

	public GameObject[] weapons = new GameObject[6]; //무기 프리팹
    int wpIdx = 0;
	public GameObject stateUI; //능력치 UI

    GameManager gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		//마우스 커서 안보이게 설정
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		//무기 저장
		for (int i = 0; i < 6; i++)
        {
            weapons[i] = transform.GetChild(1).GetChild(i).gameObject;
		}
	}
	
	//벽 충돌시 플레이어 떨림방지
    private void FixedUpdate()
    {
		//플레이어 이동
		if (!gm.isEnter && !gm.isGameOver && !gm.isShop && !gm.isGameClear)
		{
			dir.x = Input.GetAxis("Horizontal");
			dir.z = Input.GetAxis("Vertical");
			float r = Input.GetAxis("Mouse X");

			if (dir.magnitude > 1) dir.Normalize();

			float len = moveSpeed * Time.deltaTime;
			transform.Translate(dir * len); //이동
			transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r); //회전
		}
	}
    void Update()
	{
		//설명창 키고 끄기
		if(Input.GetKeyDown(KeyCode.Q) && !gm.isShop && !gm.isGameOver && !gm.isGameClear)
        {
			if(gm.isShowState)
            {
				gm.isShowState = false;
				stateUI.SetActive(false);
			}
			else
            {
				gm.isShowState = true;
				stateUI.SetActive(true);
			}
		}

		if(gm.isShop || gm.isGameClear || gm.isGameOver)
			stateUI.SetActive(false);

		//무기 설정
		SetWeapon();

		//마우스 커서 보이는 경우와 안보이는 경우 설정
		if (gm.isShop || gm.isGameOver || Input.GetKey(KeyCode.LeftAlt) || gm.isGameClear)
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

    //무기설정
    void SetWeapon()
    {
        //무기 바꾸기
        weapons[wpIdx].SetActive(false);
        wpIdx = gm.weapon;
        weapons[wpIdx].SetActive(true);
    }
}
