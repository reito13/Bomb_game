using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreate : MonoBehaviour {

	[SerializeField] private GameObject[] blocks = null;
	[SerializeField] Transform parentObj = null; 
	public float startPos = 31.0f;
	public int yValue = 3;
	private int count = 0;
	// Use this for initialization
	private void Start () {
		Create();
	}

	private void Create()
	{
		for (int y = 0; y < yValue; y++)
		{
			for (int z = 0; z < startPos; z++)
			{
				for (int x = 0; x < startPos; x++)
				{
					count++;
					Vector3 pos = new Vector3(-startPos / 2 + x, -y, -startPos / 2 + z);

					GameObject obj;

					int r = Random.Range(0, 10); //0~9
					if (r <= 5)
						r = 0;
					else if (r <= 7)
						r = 1;
					else
						r = 2;
					obj = Instantiate(blocks[r], pos, transform.rotation) as GameObject;
					
					obj.transform.parent = parentObj.transform;
					obj.name = count.ToString();
				}
			}
		}
	}

	/*private void CreateLine()
	{
		if ((z % 6 == 0 || x % 6 == 0) && y == 0)
		{
			obj = Instantiate(blocks[3], pos, transform.rotation) as GameObject;
		}
		else if (y == 0)
		{
			obj = Instantiate(blocks[0], pos, transform.rotation) as GameObject;
		}
		else if (y == 1)
		{
			obj = Instantiate(blocks[1], pos, transform.rotation) as GameObject;
		}
		else
		{
			obj = Instantiate(blocks[2], pos, transform.rotation) as GameObject;
		}
	}*/
}

