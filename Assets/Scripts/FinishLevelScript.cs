using UnityEngine;
using System.Collections;

public class FinishLevelScript : MonoBehaviour {

	private int isbossspawned = 0;
	private int isbossalive;
	
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		PlayerScript player = otherCollider.gameObject.GetComponent<PlayerScript>();

		if(player != null && player.hasheart == 0)
		{
			if(isbossspawned == 0)
			{
				SpawnBoss();
			}
			else
			{
				if(isbossalive == 0)
				{
					Application.LoadLevel(Application.loadedLevel + 1);
				}
			}
		}
	}

	void SpawnBoss()
	{


		isbossspawned = 1;
		isbossalive = 1;
	}
}
