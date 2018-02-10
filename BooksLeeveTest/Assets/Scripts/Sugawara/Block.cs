using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	[SerializeField] private int hp = 1;

	public void Damage()
	{
		hp--;
		if(hp<= 0)
		{
			hp = 0;
			GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>().ProbabilityCalculation();
			this.gameObject.SetActive(false);
		}
	}
}
