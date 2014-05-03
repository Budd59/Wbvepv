using UnityEngine;
using System.Collections;

public class StartRetourScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		PlayerScript player = otherCollider.gameObject.GetComponent<PlayerScript>();

		if(player.hasheart == 1)
		{
			if(Application.loadedLevel > 1)
			{
				Application.LoadLevel(Application.loadedLevel - 1);
			}
			else
			{
				Application.LoadLevel(0);
				// Application.LoadLevel("Final_Scene");
			}
		}
	}
}
