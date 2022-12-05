using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDice : MonoBehaviour
{
    public Sprite[] diceImg;
    Image thisImg;
    int idx;
    public int value;
    void Start()
    {
        thisImg = GetComponent<Image>();
        idx = Random.Range(0, 6);
        StartCoroutine(Roll());
    }

    void Update()
    {
        
    }

    public void ChangeImg()
    {
        thisImg.sprite = diceImg[idx];
        value = idx + 1;
        switch(value)
        {
            case 1:
                print("0.5x");
                break;
            case 2:
                print("1.5x");
                break;
            case 3:
                print("1.0x");
                break;
            case 4:
                print("0.75x");
                break;
            case 5:
                print("1.5x");
                break;
            case 6:
                print("2.0x");
                break;
        }
        idx = Random.Range(0, 6);
    }
    IEnumerator Roll()
    {
        while(true)
        {
            ChangeImg();
            yield return new WaitForSeconds(8.0f);
        }
    }
}
