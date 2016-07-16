using UnityEngine;
using System.Collections;

public class Cyclops : MonoBehaviour 
{
	private Rigidbody c_rigidbody;
	public float switchInterval;
	private bool isMoving;
	private bool isRight = true;
	public float movementForce;

	void Start () 
	{
		c_rigidbody = GetComponent<Rigidbody>();

		isMoving = true;

		StartCoroutine(cyclopsSwitchDirection());
		StartCoroutine(cyclopsMovement());
	}

	void Update () 
	{
	
	}

	IEnumerator cyclopsMovement()
	{
		while (isMoving)
		{
			yield return new WaitForSeconds(0.05f);

			if (isRight)
			{
				c_rigidbody.velocity = (Vector3.right * movementForce);
			}
			else
			{
				c_rigidbody.velocity = (Vector3.left * movementForce);
			}
		}
	}

	IEnumerator cyclopsSwitchDirection()
	{
		while (isMoving)
		{
			yield return new WaitForSeconds(switchInterval);

			if (isRight)
			{
				isRight = false;
			}
			else
			{
				isRight = true;
			}

			Vector3 newScale = transform.localScale;
			newScale.x *= -1;
			transform.localScale = newScale;
		}
	}
}
