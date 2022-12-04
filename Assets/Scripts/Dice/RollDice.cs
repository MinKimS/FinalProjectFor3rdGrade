using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDice : MonoBehaviour
{
    public Sprite[] diceImg;
    Image thisImg;
    public int value;

    void Start()
    {
        thisImg = GetComponent<Image>();
        value = Random.Range(0, 6);
        StartCoroutine(Roll());
    }

    void Update()
    {

    }

    public void ChangeImg()
    {
        thisImg.sprite = diceImg[value];
        print(value + 1);
        value = Random.Range(0, 6);
    }
    IEnumerator Roll()
    {
        while(true)
        {
            yield return new WaitForSeconds(8.0f);
            ChangeImg();
        }
    }
}
