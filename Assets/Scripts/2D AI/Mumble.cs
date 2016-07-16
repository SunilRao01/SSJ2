using UnityEngine;
using System.Collections;

public class Mumble : MonoBehaviour 
{
	private Rigidbody c_rigidbody;

	public float movementInterval;
	private bool isCycling;
	private bool isRight = true;
	private int cycleIndex = 1;

	public float movementForce;

	void Start () 
	{
		c_rigidbody = GetComponent<Rigidbody>();

		isCycling = true;
		StartCoroutine(mumbleMovement());
	}
	

	void Update () 
	{
	
	}

	IEnumerator mumbleMovement()
	{
		while (isCycling)
		{
			yield return new WaitForSeconds(movementInterval);

			Vector3 movementVector = new Vector3(movementForce/3, movementForce, 0);

			if (cycleIndex < 3)
			{
				if (isRight)
				{
					cycleIndex++;
				}
				else
				{
					if (cycleIndex != 1)
					{
						cycleIndex--;
						movementVector.x *= -1;
					}
					else
					{
						isRight = true;
						cycleIndex++;
					}
				}
			}
			else
			{
				if (isRight)
				{
					isRight = false;
				}
				else
				{
					isRight = true;
				}
					
				movementVector.x *= -1;
				cycleIndex--;
			}

			c_rigidbody.AddForce(movementVector);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.name == "Player2D")
		{
			Destroy (gameObject);
		}
	}
}
