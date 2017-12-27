using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

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
			MainManager.Instance.AddScore(number);
			MainManager.Instance.StageDelete(other);
		}
	}

	private void TimeOver()
	{
		Destroy(this.gameObject);
	}
}
