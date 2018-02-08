using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour {

	[SerializeField] private PhotonPlayerController.BombType bombType = PhotonPlayerController.BombType.NONE;
	private Rigidbody rb;
	private bool landing = false;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		rb.useGravity = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "PhotonPlayer")
		{
			other.transform.parent.GetComponent<PhotonPlayerController>().SetBombType(bombType);
			this.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (landing)
			return;
		if (transform.position.y < 1)
		{
			transform.position = new Vector3(transform.position.x,1,transform.position.z);
			rb.velocity = Vector3.zero;
			rb.useGravity = false;
			landing = true;
		}
	}

}
