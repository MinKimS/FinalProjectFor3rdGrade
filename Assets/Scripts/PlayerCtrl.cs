using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
	public float moveSpeed = 2.0f; //�̵� �ӵ�
    Vector3 dir;
    public float rotSpeed = 50.0f;//ȸ�� �ӵ�

	public GameObject[] weapons = new GameObject[6]; //���� ������
    int wpIdx = 0;
	public GameObject stateUI; //�ɷ�ġ UI

    GameManager gm;

    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		//���콺 Ŀ�� �Ⱥ��̰� ����
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		//���� ����
		for (int i = 0; i < 6; i++)
        {
            weapons[i] = transform.GetChild(1).GetChild(i).gameObject;
		}
	}
	
	//�� �浹�� �÷��̾� ��������
    private void FixedUpdate()
    {
		//�÷��̾� �̵�
		if (!gm.isEnter && !gm.isGameOver && !gm.isShop && !gm.isGameClear)
		{
			dir.x = Input.GetAxis("Horizontal");
			dir.z = Input.GetAxis("Vertical");
			float r = Input.GetAxis("Mouse X");

			if (dir.magnitude > 1) dir.Normalize();

			float len = moveSpeed * Time.deltaTime;
			transform.Translate(dir * len); //�̵�
			transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r); //ȸ��
		}
	}
    void Update()
	{
		//����â Ű�� ����
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

		//���� ����
		SetWeapon();

		//���콺 Ŀ�� ���̴� ���� �Ⱥ��̴� ��� ����
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

    //���⼳��
    void SetWeapon()
    {
        //���� �ٲٱ�
        weapons[wpIdx].SetActive(false);
        wpIdx = gm.weapon;
        weapons[wpIdx].SetActive(true);
    }
}
