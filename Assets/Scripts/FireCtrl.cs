using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    public GameObject bullet; //총알 프리팹
    public ParticleSystem cartridge; //탄피 파티클
    public Transform firePos; //총알 발사 지점
    public AudioClip fireSound; //발사 소리
    public GameObject warningText; //총알 부족 경고 텍스트

    ParticleSystem fireEffect; //화염 파티클
    AudioSource _audio; //오디오소스 컴포넌트

    // 총쏘는 딜레이
    private bool isFire = false;
    //장탄수 변수
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
            //장탄수 0이면 총발사 불가
            if(!isFire && bulletNum > 0)
            {
                Fire(); //총알발사
                StartCoroutine(FireCheck());//총 발사 딜레이
            }
        }
        if(gm.isEnter)
		{
            ChargeBullet();

        }
        //만약 r을 누르면 장탄수 원상복구
        //스테이지 변경되었을때 무기설정
        if (Input.GetKey(KeyCode.R)|| gm.isShop)
        {
            ChargeBullet();
        }
        //장탄수 부족 경고 출력
        if(bulletNum < 1)
		{
            warningText.SetActive(true);
        }

        if(gm.isGameClear || gm.isGameOver || gm.isShop)
        {
            //장탄수 경고 텍스트 숨기기
            warningText.SetActive(false);
        }
    }

    //총알 수 다시 채우기
    void ChargeBullet()
    {
        switch (gm.weapon)
        {
            //권총
            case 0:
                bulletNum = 12;
                break;
            //라이플
            case 1:
                bulletNum = 13;
                break;
            //리볼버
            case 2:
                bulletNum = 6;
                break;
            //샷건
            case 3:
                bulletNum = 2;
                break;
            //SMG
            case 4:
                bulletNum = 30;
                break;
            //스나이퍼
            case 5:
                bulletNum = 5;
                break;
        }
        //장탄수 경고 텍스트 숨기기
        warningText.SetActive(false);
    }

    //총알 발사
    void Fire()
    {
        //총알생성
        Instantiate(bullet, firePos.position, firePos.rotation);
        //총알 발사 이펙트와 효과음 재생
        cartridge.Play();
        fireEffect.Play();
        _audio.PlayOneShot(fireSound, 1.0f);
        isFire = true;
        //장탄수1감소
        if (bulletNum > 0)
            bulletNum--;
    }

    IEnumerator FireCheck()
    {
        //총마다 딜레이 설정
        switch(gm.weapon)
        {
            //권총, 리볼버
            case 0:
            case 2:
                yield return new WaitForSeconds(0.5f);
                break;
            //샷건, 스나이퍼
            case 3:
            case 5:
                yield return new WaitForSeconds(2.0f);
                break;
            //라이플, smg는 공격딜레이없음
        }
        isFire = false;
    }
}
