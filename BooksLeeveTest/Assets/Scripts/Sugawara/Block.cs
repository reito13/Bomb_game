using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	[SerializeField] private int hp = 1;
	[SerializeField] private GameObject particlePrefab = null;

	[SerializeField] bool mainMode = true;

	public void Damage()
	{
		hp--;
		if(hp<= 0)
		{
			hp = 0;
			if(mainMode)
				GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>().ProbabilityCalculation();
			GameObject obj = Instantiate(particlePrefab,transform.position,transform.rotation);
		//	transform.parent.GetComponent<StageArea>().Impact();
			Destroy(obj,1.0f);
			this.gameObject.SetActive(false);
		}
	}
}
