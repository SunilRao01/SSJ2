using UnityEngine;
using System.Collections;

public class Player2D : MonoBehaviour 
{
	enum Direction
	{
		left,
		right,
		up,
		down
	};

	// Player component instances
	private Rigidbody o_rigidbody;

	// Movement
	public float movementSpeed;
	public float maxSpeed;
	public float jumpForce;
	private bool isGrounded;
	private Direction currentDirection;

	void Start () 
	{
		// Instantiate component instances
		o_rigidbody = GetComponent<Rigidbody>();

		currentDirection = Direction.right;
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

			if (movementDirection.x < 0)
			{
				currentDirection = Direction.left;
			}
			else if (movementDirection.x > 0)
			{
				currentDirection = Direction.left;
			}
		}
		o_rigidbody.AddForce(movementDirection);
		if (isGrounded && Input.GetAxisRaw("Vertical") == 1)
		{
			o_rigidbody.AddForce(Vector3.up * jumpForce);

			if (currentDirection == Direction.left && Input.GetAxisRaw("Horizontal") != 0)
			{
				o_rigidbody.AddForce(Vector3.right * (jumpForce/4));
			}
			else if (currentDirection == Direction.right && Input.GetAxisRaw("Horizontal") != 0)
			{
				o_rigidbody.AddForce(Vector3.left * (jumpForce/4));
			}
		}


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
