using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
	public float currHp = 100;
	public Image bloodScreen;
	public Image hpBar;

	Color initColor = new Vector4(0, 1.0f, 0.0f, 1.0f);
	Color curColor;

    private void Start()
    {
		hpBar.color = initColor;
		curColor = initColor;
    }
    private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "EnemyBullet" || other.tag == "Enemy")
		{
			if(other.tag == "EnemyBullet") Destroy(other.gameObject);
			currHp -= 5.0f;
			Debug.Log("Player HP = " + currHp.ToString());
			if (currHp <= 0.0f)
			{
				PlayerDie();
			}

			StartCoroutine(ShowBloodScreen());
			DisplayHpbar();
		}
	}


	void PlayerDie()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < enemies.Length; i++)
		{
			enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
		}
	}

	IEnumerator ShowBloodScreen()
    {
		bloodScreen.color = new Color(1, 0, 0, Random.Range(0.5f, 1.0f));
		yield return new WaitForSeconds(0.1f);
		bloodScreen.color = Color.clear;
    }
	void DisplayHpbar()
	{
		float ratio = currHp / 100.0f;
		if (ratio > 0.5f)
			curColor.r = (1 - ratio) * 2.0f;
		else
			curColor.g = ratio * 2.0f;
		hpBar.color = curColor;
		hpBar.fillAmount = ratio;
	}
}
