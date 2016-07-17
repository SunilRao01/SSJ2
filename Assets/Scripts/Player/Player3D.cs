using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player3D : MonoBehaviour 
{
	// Stats
	public int mean = 0;
	public int funny = 0;
	public int compassion = 0;

	private Text meanLabel;
	private Text funnyLabel;
	private Text compassionLabel;

	void Start () 
	{
		meanLabel = GameObject.Find("MeanLabel").GetComponent<Text>();
		funnyLabel = GameObject.Find("FunnyLabel").GetComponent<Text>();
		compassionLabel = GameObject.Find("CompassionateLabel").GetComponent<Text>();
	}
	
	void Update () 
	{
		meanLabel.text = mean.ToString();
		funnyLabel.text = funny.ToString();
		compassionLabel.text = compassion.ToString();
	}


}
