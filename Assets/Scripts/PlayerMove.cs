using UnityEngine;

/// <summary>
/// Player controller and behavior
/// </summary>
public class PlayerMove : MonoBehaviour
{
	/// <summary>
	/// 1 - The speed of the ship
	/// </summary>
	public Vector2 speed = new Vector2(50, 50);
	public float turnSmoothing = 15f;
	
	// 2 - Store the movement
	private Vector2 movement;
	
	void Update()
	{
		// 3 - Retrieve axis information
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		MovementManagement (inputX, inputY);


	}

	void MovementManagement(float horizontal, float vertical)
	{
		if (horizontal != 0f || vertical != 0f)
		{
			movement = new Vector2(speed.x * horizontal, speed.y * vertical);
		}

		if (horizontal == 0f && vertical == 0f)
		{
			movement = new Vector2(0 , 0);
		}
	}
	
	void FixedUpdate()
	{

		rigidbody2D.velocity = movement;
	}
}
