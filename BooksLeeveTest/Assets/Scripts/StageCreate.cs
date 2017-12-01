using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreate : MonoBehaviour {

	[SerializeField] private GameObject block = null;
	[SerializeField] Transform parentObj = null; 
	private float startPos = 49.5f;
	// Use this for initialization
	private void Start () {
		Create();
	}

	private void Create()
	{
		for (int x = 0; x < 100; x++)
		{
			for (int z = 0; z < 100; z++)
			{
				for (int y = 0; y < 3; y++)
				{
					Vector3 pos = new Vector3(-49.5f + x, -y, -49.5f + z);
					GameObject obj = Instantiate(block, pos, transform.rotation) as GameObject;
					obj.transform.parent = parentObj.transform;
				}
			}
		}
	}
}
