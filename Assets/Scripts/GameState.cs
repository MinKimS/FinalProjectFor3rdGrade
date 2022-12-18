using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    GameManager gm;
    public Text gmOverText;
    public Text gmClearText;
    public GameObject oneDiceUI;
    public GameObject successText; //주사위 성공 텍스트
    public GameObject failText; //주사위 실패 텍스트

    AudioSource audioSource;

    void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if(gmOverText != null)
            gmOverText.text = "처치한 적 : " + gm.killNum + "\n현재 스테이지 : " + gm.curStage;
        if (gmClearText != null)
            gmClearText.text = "처치한 적 : " + gm.killNum + "\n남은 체력 : " + gm.curHp;
    }
    public void reStartGame()
    {
        audioSource.Play();
        oneDiceUI.SetActive(true);
        StartCoroutine(roadCurScene());
    }

    public void goTitle()
    {
        audioSource.Play();
        oneDiceUI.SetActive(true);
        StartCoroutine(roadTitle());
    }

    IEnumerator roadCurScene()
    {
        yield return new WaitForSeconds(2.0f);
        if (gm.diceNum > 3)
        {
            successText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            LoadSceneManager.LoadScene("sampleScene");
        }
        else
        {
            failText.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        successText.SetActive(false);
        failText.SetActive(false);
        oneDiceUI.SetActive(false);
    }
    IEnumerator roadTitle()
    {
        yield return new WaitForSeconds(2.0f);
        if (gm.diceNum > 3)
        {
            successText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            LoadSceneManager.LoadScene("Title");
        }
        else
        {
            failText.SetActive(true);
        }
        yield return new WaitForSeconds(0.5f);
        successText.SetActive(false);
        failText.SetActive(false);
        oneDiceUI.SetActive(false);
    }
}
