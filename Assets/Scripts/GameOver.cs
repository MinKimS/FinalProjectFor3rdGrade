using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    GameManager gm;
    public Text killNumText;
    public Text curStage;
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        killNumText.text = "óġ�� �� : " + gm.killNum;
        curStage.text = "���� �������� : " + gm.curStage;
    }
}
