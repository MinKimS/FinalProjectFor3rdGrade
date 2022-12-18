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
    public GameObject oneDiceUI; //�ֻ��� �Ѱ� UI
    public GameObject successText; //�ֻ��� ���� �ؽ�Ʈ
    public GameObject failText; //�ֻ��� ���� �ؽ�Ʈ

    AudioSource audioSource;

    void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        //���ӿ����� ����Ŭ��� ���� �ؽ�Ʈ ����
        if(gmOverText != null)
            gmOverText.text = "óġ�� �� : " + gm.killNum + "\n���� �������� : " + gm.curStage;
        if (gmClearText != null)
            gmClearText.text = "óġ�� �� : " + gm.killNum + "\n���� ü�� : " + gm.curHp;
    }
    //����� ��ư
    public void reStartGame()
    {
        audioSource.Play();
        oneDiceUI.SetActive(true);
        StartCoroutine(roadCurScene());
    }
    //Ÿ��Ʋ�ΰ��� ��ư
    public void goTitle()
    {
        audioSource.Play();
        oneDiceUI.SetActive(true);
        StartCoroutine(roadTitle());
    }
    //���� �� �ٽ� �θ���
    IEnumerator roadCurScene()
    {
        yield return new WaitForSeconds(2.0f);
        //�ֻ����� 4�̻��� ��쿡�� �̵�
        //�ƴѰ�� ���� �ؽ�Ʈ ����
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
    //Ÿ��Ʋ ������ �̵�
    IEnumerator roadTitle()
    {
        yield return new WaitForSeconds(2.0f);
        //�ֻ����� 4�̻��� ��쿡�� �̵�
        //�ƴѰ�� ���� �ؽ�Ʈ ����
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
