using UnityEngine;
using System.Collections;

public class Player2D : MonoBehaviour 
{
	// Player component instances
	private Rigidbody o_rigidbody;

	// Movement
	public float movementSpeed;
	public float maxSpeed;
	public float jumpForce;
	private bool isGrounded;

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
		// Horizontal Movement
		Vector2 movementDirection = new Vector2();

		if (o_rigidbody.velocity.x < maxSpeed)
		{
			movementDirection.x = Input.GetAxisRaw("Horizontal") * movementSpeed;
		}

		if (isGrounded && Input.GetAxisRaw("Vertical") == 1)
		{
			o_rigidbody.AddForce(Vector3.up * jumpForce);
		}

		o_rigidbody.AddForce(movementDirection);
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("ground"))
		{
			isGrounded = true;
		}
	}

	void OnCollisionExit(Collision other)
	{
		if (other.gameObject.CompareTag("ground"))
		{
			isGrounded = false;
		}
	}
}
