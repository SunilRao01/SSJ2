using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct dialoguePiece
{
	public string dialogueText;
}
[System.Serializable]
public struct dialogueChoice
{
	public string choice1;
	public string choice2;
	public string choice3;
}

public class Dialogue : MonoBehaviour 
{
	// Dialogue Box
	private float _alpha = 0;
	private string dialogue;
	private TextMesh dialogueTextContainer;
	
	// Variables
	public bool complete;
	
	// Scrolling Text Effect
	public float letterPause = 0.1f;
	private AudioClip sound;
	
	private string dialogueText;
	
	private bool scrollComplete;
	
	private dialoguePiece[] dialogues;
	public string[] dialogueArray;
	public List<dialogueChoice> dialogueChoices;
	private int iterator;

	public bool autoStart;
	public bool autoProgression;
	public float dialogueDelay;
	public bool loadSceneAfterCompletion;
	public string nextScene;

	void Awake()
	{
		dialogueTextContainer = transform.GetChild(0).GetComponent<TextMesh>();
	}

	// Use this for initialization
	void Start () 
	{	
		iterator = 0;
		dialogues = new dialoguePiece[dialogueArray.Length];
		for (int i = 0; i < dialogueArray.Length; i++)
		{
			dialogues[i].dialogueText = dialogueArray[i];
		}
		
		complete = false;
		scrollComplete = false;
		
		// Starting dialogue
		dialogue = dialogues[iterator].dialogueText;

		if (autoStart)
		{
			startDialogue();
		}
		
	}
	
	void Update () 
	{
		// Make everything alpha==0, complete flag set to true
		if (iterator == dialogueArray.Length)
		{
			_alpha = 0;
			complete = true;

			if (loadSceneAfterCompletion)
			{
				Application.LoadLevel(nextScene);
			}
		}
		
		if (!complete)
		{
			
			// Handles Input
			if (Input.GetKeyDown(KeyCode.E) && scrollComplete)
			{
				iterator++;
				dialogueText = "";
				
				if (iterator < dialogueArray.Length)
				{
					dialogue = dialogues[iterator].dialogueText;
					StartCoroutine(TypeText ());
				}
			}
		}
	}
	
	public void startDialogue()
	{
		_alpha = 1;
		StartCoroutine(TypeText());
	}

	public void restartDialogue()
	{
		StopCoroutine(TypeText());

		iterator = 0;
		dialogues = new dialoguePiece[dialogueArray.Length];
		for (int i = 0; i < dialogueArray.Length; i++)
		{
			dialogues[i].dialogueText = dialogueArray[i];
		}
		
		complete = false;
		scrollComplete = false;
		
		// Starting dialogue
		dialogue = dialogues[iterator].dialogueText;
		dialogueTextContainer.text = "";
		dialogueText = "";
	}

	IEnumerator TypeText () 
	{
		scrollComplete = false;
		int count = 1;
		
		foreach (char letter in dialogue.ToCharArray()) 
		{
			dialogueText += letter;
			dialogueTextContainer.text = dialogueText.ToString();

			if (sound && count % 4 == 0)
			{
				GetComponent<AudioSource>().PlayOneShot (sound);
				yield return 0;
			}
			yield return new WaitForSeconds (letterPause);
			
			count++;
		}
		
		scrollComplete = true;

		if (autoProgression)
		{
			StartCoroutine(waitThenType());
		}
	}
	
	IEnumerator waitThenType()
	{
		yield return new WaitForSeconds(dialogueDelay);

		iterator++;
		dialogueText = "";
		
		if (iterator < dialogueArray.Length)
		{
			dialogue = dialogues[iterator].dialogueText;
			StartCoroutine(TypeText ());
		}
	}
	
}
