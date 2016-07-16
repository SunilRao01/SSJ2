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

	// Attacking
	private GameObject playerSword;
	public float attackDuration;
	private bool isAttacking;
	private Vector3 playerScale;
	private Vector3 playerScaleRight;

	void Start () 
	{
		playerScale = transform.localScale;
		playerScaleRight = transform.localScale;
		playerScaleRight.x *= -1;

		// Instantiate component instances
		o_rigidbody = GetComponent<Rigidbody>();

		// Instantiate children objects
		playerSword = transform.GetChild(1).gameObject;
	
		currentDirection = Direction.right;

		Physics.IgnoreCollision(playerSword.GetComponent<BoxCollider>(), GetComponent<BoxCollider>());
		disableSword();
	}
	
	void Update () 
	{
		// Set rotation of player to zero
		transform.eulerAngles = Vector3.zero;

		if (currentDirection == Direction.right)
		{
			transform.localScale = playerScale;
		}
		else
		{
			transform.localScale = playerScaleRight;
		}

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
				currentDirection = Direction.right;
			}
		}
		o_rigidbody.AddForce(movementDirection);

		// Lock z-rotation
		Vector3 playerRotation = transform.eulerAngles;
		playerRotation.z = 0;
		transform.eulerAngles = playerRotation;

		// Jumping
		if (isGrounded && Input.GetAxisRaw("Vertical") == 1)
		{
			Debug.Log("Should jump!");
			o_rigidbody.AddForce(Vector3.up * jumpForce);
		}

		// Attacking
		if (!isAttacking && Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(playerAttack(currentDirection));
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

	private void disableSword()
	{
		playerSword.GetComponent<SpriteRenderer>().enabled = false;
		playerSword.GetComponent<BoxCollider>().enabled = false;
	}

	private void enableSword()
	{
		playerSword.GetComponent<SpriteRenderer>().enabled = true;
		playerSword.GetComponent<BoxCollider>().enabled = true;
	}

	IEnumerator playerAttack(Direction inputDirection)
	{
		// Modify sword position depending on direction
		if (currentDirection == Direction.left)
		{

			Vector3 newSwordPosition = transform.position;
			newSwordPosition.x -= 0.82f;
			newSwordPosition.y -= 0.2f;
			playerSword.transform.position = newSwordPosition;
		}
		else if (currentDirection == Direction.right)
		{

			Vector3 newSwordPosition = transform.position;
			newSwordPosition.x += 0.82f;
			newSwordPosition.y -= 0.2f;

			playerSword.transform.position = newSwordPosition;
		}

		//Spawn sword
		enableSword();

		// TODO: Add 'swish' SFX

		isAttacking = true;

		// TODO: Wait
		yield return new WaitForSeconds(attackDuration);

		// TODO: Remove sword
		disableSword();



		isAttacking = false;
	}
}
