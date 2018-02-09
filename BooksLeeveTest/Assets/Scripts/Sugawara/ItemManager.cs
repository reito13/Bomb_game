using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonMonoBehaviour<ItemManager> {

	[SerializeField] private GameObject[] itemPrefabs = null;
	private float probability;
//	private float currentProbability = 0;
	private int count = 0;
	private float rate = 0.05f;
	private float random;
	private Vector3 basePos;
	private Vector3 createPos;

	private PhotonView photonView = null;

	private void Start()
	{
		photonView = PhotonView.Get(this);
		basePos = transform.position;
		Debug.Log(transform.localPosition);
	}

	[PunRPC]
	private void ItemCreate()
	{
		float r1 = Random.Range(-7.0f, 7.0f);
		float r2 = Random.Range(-7.0f, 7.0f);
		createPos = new Vector3(r1,10,r2);
		Debug.Log(r1 + "," + r2);
		createPos += basePos;
		Instantiate(itemPrefabs[Random.Range(0,7)],createPos,transform.rotation);
	}

	public void ProbabilityCalculation()
	{
		if (!photonView.isMine)
			return;

		count++;
		probability = (count * rate);
		random = Random.Range(0.0f,100.0f);

		if (random < probability)
		{
			Debug.Log(probability + "%");
			photonView.RPC("ItemCreate",PhotonTargets.All);
		}
	}

}
