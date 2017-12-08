using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreate : MonoBehaviour {

	[SerializeField] private GameObject block = null;
	[SerializeField] Transform parentObj = null; 
	private float startPos = 50.0f;
	// Use this for initialization
	private void Start () {
		Create();
	}

	private void Create()
	{
		for (int x = 0; x < startPos; x++)
		{
			for (int z = 0; z < startPos; z++)
			{
				for (int y = 0; y < 3; y++)
				{
					Vector3 pos = new Vector3(-50/2 + x, -y, -50/2 + z);
					GameObject obj = Instantiate(block, pos, transform.rotation) as GameObject;
					obj.transform.parent = parentObj.transform;
				}
			}
		}
	}
}
