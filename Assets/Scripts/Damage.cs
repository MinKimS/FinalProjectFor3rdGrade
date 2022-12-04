using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
	public float currHp = 100;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "EnemyBullet")
		{
			Destroy(other.gameObject);
			currHp -= 5.0f;
			Debug.Log("Player HP = " + currHp.ToString());
			if (currHp <= 0.0f)
			{
				PlayerDie();
			}
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
}
