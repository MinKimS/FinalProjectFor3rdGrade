using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private Animator diceAnim; //�ֻ��� �ִϸ��̼�

    public GameObject failText; //�����ؽ�Ʈ
    public GameObject successText; //���� �ؽ�Ʈ

    private int diceIdx = 0; //�ֻ����ε���
    private AudioSource audioSource;
    public AudioClip[] clip; //Ÿ��Ʋ �����Ŭ��
    public GameObject exUI; //���ӹ��â

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        diceAnim = GetComponent<Animator>();
    }

    //��ư Ŭ���� �ֻ��� �Լ�
    public void rollDice(int i)
    {
        diceAnim.SetBool("isRoll", true);
        StartCoroutine(Dice(i));
    }

    IEnumerator Dice(int i)
    {
        yield return new WaitForSeconds(2.0f);
        //�ֻ��� �� ����, �ִϸ��̼� ����
        diceIdx = Random.Range(0, 6);
        diceAnim.SetBool("isRoll", false);
        diceAnim.SetInteger("diceNum", diceIdx + 1);
        //�ֻ��� 3 �̻��� ��� ��ư ��� ����
        if(diceIdx > 2)
        {
            successText.SetActive(true);
            audioSource.PlayOneShot(clip[1]);
            yield return new WaitForSeconds(1.0f);
            if(i == 0)
                LoadSceneManager.LoadScene("SampleScene"); //���� ���� ���� �̵�
            else if(i == 1)
            {
                //���� ����
                print("exit");
                successText.SetActive(false);
                Application.Quit();
            }
            else
            {
                //����â ����
                successText.SetActive(false);
                exUI.SetActive(true);
            }
        }
        //�ƴѰ�� ���� �ؽ�Ʈ�� ȿ���� ���̱�
        else
        {
            yield return new WaitForSeconds(0.5f);
            failText.SetActive(true);
            audioSource.PlayOneShot(clip[2]);
            yield return new WaitForSeconds(1.5f);
            failText.SetActive(false);
        }
    }

    //���ӽ��� ��ư
    public void gameStart()
    {
        audioSource.PlayOneShot(clip[0]); //��ư Ŭ�� ȿ���� ���
        rollDice(0);
    }

    //���� ������ ��ư
    public void gameExit()
    {
        audioSource.PlayOneShot(clip[0]); //��ư Ŭ�� ȿ���� ���
        rollDice(1);
    }

    //���� ���� ��ư
    public void GameEx()
    {
        audioSource.PlayOneShot(clip[0]); //��ư Ŭ�� ȿ���� ���
        rollDice(2);
    }

    //���� â ��Ȱ��ȭ
    public void Close()
    {
        exUI.SetActive(false);
    }
}
