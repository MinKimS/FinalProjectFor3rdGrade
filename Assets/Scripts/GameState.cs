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

    AudioSource audioSource;

    void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if(gmOverText != null)
            gmOverText.text = "óġ�� �� : " + gm.killNum + "\n���� �������� : " + gm.curStage;
        if (gmClearText != null)
            gmClearText.text = "óġ�� �� : " + gm.killNum + "\n���� ü�� : " + gm.curHp;
    }
    public void reStartGame()
    {
        audioSource.Play();
        Invoke("roadCurScene", 0.5f);
    }

    public void goTitle()
    {
        audioSource.Play();
        Invoke("roadTitle", 0.5f);
    }

    void roadCurScene()
    {
        SceneManager.LoadScene("sampleScene");
    }
    void roadTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
