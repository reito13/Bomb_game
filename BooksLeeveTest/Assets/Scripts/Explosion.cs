using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	private void Start()
	{
		Invoke("TimeOver",1.0f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Block")
		{
			Destroy(other.gameObject);
		}
	}

	private void TimeOver()
	{
		Destroy(this.gameObject);
	}
}
