using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	// Prefabs

	public Player2D player;
	public Canvas levelStartScreenCanvas;

	public float levelStartScreenDuration;
	private bool start = false;

	void Start () 
	{
		DontDestroyOnLoad(transform.gameObject);

		// Disable on start
		levelStartScreenCanvas.enabled = false;

				

		start = true;
		StartCoroutine(levelStartScreen());
	}
	
	void Update () 
	{
	
	}

	IEnumerator levelStartScreen()
	{
		while (start)
		{
			
			// Disable Player
			player.lockMovement(true);

			// Enable all start menu UI items
			levelStartScreenCanvas.enabled = true;

			yield return new WaitForSeconds(levelStartScreenDuration);

			// Disable all start menu UI items
			levelStartScreenCanvas.enabled = false;

			
			// Enable Player
			player.lockMovement(false);

			start = false;
		}
	}
}
