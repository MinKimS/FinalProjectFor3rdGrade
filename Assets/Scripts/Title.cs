using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private Animator diceAnim; //주사위 애니메이션

    public GameObject failText; //실패텍스트
    public GameObject successText; //성공 텍스트

    private int diceIdx = 0; //주사위인덱스
    private AudioSource audioSource;
    public AudioClip[] clip; //타이틀 오디오클립
    public GameObject exUI; //게임방법창

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        diceAnim = GetComponent<Animator>();
    }

    //버튼 클릭시 주사위 함수
    public void rollDice(int i)
    {
        diceAnim.SetBool("isRoll", true);
        StartCoroutine(Dice(i));
    }

    IEnumerator Dice(int i)
    {
        yield return new WaitForSeconds(2.0f);
        //주사위 값 설정, 애니메이션 수행
        diceIdx = Random.Range(0, 6);
        diceAnim.SetBool("isRoll", false);
        diceAnim.SetInteger("diceNum", diceIdx + 1);
        //주사위 3 이상인 경우 버튼 기능 수행
        if(diceIdx > 2)
        {
            successText.SetActive(true);
            audioSource.PlayOneShot(clip[1]);
            yield return new WaitForSeconds(1.0f);
            if(i == 0)
                LoadSceneManager.LoadScene("SampleScene"); //게임 진행 씬을 이동
            else if(i == 1)
            {
                //게임 종료
                print("exit");
                successText.SetActive(false);
                Application.Quit();
            }
            else
            {
                //설명창 띄우기
                successText.SetActive(false);
                exUI.SetActive(true);
            }
        }
        //아닌경우 실패 텍스트와 효과음 보이기
        else
        {
            yield return new WaitForSeconds(0.5f);
            failText.SetActive(true);
            audioSource.PlayOneShot(clip[2]);
            yield return new WaitForSeconds(1.5f);
            failText.SetActive(false);
        }
    }

    //게임시작 버튼
    public void gameStart()
    {
        audioSource.PlayOneShot(clip[0]); //버튼 클릭 효과음 재생
        rollDice(0);
    }

    //게임 나가기 버튼
    public void gameExit()
    {
        audioSource.PlayOneShot(clip[0]); //버튼 클릭 효과음 재생
        rollDice(1);
    }

    //게임 설명 버튼
    public void GameEx()
    {
        audioSource.PlayOneShot(clip[0]); //버튼 클릭 효과음 재생
        rollDice(2);
    }

    //설명 창 비활성화
    public void Close()
    {
        exUI.SetActive(false);
    }
}
