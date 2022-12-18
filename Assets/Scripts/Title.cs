using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private Animator diceAnim;

    public GameObject failText;
    public GameObject successText;

    private int diceIdx = 0; //주사위인덱스
    private AudioSource audioSource;
    public AudioClip[] clip;
    public GameObject exUI; //게임방법창

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        diceAnim = GetComponent<Animator>();
    }

    public void rollDice(int i)
    {
        diceAnim.SetBool("isRoll", true);
        StartCoroutine(Dice(i));
    }

    IEnumerator Dice(int i)
    {
        yield return new WaitForSeconds(2.0f);
        diceIdx = Random.Range(0, 6);
        diceAnim.SetBool("isRoll", false);
        diceAnim.SetInteger("diceNum", diceIdx + 1);
        if(diceIdx > 2)
        {
            successText.SetActive(true);
            audioSource.PlayOneShot(clip[1]);
            yield return new WaitForSeconds(1.0f);
            if(i == 0)
                LoadSceneManager.LoadScene("SampleScene");
            else if(i == 1)
            {
                print("exit");
                Application.Quit();
            }
            else
            {
                successText.SetActive(false);
                exUI.SetActive(true);
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            failText.SetActive(true);
            audioSource.PlayOneShot(clip[2]);
            yield return new WaitForSeconds(1.5f);
            failText.SetActive(false);
        }
    }

    public void gameStart()
    {
        audioSource.PlayOneShot(clip[0]);
        rollDice(0);
    }

    public void gameExit()
    {
        audioSource.PlayOneShot(clip[0]);
        rollDice(1);
    }

    public void GameEx()
    {
        audioSource.PlayOneShot(clip[0]);
        rollDice(2);
    }

    public void Close()
    {
        exUI.SetActive(false);
    }
}
