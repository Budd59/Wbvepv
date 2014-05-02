using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	public int type; // type du personnage ?

	public int health;
	public int movementspeed;
	public int range;
	public int damage;
	public int attackspeed;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy()
	{
		// Game Over.
		transform.parent.gameObject.AddComponent<GameOverScript>();
	}
}
