using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundTile : MonoBehaviour 
{
	public List<Sprite> sprites;
	public float minChangeInterval;
	public float maxChangeInterval;
	private SpriteRenderer o_spriteRenderer;

	void Awake () 
	{
		o_spriteRenderer = GetComponent<SpriteRenderer>();
		int randomPosition = Random.Range(0, sprites.Count);
		o_spriteRenderer.sprite = sprites[randomPosition];
		StartCoroutine(transitionGround());
	}
	
	void Update () 
	{
	
	}

	IEnumerator transitionGround()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(minChangeInterval, maxChangeInterval));

			int randomPosition = Random.Range(0, sprites.Count);
			o_spriteRenderer.sprite = sprites[randomPosition];
		}
	}
}
