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
    public GameObject oneDiceUI; //주사위 한개 UI
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
        //게임오버와 게임클리어에 따라 텍스트 설정
        if(gmOverText != null)
            gmOverText.text = "처치한 적 : " + gm.killNum + "\n현재 스테이지 : " + gm.curStage;
        if (gmClearText != null)
            gmClearText.text = "처치한 적 : " + gm.killNum + "\n남은 체력 : " + gm.curHp;
    }
    //재시작 버튼
    public void reStartGame()
    {
        audioSource.Play();
        oneDiceUI.SetActive(true);
        StartCoroutine(roadCurScene());
    }
    //타이틀로가기 버튼
    public void goTitle()
    {
        audioSource.Play();
        oneDiceUI.SetActive(true);
        StartCoroutine(roadTitle());
    }
    //현재 씬 다시 부르기
    IEnumerator roadCurScene()
    {
        yield return new WaitForSeconds(2.0f);
        //주사위가 4이상인 경우에만 이동
        //아닌경우 실패 텍스트 띄우기
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
    //타이틀 씬으로 이동
    IEnumerator roadTitle()
    {
        yield return new WaitForSeconds(2.0f);
        //주사위가 4이상인 경우에만 이동
        //아닌경우 실패 텍스트 띄우기
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
