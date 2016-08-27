using UnityEngine;
using System.Collections;
 using System.Collections.Generic;

public class Player2D : Player 
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
	private Animator o_animator;
	private Player2DDialogue playerDialogue;

	// Movement
	private bool locked;
	public float movementSpeed;
	private static float staticMovementSpeed;
	public float maxSpeed;
	public float jumpForce;
	private bool isGrounded;
	private Direction currentDirection;

	// Attacking
	private GameObject playerSword;
	public float attackDuration;
	public bool isAttacking;
	private bool hasJumpedDown;

	// TODO: Call 'Player2DDialogue''s enableDialogue(string inputDialogue)

	void Awake()
	{
		staticMovementSpeed = movementSpeed;

		playerDialogue = GetComponent<Player2DDialogue>();
	}

	void Start () 
	{
		locked = false;

		// Instantiate component instances
		o_rigidbody = GetComponent<Rigidbody>();
		o_animator = GetComponent<Animator>();

		// Instantiate children objects
		playerSword = transform.GetChild(1).gameObject;
	
		currentDirection = Direction.right;

		Physics.IgnoreCollision(playerSword.GetComponent<BoxCollider>(), GetComponent<BoxCollider>());
		disableSword();

		// NOTE
		// Initial dialogue when game starts


		//StartCoroutine(waitThenTalkToSelf());
	}
	
	IEnumerator waitThenTalkToSelf()
	{
		yield return new WaitForSeconds(3);

		playerDialogue.enableDialogue("Here we go again...");
	}



	void Update () 
	{
		// Set rotation of player to zero
		transform.eulerAngles = Vector3.zero;

		if (currentDirection == Direction.right)
		{
			GetComponent<SpriteRenderer>().flipX = false;
		}
		else
		{
			GetComponent<SpriteRenderer>().flipX = true;
		}

		input();
	}

	public void lockMovement(bool lockedInput)
	{
		if (lockedInput)
		{
			movementSpeed = 0;
		}
		else 
		{
			movementSpeed = staticMovementSpeed;
		}
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

		if (Mathf.Abs(o_rigidbody.velocity.x) >= 0.1f)
		{
			o_animator.SetBool("walking", true);
		}
		else
		{
			o_animator.SetBool("walking", false);
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
			hasJumpedDown = false;
			o_animator.SetBool("jumpingUp", true);
			o_rigidbody.AddForce(Vector3.up * jumpForce);
		}

		// Attacking
		if (!isAttacking && Input.GetKeyDown(KeyCode.Space) && !o_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
		{
			StartCoroutine(playerAttack(currentDirection));
		}

		if (transform.position.y < -2)
		{
			// Restart level 
			int randomDialogueIndex = Random.Range(0, playerDialogue.postDeathDialogue.Count-1);
			playerDialogue.enableDialogue(playerDialogue.postDeathDialogue[randomDialogueIndex]);

			StartCoroutine(waitThenRestart());
		}

		if (o_rigidbody.velocity.y < -2f && !hasJumpedDown)
		{
			o_animator.SetBool("jumpingDown", true);
			hasJumpedDown = true;
		}
	}

	IEnumerator waitThenRestart()
	{
		yield return new WaitForSeconds(3);

		Application.LoadLevel(Application.loadedLevelName);
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("ground"))
		{
			isGrounded = true;
			o_animator.SetBool("jumpingDown", false);
			o_animator.SetBool("jumpingUp", false);
		}

		// TODO: When player collides with 
		if (other.gameObject.CompareTag("EnemyProjectile"))
		{
			reduceHealth();
		}
	}

	void OnCollisionExit(Collision other)
	{
		if (other.gameObject.CompareTag("ground"))
		{
			isGrounded = false;
		}
	}

	public void reduceHealth()
	{
		health--;

		if (health <= 0)
		{
			// Restart level when player dies
			int randomDialogueIndex = Random.Range(0, playerDialogue.postDeathDialogue.Count-1);
			playerDialogue.enableDialogue(playerDialogue.postDeathDialogue[randomDialogueIndex]);

			StartCoroutine(waitThenRestart());
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
		isAttacking = true;
		Debug.Log("Attacking");
		o_animator.SetBool("attacking", true);

		// Modify sword position depending on direction
		if (currentDirection == Direction.left)
		{

			Vector3 newSwordPosition = transform.position;
			newSwordPosition.x -= 0.801f;
			//newSwordPosition.y -= 0.2f;
			playerSword.transform.position = newSwordPosition;
		}
		else if (currentDirection == Direction.right)
		{

			Vector3 newSwordPosition = transform.position;
			newSwordPosition.x += 0.801f;
			//newSwordPosition.y -= 0.2f;

			playerSword.transform.position = newSwordPosition;
		}

		//yield return new WaitForSeconds(0.1f);

		//Spawn sword
		enableSword();

		// TODO: Add 'swish' SFX

		

		// TODO: Wait
		yield return new WaitForSeconds(attackDuration);

		o_animator.SetBool("attacking", false);

		// TODO: Remove sword
		disableSword();



		isAttacking = false;

		

	}

	void OnColliderEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			Debug.Log("Input dialogue should've started");
			playerDialogue.enableDialogue("[E]");
		}
	}
}
