using UnityEngine;
using System.Collections;

public class TalkingMonster : MonoBehaviour 
{
	private GameObject o_dialogueBox;
	private bool withPlayer;

	void Awake () 
	{
		o_dialogueBox = transform.GetChild(0).gameObject;

		o_dialogueBox.GetComponent<MeshRenderer>().enabled = false;
		o_dialogueBox.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			o_dialogueBox.GetComponent<MeshRenderer>().enabled = true;
			o_dialogueBox.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

			Vector3 targetScale = new Vector3(0.05f, 0.02f, 0.07f);
			iTween.ScaleTo(o_dialogueBox, iTween.Hash("scale", targetScale, "oncompletetarget", gameObject,
			                                          "oncomplete", "afterScaleUp", "time", 0.5f));
			withPlayer = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Vector3 targetScale = new Vector3(0.01f, 0.003f, 0.01f);
			iTween.ScaleTo(o_dialogueBox, iTween.Hash("scale", targetScale, "oncompletetarget", gameObject,
			                                          "oncomplete", "afterScaleDown", "time", 0.5f));
			withPlayer = false;
		}
	}

	void afterScaleUp()
	{


		GetComponent<Dialogue>().startDialogue();
	}

	void afterScaleDown()
	{
		o_dialogueBox.GetComponent<MeshRenderer>().enabled = false;
		o_dialogueBox.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

		GetComponent<Dialogue>().restartDialogue();
	}
}
