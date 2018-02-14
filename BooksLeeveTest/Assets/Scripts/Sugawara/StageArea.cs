using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageArea : MonoBehaviour {

	private int count = 16;
	private Vector3 randomRotate;
	public void Impact()
	{
		count--;
		randomRotate = new Vector3(Random.Range(-2f,2f), Random.Range(-2f,2f), Random.Range(-2f, 2f));
		transform.Rotate(randomRotate);
		//transform.Translate(randomRotate * 0.1f);
	}
}
