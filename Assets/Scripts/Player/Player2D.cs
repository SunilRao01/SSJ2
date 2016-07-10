using UnityEngine;
using System.Collections;

public class Player2D : MonoBehaviour 
{
	// Player component instances
	private Rigidbody o_rigidbody;

	// Movement
	public float movementSpeed;
	public float maxSpeed;

	void Start () 
	{
		// Instantiate component instances
		o_rigidbody = GetComponent<Rigidbody>();


	}
	
	void Update () 
	{
		input();
	}

	private void input()
	{
		playerControls();
	}

	private void playerControls()
	{
		Vector2 movementDirection = new Vector2();

		if (o_rigidbody.velocity.x < maxSpeed)
		{
			movementDirection.x = Input.GetAxisRaw("Horizontal") * movementSpeed;
		}
		if (o_rigidbody.velocity.y < maxSpeed)
		{
			movementDirection.y = Input.GetAxisRaw("Vertical") * movementSpeed;
		}

		o_rigidbody.AddForce(movementDirection);
	}

	void OnCollisionEnter(Collision other)
	{

	}

	void OnCollisionExit(Collision other)
	{
		
	}
}
