using UnityEngine;
using System.Collections;

public class TalkingMonster : MonoBehaviour 
{
	public Vector3 customDialogueChoiceScale;
	private GameObject o_dialogueBox;
	private Dialogue c_dialogue;
	private bool withPlayer;
	private TextAsset dialogueText;
	private bool activatedChoices = false;
	private bool isDialoguing = false;

	private GameObject dialogueChoice1;
	private GameObject dialogueChoice2;
	private GameObject dialogueChoice3;

	void Awake () 
	{
		o_dialogueBox = transform.GetChild(0).gameObject;
		o_dialogueBox.GetComponent<MeshRenderer>().enabled = false;
		o_dialogueBox.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

		/*c_dialogue = GetComponent<Dialogue>();

		dialogueText = Resources.Load("test") as TextAsset;
		int startIndex = 1;
		bool choicesFlag = false;
		dialogueChoice tempDialogueChoice = new dialogueChoice();

		c_dialogue.dialogueArray.Clear();
		c_dialogue.dialogueChoices.Clear();
		c_dialogue.dialogueChoicePositions.Clear();

		dialogueChoice1 = transform.GetChild(1).gameObject;
		dialogueChoice2 = transform.GetChild(2).gameObject;
		dialogueChoice3 = transform.GetChild(3).gameObject;


		for (int i = 0; i < dialogueText.text.Length; i++)
		{
			if (i == 0)
			{
				continue;
			}
			else if (!choicesFlag)
			{
				if (dialogueText.text[i] == '-')
				{
					c_dialogue.dialogueArray.Add(dialogueText.text.Substring(startIndex, (i-startIndex)));
					startIndex = i+1;
				}
				else if (dialogueText.text[i] == '1')
				{
					c_dialogue.dialogueArray.Add(dialogueText.text.Substring(startIndex, (i-startIndex)));

					c_dialogue.dialogueChoicePositions.Add(c_dialogue.dialogueArray.Count);

					choicesFlag = true;
					startIndex = i+1;
					continue;
				}
			}
			else
			{
				if (dialogueText.text[i] == '-')
				{
					tempDialogueChoice.choice3 = dialogueText.text.Substring(startIndex, (i-startIndex));

					c_dialogue.dialogueChoices.Add(tempDialogueChoice);
					choicesFlag = false;
					startIndex = i+1;
					continue;
				}
				else
				{
					if (dialogueText.text[i] == '2')
					{
						tempDialogueChoice.choice1 = dialogueText.text.Substring(startIndex, (i-startIndex));
						startIndex = i+1;
					}
					else if (dialogueText.text[i] == '3')
					{
						tempDialogueChoice.choice2 = dialogueText.text.Substring(startIndex, (i-startIndex));
						startIndex = i+1;
					}
				}
			}

			if (i == (dialogueText.text.Length-1))
			{
				c_dialogue.dialogueArray.Add(dialogueText.text.Substring(startIndex, (i-startIndex)));
			}
		}

		c_dialogue.initializeDialogue();*/

	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (withPlayer && !isDialoguing)
			{
				withPlayer = true;
				o_dialogueBox.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
				o_dialogueBox.GetComponent<MeshRenderer>().enabled = true;

				GetComponent<Dialogue>().initializeDialogue();

				o_dialogueBox.GetComponent<TextMesh>().text = "";
				Vector3 targetScale;

				if (customDialogueChoiceScale == Vector3.zero)
				{
					targetScale = new Vector3(0.054f, 0.0162f, 0.054f);
				}
				else
				{
					targetScale = customDialogueChoiceScale;
				}
				iTween.ScaleTo(o_dialogueBox, iTween.Hash("scale", targetScale, "oncompletetarget", gameObject,
				                                          "oncomplete", "afterScaleUp", "time", 0.5f));
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			withPlayer = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{

			withPlayer = false;
			isDialoguing = false;


		}
	}

	void afterScaleUp()
	{

		Debug.Log("Starting dialogue!");
		GetComponent<Dialogue>().startDialogue();
		isDialoguing = true;
	}

	void afterScaleDown()
	{
		o_dialogueBox.GetComponent<MeshRenderer>().enabled = false;
		o_dialogueBox.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

		GetComponent<Dialogue>().restartDialogue();
	}

	void afterScaleDownChoices()
	{
		o_dialogueBox.GetComponent<MeshRenderer>().enabled = false;
		o_dialogueBox.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		
		dialogueChoice1.GetComponent<MeshRenderer>().enabled = false;
		dialogueChoice1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		dialogueChoice2.GetComponent<MeshRenderer>().enabled = false;
		dialogueChoice2.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
		dialogueChoice3.GetComponent<MeshRenderer>().enabled = false;
		dialogueChoice3.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
	}
}
