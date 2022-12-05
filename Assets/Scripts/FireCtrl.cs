using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    public GameObject bullet;
    public ParticleSystem cartridge;
    public Transform firePos;
    public AudioClip fireSound;

    ParticleSystem fireEffect;
    AudioSource _audio;
    void Start()
    {
        fireEffect = firePos.GetComponentInChildren<ParticleSystem>();
        _audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }
    void Fire()
    {

        Instantiate(bullet, firePos.position, firePos.rotation);
        cartridge.Play();
        fireEffect.Play();
        _audio.PlayOneShot(fireSound, 1.0f);
    }
}
