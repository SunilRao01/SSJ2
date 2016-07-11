using UnityEngine;
using System.Collections;

public class DimensionSwitch : MonoBehaviour 
{
	public GameObject player2DPrefab;
	public GameObject player3DPrefab;
	private bool is3D = false;
	public bool canSwitch = true;

	void Start () 
	{
	
	}
	
	void Update () 
	{
	
	}

	void dimensionSwitch(Vector3 spawnPosition)
	{
		spawnPosition.z = 0;

		if (is3D)
		{

			GameObject player2D = (GameObject) Instantiate(player2DPrefab, spawnPosition, Quaternion.identity);


			is3D = false;
		}
		else
		{
			GameObject player3D = (GameObject) Instantiate(player3DPrefab, spawnPosition, Quaternion.identity);
			
			is3D = true;
		}
	}

	public void initiate(GameObject inputObject)
	{
		if (canSwitch)
		{
			Destroy (inputObject);
			dimensionSwitch(inputObject.transform.position);
			//other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * 5);
			StartCoroutine(dimensionSwitchCooldown());
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Debug.Log("Switching dimensions");
			initiate(other.gameObject);
		}
	}

	IEnumerator dimensionSwitchCooldown()
	{
		canSwitch = false;
		yield return new WaitForSeconds(1);
		canSwitch = true;
	}
}
