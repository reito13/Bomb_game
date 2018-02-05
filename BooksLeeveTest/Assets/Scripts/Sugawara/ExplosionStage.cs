using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionStage : MonoBehaviour {

	private void Awake()
	{
		Destroy(this.gameObject, 1.0f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Block")
		{
			other.GetComponent<Block>().Damage();
		}
	}

}
