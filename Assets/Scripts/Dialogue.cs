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
	// Player
	private Player3D c_player;

	// Dialogue Box
	private float _alpha = 0;
	private string dialogue;
	private TextMesh dialogueTextContainer;
	private TextMesh dialogueChocieTextContainer_1;
	private TextMesh dialogueChocieTextContainer_2;
	private TextMesh dialogueChocieTextContainer_3;
	
	// Variables
	public bool complete;
	
	// Scrolling Text Effect
	public float letterPause = 0.1f;
	private AudioClip sound;
	
	private string dialogueText;
	
	private bool scrollComplete;
	
	private dialoguePiece[] dialogues;
	public List<string> dialogueArray;
	public List<dialogueChoice> dialogueChoices;
	public List<int> dialogueChoicePositions;
	public int iterator;

	public bool autoStart;
	public bool autoProgression;
	public float dialogueDelay;
	public bool loadSceneAfterCompletion;
	public string nextScene;
	public int dialogueChoiceIndex = 0;
	private bool dialogueChoicing;
	private bool dialogueChoiceFinished = true;
	public int dialogueChoicePosition = 0;
	public int lineCharLimit;
	private Vector3 originalDialogueChoiceScale;

	void Awake()
	{
		dialogueTextContainer = transform.GetChild(0).GetComponent<TextMesh>();
		dialogueChocieTextContainer_1 = transform.GetChild(1).GetComponent<TextMesh>();
		dialogueChocieTextContainer_2 = transform.GetChild(2).GetComponent<TextMesh>();
		dialogueChocieTextContainer_3 = transform.GetChild(3).GetComponent<TextMesh>();

		// Disbale dialogue choices at start
		dialogueChocieTextContainer_1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_2.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_3.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

		dialogueChocieTextContainer_1.GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_2.GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_3.GetComponent<MeshRenderer>().enabled = false;

		originalDialogueChoiceScale = dialogueChocieTextContainer_1.transform.localScale;

		// Player
		c_player = GetComponent<Player3D>();
	}

	public void initializeDialogue()
	{
		iterator = 0;
		dialogueChoiceIndex = 0;
		dialogueChoicePosition = 0;
		dialogues = new dialoguePiece[dialogueArray.Count];
		for (int i = 0; i < dialogueArray.Count; i++)
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

	void choiceSelection()
	{
		// M: Mean
		// F: Funny
		// C: Compassionate

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			switch (dialogueChoices[dialogueChoiceIndex].choice1[dialogueChoices[dialogueChoiceIndex].choice1.Length-1])
			{
				case 'M':
					c_player.mean++;
					break;
				case 'F':
					c_player.funny++;
					break;
				case 'C':
					c_player.compassion++;
					break;
			}



			// Tween out dialogue choices
			disableDialgueChoices();

			// Continue regular dialogue
			StartCoroutine(TypeText());

		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			switch (dialogueChoices[dialogueChoiceIndex].choice2[dialogueChoices[dialogueChoiceIndex].choice2.Length-1])
			{
				case 'M':
					c_player.mean++;
					break;
				case 'F':
					c_player.funny++;
					break;
				case 'C':
					c_player.compassion++;
					break;
			}

			// Tween out dialogue choices
			disableDialgueChoices();
			
			// Continue regular dialogue
			StartCoroutine(TypeText());
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			switch (dialogueChoices[dialogueChoiceIndex].choice3[dialogueChoices[dialogueChoiceIndex].choice3.Length-1])
			{
				case 'M':
					c_player.mean++;
					break;
				case 'F':
					c_player.funny++;
					break;
				case 'C':
					c_player.compassion++;
					break;
			}

			// Tween out dialogue choices
			disableDialgueChoices();
			
			// Continue regular dialogue
			StartCoroutine(TypeText());
		}
	}

	void Update() 
	{
		// Make everything alpha==0, complete flag set to true
		if (iterator == dialogueArray.Count)
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
			// Dialogue progression
			if (Input.GetKeyDown(KeyCode.E) && scrollComplete && !dialogueChoicing)
			{
				iterator++;
				dialogueText = "";

				if (iterator == dialogueArray.Count)
				{
					Vector3 targetScale = new Vector3(0.01f, 0.003f, 0.01f);
					iTween.ScaleTo(dialogueTextContainer.gameObject, iTween.Hash("scale", targetScale, "oncompletetarget", gameObject,
					                                                             "oncomplete", "afterScaleDown", "time", 0.5f));
					
					dialogueTextContainer.GetComponent<MeshRenderer>().enabled = false;
					dialogueTextContainer.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
				}
				else if (iterator == dialogueChoicePositions[dialogueChoiceIndex])
				{
					dialogueChoicing = true;

					dialogue = dialogueChoices[dialogueChoiceIndex].choice1;

					// Enable dialogue choices
					enableDialogueChoices();
				}
				else if (iterator < dialogueArray.Count)
				{
					if (dialogueChoicing)
					{
						dialogueChoicing = false;
						iterator--;
					}

					dialogue = dialogues[iterator].dialogueText;
					StartCoroutine(TypeText ());
				}
			}

			// Choice selection
			if (dialogueChoicing)
			{
				choiceSelection();
			}
		}
	}

	void enableDialogueChoices()
	{
		// Reset any exisitng dialogue choice text
		dialogueChocieTextContainer_1.text = "";
		dialogueChocieTextContainer_2.text = "";
		dialogueChocieTextContainer_3.text = "";

		// Enable renderer
		dialogueChocieTextContainer_1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
		dialogueChocieTextContainer_2.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
		dialogueChocieTextContainer_3.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
		
		dialogueChocieTextContainer_1.GetComponent<MeshRenderer>().enabled = true;
		dialogueChocieTextContainer_2.GetComponent<MeshRenderer>().enabled = true;
		dialogueChocieTextContainer_3.GetComponent<MeshRenderer>().enabled = true;

		// Tween up
		Vector3 targetScale = new Vector3(0.05f, 0.02f, 0.07f);

		Debug.Log("Scaling choices up");
		iTween.ScaleTo(dialogueChocieTextContainer_1.gameObject, iTween.Hash("scale", targetScale, "time", 0.5f));
		iTween.ScaleTo(dialogueChocieTextContainer_2.gameObject, iTween.Hash("scale", targetScale, "time", 0.5f));
		iTween.ScaleTo(dialogueChocieTextContainer_3.gameObject, iTween.Hash("scale", targetScale, "oncompletetarget", gameObject,
		                                                                     "oncomplete", "afterDialogueChoiceTweenUp", "time", 0.5f));
		
		dialogueChoiceFinished = false;
	}

	void disableDialgueChoices()
	{
		dialogueChocieTextContainer_1.text = "";
		dialogueChocieTextContainer_2.text = "";
		dialogueChocieTextContainer_3.text = "";

		// Tween down
		Vector3 targetScale =  originalDialogueChoiceScale;
		
		iTween.ScaleTo(dialogueChocieTextContainer_1.gameObject, iTween.Hash("scale", targetScale, "time", 0.5f));
		iTween.ScaleTo(dialogueChocieTextContainer_2.gameObject, iTween.Hash("scale", targetScale, "time", 0.5f));
		iTween.ScaleTo(dialogueChocieTextContainer_3.gameObject, iTween.Hash("scale", targetScale, "oncompletetarget", gameObject,
		                                                                     "oncomplete", "afterDialogueChoiceTweenDown", "time", 0.5f));
		
		dialogueChoiceFinished = false;
	}

	void afterDialogueChoiceTweenUp()
	{
		StartCoroutine(TypeText());
	}

	void afterDialogueChoiceTweenDown()
	{
		// Disable renderer
		dialogueChocieTextContainer_1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_2.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_3.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		
		dialogueChocieTextContainer_1.GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_2.GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_3.GetComponent<MeshRenderer>().enabled = false;

		// Set up dialogue to continue

		dialogueChoicing = false;
		dialogue = dialogueArray[iterator];
		dialogueTextContainer.text = "";
		dialogueText = "";
		dialogueChoiceIndex++;
		dialogueChoicePosition = 0;

		Debug.Log("Starting dialogue: " + dialogueArray[iterator].ToString());

		StartCoroutine(TypeText());
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
		dialogues = new dialoguePiece[dialogueArray.Count];
		for (int i = 0; i < dialogueArray.Count; i++)
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
		int currentCharCount = 0;
		foreach (char letter in dialogue.ToCharArray()) 
		{
			dialogueText += letter;
			currentCharCount++;

			if (currentCharCount % lineCharLimit == 0)
			{
				dialogueText += "\n";
			}

			if (dialogueChoicing)
			{
				switch (dialogueChoicePosition)
				{
				case 0:
					dialogueChocieTextContainer_1.text = dialogueText.ToString();
					break;
				case 1:
					dialogueChocieTextContainer_2.text = dialogueText.ToString();
					break;
				case 2:
					dialogueChocieTextContainer_3.text = dialogueText.ToString();
					break;
				default:
					break;
				}
			}
			else
			{
				dialogueTextContainer.text = dialogueText.ToString();
			}

			if (sound && count % 4 == 0)
			{
				GetComponent<AudioSource>().PlayOneShot (sound);
				yield return 0;
			}
			yield return new WaitForSeconds (letterPause);
			
			count++;
		}


		if (dialogueChoicing)
		{
			dialogueChoicePosition++;
			
			if (dialogueChoicePosition < 3)
			{
				if (dialogueChoicePosition == 1)
				{
					dialogue = dialogueChoices[dialogueChoiceIndex].choice2;
				}
				else if (dialogueChoicePosition == 2)
				{
					dialogue = dialogueChoices[dialogueChoiceIndex].choice3;
				}

				dialogueText = "";
				StartCoroutine(TypeText());
			}
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
		
		if (iterator < dialogueArray.Count)
		{
			dialogue = dialogues[iterator].dialogueText;
			StartCoroutine(TypeText ());
		}
	}
	
}
