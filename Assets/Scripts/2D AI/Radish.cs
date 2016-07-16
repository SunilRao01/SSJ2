using UnityEngine;
using System.Collections;

public class Radish : MonoBehaviour 
{
	enum Direction
	{
		left,
		right,
		up,
		down
	}

	// Components
	private Rigidbody c_rigidbody;

	public float movementForce;
	private bool isMoving;
	private Direction currentDirection;

	void Awake () 
	{
		c_rigidbody = GetComponent<Rigidbody>();

		currentDirection = Direction.right;

		isMoving = true;
		StartCoroutine(radishMovement());
	}
	
	void Update () 
	{
	
	}

	IEnumerator radishMovement()
	{
		while (isMoving)
		{
			yield return new WaitForSeconds(0.1f);

			if (currentDirection == Direction.right)
			{
				c_rigidbody.velocity = (Vector3.right * movementForce);
			}
			else if (currentDirection == Direction.left)
			{
				c_rigidbody.velocity = (Vector3.left * movementForce);
			}
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("DirectionSwitch"))
		{
			if (currentDirection == Direction.right)
			{
				currentDirection = Direction.left;

				Vector3 newScale = transform.localScale;
				newScale.x *= -1;
				transform.localScale = newScale;
			}
			else
			{
				currentDirection = Direction.right;
				
				Vector3 newScale = transform.localScale;
				newScale.x *= -1;
				transform.localScale = newScale;
			}
		}
	}
}
