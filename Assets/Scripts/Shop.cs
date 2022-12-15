using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    GameManager gm;
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        
    }

    //다음 스테이지로 이동
    void NextStageBtn()
    {
        //isspawn true로 변경
        //shopui끄기
    }
}
