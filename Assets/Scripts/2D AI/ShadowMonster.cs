using UnityEngine;
using System.Collections;

public class ShadowMonster : MonoBehaviour 
{
	public float bottomInterval;
	public float topInterval;
	private Rigidbody c_rigidbody;

	private bool isPhasing;
	private bool isMoving;
	private int phaseIndex = 1;
	public float movementForce;
	private Vector3 startingPosition;

	void Start () 
	{
		// Starting position is where phase 1 will start
		startingPosition = transform.position;

		c_rigidbody = GetComponent<Rigidbody>();

		isPhasing = true;
		StartCoroutine(phaseProgression());
	}
	
	void Update () 
	{
	
	}

	IEnumerator phaseProgression()
	{
		while (isPhasing)
		{
			float currentInterval;

			if (phaseIndex == 1 || phaseIndex == 3 || phaseIndex == 6 || phaseIndex == 8)
			{
				currentInterval = topInterval;
			}
			else
			{
				currentInterval = bottomInterval;
			}

			Vector3 phasePosition = startingPosition;
			// TODO: Change position depending on phase index
			Vector3 newPosition = transform.position;
			Vector3 newScale = transform.localScale;
			switch (phaseIndex)
			{
			case 1:
				newScale.x *= -1;
				transform.localScale = newScale;
				break;
				case 2:
					phasePosition.x -= 0.5f;
					phasePosition.y -= 1.7f;
					break;
				case 3:
					phasePosition.x += 7.4f;
					break;
				case 4:
					phasePosition.x += 6.5f;
					phasePosition.y -= 1.7f;
					break;
				case 5:
					newScale.x *= -1;

					transform.localScale = newScale;	
					phasePosition.x += 11.4f;
					phasePosition.y -= 1.7f;
					break;
				case 6:
					phasePosition.x += 11.0f;
					break;
				case 7:
					phasePosition.x += 5.0f;
					phasePosition.y -= 1.7f;
					break;
				case 8:
					phasePosition.x += 4.0f;
					break;
			}
			transform.position = phasePosition;
			isMoving = true;
			StartCoroutine(startPhase(phaseIndex));


			yield return new WaitForSeconds(currentInterval);

			isMoving = false;
			StopCoroutine(startPhase(phaseIndex));

			if (phaseIndex < 8)
			{
				phaseIndex++;
			}
			else
			{
				phaseIndex = 1;
			}
		}
	}

	IEnumerator startPhase(int phase)
	{
		while (isMoving)
		{
			yield return new WaitForSeconds(0.1f);

			Vector3 movementVector = (Vector3.right * movementForce);

			if (phaseIndex > 4)
			{
				movementVector.x *= -1;


			}

			c_rigidbody.velocity = movementVector;
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
