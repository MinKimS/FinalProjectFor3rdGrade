using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    public GameObject bullet; //�Ѿ� ������
    public ParticleSystem cartridge; //ź�� ��ƼŬ
    public Transform firePos; //�Ѿ� �߻� ����
    public AudioClip fireSound; //�߻� �Ҹ�
    public GameObject warningText; //�Ѿ� ���� ��� �ؽ�Ʈ

    ParticleSystem fireEffect; //ȭ�� ��ƼŬ
    AudioSource _audio; //������ҽ� ������Ʈ

    // �ѽ�� ������
    private bool isFire = false;
    //��ź�� ����
    public int bulletNum = 12;
    GameManager gm;
    void Start()
    {
        fireEffect = firePos.GetComponentInChildren<ParticleSystem>();
        _audio = GetComponent<AudioSource>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !gm.isEnter && !gm.isGameOver && !gm.isGameClear && !gm.isShop)
        {
            //��ź�� 0�̸� �ѹ߻� �Ұ�
            if(!isFire && bulletNum > 0)
            {
                Fire(); //�Ѿ˹߻�
                StartCoroutine(FireCheck());//�� �߻� ������
            }
        }
        if(gm.isEnter)
		{
            ChargeBullet();

        }
        //���� r�� ������ ��ź�� ���󺹱�
        //�������� ����Ǿ����� ���⼳��
        if (Input.GetKey(KeyCode.R)|| gm.isShop)
        {
            ChargeBullet();
        }
        //��ź�� ���� ��� ���
        if(bulletNum < 1)
		{
            warningText.SetActive(true);
        }

        if(gm.isGameClear || gm.isGameOver || gm.isShop)
        {
            //��ź�� ��� �ؽ�Ʈ �����
            warningText.SetActive(false);
        }
    }

    //�Ѿ� �� �ٽ� ä���
    void ChargeBullet()
    {
        switch (gm.weapon)
        {
            //����
            case 0:
                bulletNum = 12;
                break;
            //������
            case 1:
                bulletNum = 13;
                break;
            //������
            case 2:
                bulletNum = 6;
                break;
            //����
            case 3:
                bulletNum = 2;
                break;
            //SMG
            case 4:
                bulletNum = 30;
                break;
            //��������
            case 5:
                bulletNum = 5;
                break;
        }
        //��ź�� ��� �ؽ�Ʈ �����
        warningText.SetActive(false);
    }

    //�Ѿ� �߻�
    void Fire()
    {
        //�Ѿ˻���
        Instantiate(bullet, firePos.position, firePos.rotation);
        //�Ѿ� �߻� ����Ʈ�� ȿ���� ���
        cartridge.Play();
        fireEffect.Play();
        _audio.PlayOneShot(fireSound, 1.0f);
        isFire = true;
        //��ź��1����
        if (bulletNum > 0)
            bulletNum--;
    }

    IEnumerator FireCheck()
    {
        //�Ѹ��� ������ ����
        switch(gm.weapon)
        {
            //����, ������
            case 0:
            case 2:
                yield return new WaitForSeconds(0.5f);
                break;
            //����, ��������
            case 3:
            case 5:
                yield return new WaitForSeconds(2.0f);
                break;
            //������, smg�� ���ݵ����̾���
        }
        isFire = false;
    }
}
