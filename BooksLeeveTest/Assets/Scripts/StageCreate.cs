using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreate : Photon.MonoBehaviour
{
	[SerializeField] private GameObject[] blockPrefabs = null;
	[SerializeField] private Transform parentObj = null;
	[SerializeField] private int sideLength = 10;
	[SerializeField] private int areasQty = 4;
	[SerializeField] private Vector2 startPosition = Vector2.zero;
	[SerializeField] private int yValue = 3;
	[SerializeField] private float space = 3;

	public GameObject[] blocks;
	public Transform[] stages;

	private int spaceCountX = 0;
	private int spaceCountZ = 0;
	private int count = 0;

	private int x, y, z;
	GameObject obj;
	Vector3 pos;

	private void Start()
	{
		//SetUp();
	}

	public void SetUp()
	{
	spaceCountX = 0;
	spaceCountZ = 0;
	count = 0;

	System.Array.Resize(ref stages, areasQty * areasQty);
		System.Array.Resize(ref blocks, 4800);

		//photonView.RPC("Create",PhotonTargets.All);
		Create();
	}

	public void Reset()
	{
		for (int i = 0;i<stages.Length;i++)
		{
			Destroy(stages[i].gameObject);
		}
		System.Array.Resize(ref stages, 0);
		System.Array.Resize(ref blocks, 0);
	}

	private void Create()
	{
		for (x = 0; x < areasQty * areasQty; x++)
		{
			obj = new GameObject("stage" + (x + 1).ToString());
			obj.transform.parent = parentObj;
			stages[x] = obj.transform;
		}

		for (y = 0; y < yValue; y++)
		{
			for (z = 0; z < sideLength * areasQty; z++)
			{
				spaceCountZ = z / sideLength;

				for (x = 0; x < sideLength * areasQty; x++)
				{
					count++;
					spaceCountX = x / sideLength;
					pos = new Vector3(startPosition.x + x + space * spaceCountX, y, startPosition.y + z + space * spaceCountZ);

					obj = Instantiate(blockPrefabs[GetRandamBlock()], pos, transform.rotation) as GameObject;
					blocks[count-1] = obj;
					//obj.transform.parent = parentObj.transform;
					obj.transform.parent = stages[spaceCountX * areasQty + (1 + spaceCountZ) - 1];
					obj.name = count.ToString();
				}
			}
		}

		for (x = 0; x < areasQty * areasQty; x++)
		{
			int num = Random.Range(0,5);
		/*	if (PhotonNetwork.player.ID == 1)
			{
				photonView.RPC("MoveArea", PhotonTargets.All, x, num);
			}*/
			stages[x].Translate(Vector3.up * Random.Range(0, 5) * -1);
		}
	}

	private void MoveArea(int stageNum,int value)
	{
		stages[stageNum].Translate(Vector3.up * value * -1);
	}

	private int GetRandamBlock()
	{
		int r = Random.Range(0, 10);

		if (r <= 5)
			r = 0;
		else if (r <= 7)
			r = 1;
		else
			r = 2;

		return r;
	}

	public Vector3 CarrotCreate()
	{
		int num = Random.Range(3200,4800); //arraQty^2 * stageLength^2 * '2' +1, arraQty^2 * stageLength^2 * '3' +1
		while (!blocks[num].activeSelf)
		{
			if (num - 1600 >= 0)
			{
				num -= 1600;
			}
			else
			{
				num = Random.Range(3200, 4800);
			}
		}
		return blocks[num].transform.position;
	}

}


