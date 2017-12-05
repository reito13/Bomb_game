using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreate : MonoBehaviour {

	[SerializeField] private GameObject block = null;
	[SerializeField] Transform parentObj = null; 
	private int setPos = 30;

	private void Start () {
		Create();
	}

	private void Create()
	{
		for (int x = 0; x < setPos; x++)
		{
			for (int z = 0; z < setPos; z++)
			{
				for (int y = 0; y < 3; y++)
				{
					Vector3 pos = new Vector3((-setPos/2) + x, -y, -(setPos/2)+ z);
					GameObject obj = Instantiate(block, pos, transform.rotation) as GameObject;
					obj.transform.parent = parentObj.transform;
				}
			}
		}
	}
}
