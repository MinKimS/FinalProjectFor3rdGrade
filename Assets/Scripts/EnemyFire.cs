using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    public bool isFire = false;
    public AudioClip fireSfx;
    public GameObject Bullet;
    public Transform firePos;
    AudioSource _audio;
    Transform playerTr;
    Transform enemyTr;
    ParticleSystem fireEffect;
    float nextFire = 0.0f;
    float fireRate = 0.5f;
    float damping = 10.0f;

    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTr = GetComponent<Transform>();
        _audio = GetComponent<AudioSource>();
        fireEffect = firePos.GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (isFire)
        {
            if (Time.time >= nextFire)
            {
                Fire();
                //다음 공격
                nextFire = Time.time + fireRate + Random.Range(0.0f, 1.0f);
            }

            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
    }

    void Fire()
    {
        _audio.PlayOneShot(fireSfx, 0.5f);
        fireEffect.Play();

        GameObject _bullet = Instantiate(Bullet, firePos.position, firePos.rotation);
        Destroy(_bullet, 3.0f);
    }
}
