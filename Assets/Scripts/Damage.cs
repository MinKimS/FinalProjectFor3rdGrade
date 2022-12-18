using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
	public Image bloodScreen; //피격 이미지
	public Image hpBar;// 체력바

	Color initColor = new Vector4(0, 1.0f, 0.0f, 1.0f);
	Color curColor; //현재 색상
	GameManager gm;

    private void Start()
	{
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();

		hpBar.color = initColor; //hp바 초기 색상
		curColor = initColor;
    }

    private void OnTriggerEnter(Collider other)
	{
		//적 또는 적의 총알과 충돌 시 데미지를 입음
		if (other.tag == "EnemyBullet" || other.tag == "Enemy")
		{
			if(other.tag == "EnemyBullet") Destroy(other.gameObject); //총알 제거
			//데미지
			float bulletDmg = 0;
			float contactDmg = 0;
			if (other.tag == "EnemyBullet")
            {
				bulletDmg = other.GetComponent<EDamageData>().damage; //총알 데미지
				gm.curHp -= (gm.atkE + bulletDmg) * (gm.defP / (1 + gm.defP)); //적의 공격력과 플레이어의 방어력에 맞게 체력 감소
			}
			else
            {
				contactDmg = other.GetComponent<EDamageData>().damage; //적과 충돌 데미지
				gm.curHp -= (gm.atkE + contactDmg) * (gm.defP / (1 + gm.defP)); //적의 공격력과 플레이어의 방어력에 맞게 체력 감소
			}
			//플레이어 사망
			if (gm.curHp <= 0.0f)
			{
				PlayerDie();
			}

			//피격화면 출력
			StartCoroutine(ShowBloodScreen());
		}
	}

    private void Update()
    {
		DisplayHpbar(); //hp 설정
	}


    void PlayerDie()
	{
		//GameOver처리하기
		gm.isGameOver = true;
		//플레이어 사망시 적 AI 멈추기
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		//적의 OnPlayerDie를 호출해 플레이어가 죽었을 때 행동 처리
		for (int i = 0; i < enemies.Length; i++)
		{
			enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
		}
		//게임오버창 띄우기
		gm.gmOverUI.SetActive(true);
	}

	IEnumerator ShowBloodScreen()
    {
		//매번 투명도가 다르게 출력이 된다
		bloodScreen.color = new Color(1, 0, 0, Random.Range(0.5f, 1.0f));
		yield return new WaitForSeconds(0.1f);
		bloodScreen.color = Color.clear;
    }
	void DisplayHpbar()
	{
		//체력 비율
		float ratio = gm.curHp / gm.pMaxHP;
		//비율에 따라 체력바의 색상변경
		if (ratio > 0.5f)
			curColor.r = (1 - ratio) * 2.0f;
		else
			curColor.g = ratio * 2.0f;
		hpBar.color = curColor;
		hpBar.fillAmount = ratio;
	}
}
