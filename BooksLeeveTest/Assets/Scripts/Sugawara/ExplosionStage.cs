using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionStage : MonoBehaviour {

	private int number;

	public void Set(int num)
	{
		number = num;
		Invoke("TimeOver",1.0f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Block")
		{
			other.GetComponent<Block>().Damage(number);
		}
	}

	private void TimeOver()
	{
		Destroy(this.gameObject);
	}
}
