using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	[SerializeField] private int hp = 1;
	[SerializeField] private bool menu = true;

	public void Damage(int number)
	{
		hp--;
		if(hp<= 0)
		{
			hp = 0;
			if (menu)
			{
				MainManager.Instance.AddScore(number);
			}
			Destroy(gameObject);
		}
	}
}
