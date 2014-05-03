using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public int character; // type du personnage ?

	public int health;
	public int maxHealth;
	public int movementSpeed;
	public int range;
	public int damage;
	public int attackSpeed;

	public int hasheart = 0; // 0 à l'aller, 1 au retour


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

/*	void OnDestroy()
	{
		// Game Over.
		transform.parent.gameObject.AddComponent<GameOverScript>();
	}*/
}
