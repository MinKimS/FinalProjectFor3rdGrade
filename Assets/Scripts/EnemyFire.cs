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

    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTr = GetComponent<Transform>();
        _audio = GetComponent<AudioSource>();
        fireEffect = firePos.GetComponentInChildren<ParticleSystem>();
        ed = GetComponent<EDamageData>();
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
            //Destroy(_bullet, 3.5f);
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
