using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Photon.MonoBehaviour {
	
	[SerializeField] private GameObject[] itemPrefabs = null;
	[SerializeField] private GameObject[] carrotPrefabs = null;
	private float probability;
//	private float currentProbability = 0;
	private int count = 0;
	private float rate = 0.05f;
	private float random;
	private Vector3 basePos;
	private Vector3 createPos;

	private PhotonView photonView = null;

	private StageCreate stageCreate = null;

	private void Start()
	{
		photonView = PhotonView.Get(this);
		if (!photonView.isMine)
		{
			//this.gameObject.SetActive(false);
			//return;
		}
		basePos = transform.position;

		StartCoroutine(CarrotCreateCoroutine());
		stageCreate = GameObject.FindGameObjectWithTag("StageCreate").GetComponent<StageCreate>();

	}

	[PunRPC]
	private void ItemCreate()
	{
	/*	float r1 = Random.Range(-7.0f, 7.0f);
		float r2 = Random.Range(-7.0f, 7.0f);
		createPos = new Vector3(r1,10,r2);
		Debug.Log(r1 + "," + r2);
		createPos += basePos;
		Instantiate(itemPrefabs[Random.Range(0,7)],createPos,transform.rotation);*/
	}

	[PunRPC]
	private void CarrotCreate(Vector3 pos)
	{
		Instantiate(carrotPrefabs[0],pos,transform.rotation);
	}

	public void CarrotCreateSet()
	{
		Vector3 pos = stageCreate.CarrotCreate();
		pos.y += 0.75f;
		photonView.RPC("CarrotCreate",PhotonTargets.All,pos);
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

	private IEnumerator CarrotCreateCoroutine()
	{
		yield return new WaitForSeconds(1);
		if (PhotonNetwork.player.ID == 1)
		{
			for (int i = 0; i < 32; i++)
			{
				CarrotCreateSet();
			}

			while (true)
			{
				CarrotCreateSet();
				yield return new WaitForSeconds(5);
			}
		}
	}

}
