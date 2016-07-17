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
	public Vector3 customDialogueScale;

	// Player
	private Player3D c_player;

	// Dialogue Box
	private float _alpha = 0;
	private string dialogue;
	private GameObject dialogueTextContainer;
	private GameObject dialogueChocieTextContainer_1;
	private GameObject dialogueChocieTextContainer_2;
	private GameObject dialogueChocieTextContainer_3;
	
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
	public List<dialogueChoice> dialogueChoiceResponses;
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
	private int dialogueChoiceResponseIndex = 0;

	public string dialogueFileName;
	private TextAsset dialogueTextAsset;

	public bool dialogueComplete = false;

	void Awake()
	{
		// Initialization
		dialogueTextContainer = transform.GetChild(0).gameObject;
		dialogueChocieTextContainer_1 = transform.GetChild(1).gameObject;
		dialogueChocieTextContainer_2 = transform.GetChild(2).gameObject;
		dialogueChocieTextContainer_3 = transform.GetChild(3).gameObject;

		// Disable main dialogue box
		dialogueTextContainer.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

		// Disable main dialogue text
		dialogueTextContainer.GetComponent<MeshRenderer>().enabled = false;

		// Disable dialogue choices text
		dialogueChocieTextContainer_1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_2.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_3.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

		// Disable dialogue choice dialogue box
		dialogueChocieTextContainer_1.GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_2.GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_3.GetComponent<MeshRenderer>().enabled = false;

		// Store scale to revert to later
		originalDialogueChoiceScale = dialogueChocieTextContainer_1.transform.localScale;

		// Player
		c_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player3D>();

		// Dialogue local vars
		dialogueTextAsset = Resources.Load(dialogueFileName) as TextAsset;
		bool choicesFlag = false;
		bool responsesFlag = false;
		int startIndex = 1;
		dialogueChoice tempDialogueChoice = new dialogueChoice();

		// Parse Dialogue
		for (int i = 0; i < dialogueTextAsset.text.Length; i++)
		{
			if (i == 0)
			{
				continue;
			}
			else if (!choicesFlag)
			{
				// REGULAR dialogue to dialogue parsing
				if (dialogueTextAsset.text[i] == '-')
				{
					if (responsesFlag)
					{
						// TODO: Add last choice reponsa dialogue
						tempDialogueChoice.choice3 = (dialogueTextAsset.text.Substring(startIndex, (i-startIndex)));
						dialogueChoiceResponses.Add(tempDialogueChoice);

						responsesFlag = false;
					}

					dialogueArray.Add(dialogueTextAsset.text.Substring(startIndex, (i-startIndex)));
					startIndex = i+1;
				}
				// First time seeing a choice from a regular dialogue
				else if (dialogueTextAsset.text[i] == '1')
				{
					// Add previous line before choices begin
					dialogueArray.Add(dialogueTextAsset.text.Substring(startIndex, (i-startIndex)));

					dialogueChoicePositions.Add(dialogueArray.Count);

					choicesFlag = true;
					startIndex = i+1;
					continue;
				}
				else if (dialogueTextAsset.text[i] == '5')
				{
					// Add first choice response
					tempDialogueChoice.choice1 = (dialogueTextAsset.text.Substring(startIndex, (i-startIndex)));

					startIndex = i+1;
					continue;
				}
				else if (dialogueTextAsset.text[i] == '6')
				{
					// Add second choice response
					tempDialogueChoice.choice2 = (dialogueTextAsset.text.Substring(startIndex, (i-startIndex)));
					
					startIndex = i+1;
					continue;
				}
			}
			else
			{
				if (dialogueTextAsset.text[i] == '4')
				{
					tempDialogueChoice.choice3 = dialogueTextAsset.text.Substring(startIndex, (i-startIndex));

					dialogueChoices.Add(tempDialogueChoice);
					responsesFlag = true;
					choicesFlag = false;
					startIndex = i+1;

					continue;
				}
				else
				{
					if (dialogueTextAsset.text[i] == '2')
					{
						tempDialogueChoice.choice1 = dialogueTextAsset.text.Substring(startIndex, (i-startIndex));
						startIndex = i+1;
					}
					else if (dialogueTextAsset.text[i] == '3')
					{
						tempDialogueChoice.choice2 = dialogueTextAsset.text.Substring(startIndex, (i-startIndex));
						startIndex = i+1;
					}
				}
			}

			if (i == (dialogueTextAsset.text.Length-1))
			{
				dialogueArray.Add(dialogueTextAsset.text.Substring(startIndex, (i-startIndex)));
			}
		}

		initializeDialogue();
	}

	public void parseDialogue()
	{

	}

	public void enableDialogue()
	{
		// Enable main dialogue box
		dialogueTextContainer.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		
		// Enable main dialogue text
		dialogueTextContainer.GetComponent<MeshRenderer>().enabled = false;
	}

	public void disableDialogue()
	{
		// Disable main dialogue box
		dialogueTextContainer.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		
		// Disable main dialogue text
		dialogueTextContainer.GetComponent<MeshRenderer>().enabled = false;
		
		// Disable dialogue choices text
		dialogueChocieTextContainer_1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_2.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_3.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		
		// Disable dialogue choice dialogue box
		dialogueChocieTextContainer_1.GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_2.GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_3.GetComponent<MeshRenderer>().enabled = false;
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
			switch (dialogueChoiceResponses[dialogueChoiceResponseIndex].choice1[dialogueChoiceResponses[dialogueChoiceResponseIndex].choice1.Length-3])
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

			// Add dialogue choice response
			string choice = dialogueChoiceResponses[dialogueChoiceResponseIndex].choice1;
			string inputString = choice.Substring(0, choice.Length-5);
			dialogueArray.Insert(iterator, inputString);
			dialogueChoiceResponseIndex++;

			// Tween out dialogue choices
			disableDialgueChoices();



		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			switch (dialogueChoiceResponses[dialogueChoiceResponseIndex].choice2[dialogueChoiceResponses[dialogueChoiceResponseIndex].choice2.Length-3])
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



			// Add dialogue choice response
			string choice = dialogueChoiceResponses[dialogueChoiceResponseIndex].choice2;
			string inputString = choice.Substring(0, choice.Length-5);
			dialogueArray.Insert(iterator, inputString);
			dialogueChoiceResponseIndex++;

			// Tween out dialogue choices
			disableDialgueChoices();

		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			switch (dialogueChoiceResponses[dialogueChoiceResponseIndex].choice3[dialogueChoiceResponses[dialogueChoiceResponseIndex].choice3.Length-3])
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



			// Add dialogue choice response
			string choice = dialogueChoiceResponses[dialogueChoiceResponseIndex].choice3;
			string inputString = choice.Substring(0, choice.Length-5);
			dialogueArray.Insert(iterator, inputString);
			dialogueChoiceResponseIndex++;

			// Tween out dialogue choices
			disableDialgueChoices();
		}
	}

	void afterScaleDown()
	{
		GetComponent<MeshRenderer>().enabled = false;
		transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		
		GetComponent<Dialogue>().restartDialogue();
	}
	
	void afterScaleDownChoices()
	{
		GetComponent<MeshRenderer>().enabled = false;
		transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		
		dialogueChocieTextContainer_1.GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_2.GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_2.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_3.GetComponent<MeshRenderer>().enabled = false;
		dialogueChocieTextContainer_3.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
	}

	public void shrinkDialogueBoxes()
	{
		Vector3 targetScale = new Vector3(0.01f, 0.003f, 0.01f);
		iTween.ScaleTo(dialogueTextContainer.gameObject, iTween.Hash("scale", targetScale, "oncompletetarget", gameObject,
		                                          "oncomplete", "afterScaleDown", "time", 0.5f));
		
		Vector3 targetScale_2 = new Vector3(0.0148f, 0.00592f, 0.0207f);
		iTween.ScaleTo(dialogueChocieTextContainer_1, iTween.Hash("scale", targetScale_2, "oncompletetarget", gameObject,
		                                            "time", 0.5f));
		iTween.ScaleTo(dialogueChocieTextContainer_2, iTween.Hash("scale", targetScale_2, "oncompletetarget", gameObject,
		                                            "time", 0.5f));
		iTween.ScaleTo(dialogueChocieTextContainer_3, iTween.Hash("scale", targetScale_2, "oncompletetarget", gameObject,
		                                       "oncomplete", "afterScaleDownChoices", "time", 0.5f));
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
			if (Input.GetKeyDown(KeyCode.E) && scrollComplete && !dialogueChoicing && !dialogueComplete)
			{
				iterator++;
				dialogueText = "";

				if (iterator == dialogueArray.Count)
				{
					dialogueComplete = true;
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
		dialogueChocieTextContainer_1.GetComponent<TextMesh>().text = "";
		dialogueChocieTextContainer_2.GetComponent<TextMesh>().text = "";
		dialogueChocieTextContainer_3.GetComponent<TextMesh>().text = "";

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
		dialogueChocieTextContainer_1.GetComponent<TextMesh>().text = "";
		dialogueChocieTextContainer_2.GetComponent<TextMesh>().text = "";
		dialogueChocieTextContainer_3.GetComponent<TextMesh>().text = "";

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

		// JACKPOT BELOW HERE

		/*
		// Add dialogue choice response
		dialogueArray.Insert(iterator, dialogueChoiceResponses[dialogueChoiceResponseIndex].choice1);
		Debug.Log("Inserted " + dialogueChoiceResponses[dialogueChoiceResponseIndex].choice1 + " @ " + iterator);

		dialogueText = "";
		dialogue = dialogueArray[iterator];

		dialogueChoiceResponseIndex++;
		*/

		// Continue regular dialogue
		//StartCoroutine(TypeText());

		StopCoroutine(TypeText());

		// Set up dialogue to continue
		dialogueChoicing = false;
		dialogue = dialogueArray[iterator];
		dialogueTextContainer.GetComponent<TextMesh>().text = "";
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
		dialogueTextContainer.GetComponent<TextMesh>().text = "";
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
					dialogueChocieTextContainer_1.GetComponent<TextMesh>().text = dialogueText.ToString();
					break;
				case 1:
					dialogueChocieTextContainer_2.GetComponent<TextMesh>().text = dialogueText.ToString();
					break;
				case 2:
					dialogueChocieTextContainer_3.GetComponent<TextMesh>().text = dialogueText.ToString();
					break;
				default:
					break;
				}
			}
			else
			{
				dialogueTextContainer.GetComponent<TextMesh>().text = dialogueText.ToString();
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

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			//disableDialogue();
			//disableDialgueChoices();
			shrinkDialogueBoxes();
		}
	}
}
