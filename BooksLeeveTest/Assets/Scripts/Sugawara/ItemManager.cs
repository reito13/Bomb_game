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

	private PhotonView photonView = null;

	private void Awake()
	{
		photonView = PhotonView.Get(this);
	}

	[PunRPC]
	private void ItemCreate()
	{
		Debug.Log(transform.position);
		Instantiate(itemPrefabs[Random.Range(0,7)],transform.position,transform.rotation);
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
			Debug.Log(random + "<" + probability);
			photonView.RPC("ItemCreate",PhotonTargets.All);
		}
		else
			Debug.Log("はずれ");
	}

}
