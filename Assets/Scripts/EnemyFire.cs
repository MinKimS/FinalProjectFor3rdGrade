using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    public bool isFire = false; //�� �߻� ���� ����
    public AudioClip fireSfx;
    public GameObject Bullet; //�Ѿ� ������Ʈ
    public Transform firePos; //�Ѿ� �߻� ��ġ
    AudioSource _audio;
    Transform playerTr; //�÷��̾���ġ
    Transform enemyTr; //�ڽ�(��)�� ��ġ
    ParticleSystem fireEffect;
    float nextFire = 0.0f; //���� �߻��� �ð� ���� ����
    public float fireRate = 0.5f; //�Ѿ� �߻� ����
    public float damping = 10.0f; //���ΰ��� ���� ȸ���� �ӵ� ���
    EDamageData ed;
    int bulletNum; //�Ѿ� ����
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
        //���� Ȱ��ȭ �� �Ѿ� �߻�
        if (isFire)
        {
            //���� ���ݽð��� �Ǹ� ����
            if (Time.time >= nextFire)
            {
                //�� ĳ���Ϳ� ���� �Ѿ� �߻� ���� �ٸ�
                SetBullet();
                Fire(); //�Ѿ� �߻�
                nextFire = Time.time + fireRate + Random.Range(0.0f, 1.0f);//���� ���ݽð� ���
            }

            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);//�÷��̾ �ִ� ��ġ������ ȸ�� ���� ���
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);//ȸ��
        }
    }

    private void OnEnable()
    {
        if(ed.etype == EDamageData.EType.BOSS)
        {
            //�����ð����� ���ϱ�
            StartCoroutine(RangeAttack()); //�����ð����� ���� ���� �߻�
        }
    }

    IEnumerator RangeAttack()
    {
        //���ӿ����� �Ǳ������� 6�ʸ��� ����
        while(!gm.isGameOver)
        {
            yield return new WaitForSeconds(3.0f);
            int rot = 20;
            int roY = 0;
            //8�������� ����
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
        _audio.PlayOneShot(fireSfx, 0.5f); //�Ѿ� �߻� ���� ���
        fireEffect.Play(); //�Ѿ� ����Ʈ ���

        Vector3 RandomPosition; //������ġ ����
        GameObject _bullet;
        for (int i = 0; i < bulletNum; i++)
        {
            RandomPosition = new Vector3(Random.Range(-0.4f, 0.4f), 0f, 0f);
            //�Ѿ� ������3�ʵ� ����
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
