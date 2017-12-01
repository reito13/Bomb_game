using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Block")
		{
			Destroy(other.gameObject);
		}
	}
}
