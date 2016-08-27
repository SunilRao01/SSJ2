using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player2DDialogue : MonoBehaviour 
{
	public Vector3 customDialogueScale;
	public float postDialogueDelay;

	// Dialogue Box
	private string dialogue;
	public GameObject dialogueTextContainer;
	
	// Variables
	public bool complete;
	
	// Scrolling Text Effect
	public float letterPause = 0.1f;
	private AudioClip sound;
	
	private string dialogueText;
	
	private bool scrollComplete;
	
	private dialoguePiece[] dialogues;
	public List<string> postDeathDialogue;
	public List<string> postFightDialogue;
	public int iterator;

	public bool autoStart;
	public bool autoProgression;
	public float dialogueDelay;
	public bool loadSceneAfterCompletion;
	public string nextScene;
	public int lineCharLimit;

	public string dialogueFileName;
	private TextAsset dialogueTextAsset;

	public bool dialogueComplete = false;
	private string currentDialogue;

	void Awake()
	{
		// Initialization

		// Disable main dialogue box
		dialogueTextContainer.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

		// Disable main dialogue text
		dialogueTextContainer.GetComponent<MeshRenderer>().enabled = false;		

		// Dialogue local vars
		dialogueTextAsset = Resources.Load(dialogueFileName) as TextAsset;

		initializeDialogue();
	}

	public void enableDialogue(string inputDialogue)
	{
		currentDialogue = inputDialogue;

		// Enable main dialogue box
		dialogueTextContainer.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
		
		// Enable main dialogue text
		dialogueTextContainer.GetComponent<MeshRenderer>().enabled = true;

		// TODO: iTween up the dialogue box from here!
		Vector3 targetScale;

		if (customDialogueScale == Vector3.zero)
		{
			targetScale = new Vector3(0.198f, 0.286f, 0.3552f);
		}
		else
		{
			targetScale = customDialogueScale;
		}

		iTween.ScaleTo(dialogueTextContainer, iTween.Hash("scale", targetScale, "oncompletetarget", gameObject,
		                                          "oncomplete", "afterScaleUp", "time", 0.5f));
		
	}

	void afterScaleUp()
	{
		Debug.Log("Starting dialogue!");

		startDialogue(currentDialogue);
	}

	public void disableDialogue()
	{
		// Disable main dialogue box
		dialogueTextContainer.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		
		// Disable main dialogue text
		dialogueTextContainer.GetComponent<MeshRenderer>().enabled = false;
	}

	public void initializeDialogue()
	{
		
		
		complete = false;
		scrollComplete = false;
		
		// Starting dialogue
		dialogueTextContainer.GetComponent<TextMesh>().text = "";
		dialogueText = "";
	}

	void afterScaleDown()
	{
		dialogueTextContainer.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		dialogueTextContainer.GetComponent<MeshRenderer>().enabled = false;
		

		disableDialogue();

	}

	public void shrinkDialogueBoxes()
	{
		Vector3 targetScale = new Vector3(0.01f, 0.003f, 0.01f);
		iTween.ScaleTo(dialogueTextContainer.gameObject, iTween.Hash("scale", targetScale, "oncompletetarget", gameObject,
		                                          "oncomplete", "afterScaleDown", "time", 0.5f));
	}


	private void startDialogue(string inputDialogue)
	{
		StartCoroutine(TypeText(inputDialogue));
	}

	public void restartDialogue()
	{
		StopAllCoroutines();
		//StopCoroutine(TypeText());

		iterator = 0;

		complete = false;
		scrollComplete = false;
		
		// Starting dialogue
		dialogue = dialogues[iterator].dialogueText;
		dialogueTextContainer.GetComponent<TextMesh>().text = "";
		dialogueText = "";
	}

	IEnumerator TypeText (string dialogueLine)
	{
		dialogue = dialogueLine;
		scrollComplete = false;

		int count = 1;
		int currentCharCount = 0;

		foreach (char letter in dialogue.ToCharArray()) 
		{
			dialogueText += letter;
			currentCharCount++;

			if (currentCharCount % lineCharLimit == 0)
			{
				if (letter != ' ')
				{
					dialogueText += "-";
				}

				dialogueText += "\n";
			}

			dialogueTextContainer.GetComponent<TextMesh>().text = dialogueText.ToString();			

			if (sound && count % 4 == 0)
			{
				GetComponent<AudioSource>().PlayOneShot (sound);
				yield return 0;
			}
			yield return new WaitForSeconds (letterPause);
			
			count++;
		}

		scrollComplete = true;

		yield return new WaitForSeconds(postDialogueDelay);

		shrinkDialogueBoxes();
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			StopAllCoroutines();

			shrinkDialogueBoxes();
		}
	}
}
