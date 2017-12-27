using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreate : MonoBehaviour {

	[SerializeField] private GameObject block = null;
	[SerializeField] private GameObject block2 = null;
	[SerializeField] Transform parentObj = null; 
	private float startPos = 31.0f;
	private int count = 0;
	// Use this for initialization
	private void Start () {
		Create();
	}

	private void Create()
	{
		for (int y = 0; y < 3; y++)
		{
			for (int z = 0; z < startPos; z++)
			{
				for (int x = 0; x < startPos; x++)
				{
					count++;
					Vector3 pos = new Vector3(-startPos/2 + x, -y, -startPos/2 + z);

					GameObject obj;

					if ((z % 6 == 0 || x % 6 == 0) && y == 0)
					{
						obj = Instantiate(block2, pos, transform.rotation) as GameObject;
					}
					else
					{
						obj = Instantiate(block, pos, transform.rotation) as GameObject;
					}
					obj.transform.parent = parentObj.transform;
					obj.name = count.ToString();
				}
			}
		}
	}
}

