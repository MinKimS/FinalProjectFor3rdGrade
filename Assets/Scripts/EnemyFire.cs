using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    public bool isFire = false; //총 발사 가능 여부
    public AudioClip fireSfx;
    public GameObject Bullet; //총알 오브젝트
    public Transform firePos; //총알 발사 위치
    AudioSource _audio;
    Transform playerTr; //플레이어위치
    Transform enemyTr; //자신(적)의 위치
    ParticleSystem fireEffect;
    float nextFire = 0.0f; //다음 발사할 시간 계산용 변수
    public float fireRate = 0.5f; //총알 발사 간격
    public float damping = 10.0f; //주인공을 향해 회전할 속도 계수
    EDamageData ed;
    int bulletNum; //총알 개수
    GameManager gm;

    private void Awake()
    {
        ed = GetComponent<EDamageData>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTr = GetComponent<Transform>();
        _audio = GetComponent<AudioSource>();
        fireEffect = firePos.GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        //공격 활성화 시 총알 발사
        if (isFire)
        {
            //다음 공격시간이 되면 공격
            if (Time.time >= nextFire)
            {
                //적 캐릭터에 따라 총알 발사 개수 다름
                SetBullet();
                Fire(); //총알 발사
                nextFire = Time.time + fireRate + Random.Range(0.0f, 1.0f);//다음 공격시간 계산
            }

            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);//플레이어가 있는 위치까지의 회전 각도 계산
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);//회전
        }
    }

    private void OnEnable()
    {
        if(ed.etype == EDamageData.EType.BOSS)
        {
            //일정시간마다 변하기
            StartCoroutine(RangeAttack()); //일정시간마다 범위 공격 발사
        }
    }

    IEnumerator RangeAttack()
    {
        //게임오버가 되기전까지 6초마다 공격
        while(!gm.isGameOver)
        {
            yield return new WaitForSeconds(3.0f);
            int rot = 20;
            int roY = 0;
            //8방향으로 공격
            for (int i = 0; i < 19; i++)
            {
                roY += rot;
                Instantiate(Bullet, firePos.position, Quaternion.Euler(0, roY, 0));
            }
            roY = 0;
        }
    }

    void Fire()
    {
        _audio.PlayOneShot(fireSfx, 0.5f); //총알 발사 사운드 재생
        fireEffect.Play(); //총알 이펙트 재생

        Vector3 RandomPosition; //랜덤위치 설정
        GameObject _bullet;
        for (int i = 0; i < bulletNum; i++)
        {
            RandomPosition = new Vector3(Random.Range(-0.4f, 0.4f), 0f, 0f);
            //총알 생성후3초뒤 삭제
            _bullet = Instantiate(Bullet, firePos.position+ RandomPosition, firePos.rotation);
        }
    }

    void SetBullet()
    {
        switch (ed.etype)
        {
            case EDamageData.EType.BISHOP:
            case EDamageData.EType.ROOK:
            case EDamageData.EType.CARD:
                bulletNum = 1;
                break;
            case EDamageData.EType.QCARD:
                bulletNum = 2;
                break;
            case EDamageData.EType.JCARD:
                bulletNum = 3;
                break;
            case EDamageData.EType.BOSS:
                bulletNum = 4;
                break;
        }
    }
}
