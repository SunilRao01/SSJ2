using UnityEngine;
using System.Collections;

public class TalkingMonster : MonoBehaviour 
{
	public bool is2D;
	public Vector3 customDialogueChoiceScale;
	private GameObject o_dialogueBox;
	private Dialogue c_dialogue;
	private bool withPlayer;
	private TextAsset dialogueText;
	private bool activatedChoices = false;
	private bool isDialoguing = false;

	private GameObject wizardSprite;
	private GameObject dialogueChoice1;
	private GameObject dialogueChoice2;
	private GameObject dialogueChoice3;

	void Awake () 
	{
		wizardSprite = transform.parent.GetChild(0).gameObject;
	}

	void Start()
	{

	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (withPlayer && !isDialoguing)
			{
				withPlayer = true;
				isDialoguing = true;
				
				GetComponent<Dialogue>().enableDialogue();
				GetComponent<Dialogue>().initializeDialogue();
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			withPlayer = true;

			if (is2D)
			{
				other.GetComponent<Player2DDialogue>().enableDialogue("[E]");

				Debug.Log("Wizard entering...");
				// TODO: Animate/Rotate wizard into 2D
				Vector3 targetRotation = transform.eulerAngles;
				targetRotation.y -= 90;

				iTween.RotateTo(wizardSprite, iTween.Hash("rotation", targetRotation, "time", 1, "islocal", true));
			}
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			withPlayer = false;
			isDialoguing = false;

			if (is2D)
			{
				Debug.Log("Wizard leaving...");

				// TODO: Animate/Rotate wizard into 3D
				Vector3 targetRotation = transform.eulerAngles;
				targetRotation.y = 270;

				iTween.RotateTo(wizardSprite, iTween.Hash("rotation", targetRotation, "time", 1, "islocal", true));
			}
		}
	}
}
