using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	[SerializeField] private int hp = 1;
	[SerializeField] private GameObject particlePrefab = null;

	public void Damage()
	{
		hp--;
		if(hp<= 0)
		{
			hp = 0;
			GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>().ProbabilityCalculation();
			Instantiate(particlePrefab,transform.position,transform.rotation);
			this.gameObject.SetActive(false);
		}
	}
}
