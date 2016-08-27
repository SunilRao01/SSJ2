using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void start3dWorld()
	{
		Application.LoadLevel("Sandbox");
	}

	public void start2dWorld()
	{
		Application.LoadLevel("2DWorld");
	}

	public void exitGame()
	{
		Application.Quit();
	}
}
