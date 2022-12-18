using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
	public Image bloodScreen;
	public Image hpBar;

	Color initColor = new Vector4(0, 1.0f, 0.0f, 1.0f);
	Color curColor;
	GameManager gm;

    private void Start()
	{
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();

		hpBar.color = initColor;
		curColor = initColor;
    }
  //  private void Update()
  //  {
		////MaxHp 설정
		//if(gm.isEnter)
		//{
		//	curHp = gm.pMaxHP;
		//}
  //  }
    private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "EnemyBullet" || other.tag == "Enemy")
		{
			if(other.tag == "EnemyBullet") Destroy(other.gameObject); //총알 제거
			//데미지
			float bulletDmg = 0;
			float contactDmg = 0;
			if (other.tag == "EnemyBullet")
            {
				bulletDmg = other.GetComponent<EDamageData>().damage; //총알 데미지
				gm.curHp -= (gm.atkE + bulletDmg) * (1 - gm.defP); //적의 공격력과 플레이어의 방어력에 맞게 체력 감소
				Debug.Log("Damage = " + ((gm.atkE + bulletDmg) * (1 - gm.defP)));
			}
			else
            {
				contactDmg = other.GetComponent<EDamageData>().damage; //적과 충돌 데미지
				gm.curHp -= (gm.atkE + contactDmg) * (1 - gm.defP); //적의 공격력과 플레이어의 방어력에 맞게 체력 감소
				Debug.Log("Damage = " + ((gm.atkE + contactDmg) * (1 - gm.defP)));
			}
			//플레이어 사망
			if (gm.curHp <= 0.0f)
			{
				PlayerDie();
			}

			StartCoroutine(ShowBloodScreen());
			DisplayHpbar(); //hp 설정
		}
	}


	void PlayerDie()
	{
		//GameOver처리하기
		gm.isGameOver = true;
		//플레이어 사망시 적 AI 멈추기
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < enemies.Length; i++)
		{
			enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
		}
		//게임오버창 띄우기
		gm.gmOverUI.SetActive(true);
	}

	IEnumerator ShowBloodScreen()
    {
		bloodScreen.color = new Color(1, 0, 0, Random.Range(0.5f, 1.0f));
		yield return new WaitForSeconds(0.1f);
		bloodScreen.color = Color.clear;
    }
	void DisplayHpbar()
	{
		float ratio = gm.curHp / gm.pMaxHP;
		if (ratio > 0.5f)
			curColor.r = (1 - ratio) * 2.0f;
		else
			curColor.g = ratio * 2.0f;
		hpBar.color = curColor;
		hpBar.fillAmount = ratio;
	}
}
