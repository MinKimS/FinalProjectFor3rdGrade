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
        killNumText.text = "처치한 적 : " + gm.killNum;
        curStage.text = "현재 스테이지 : " + gm.curStage;
    }
}
