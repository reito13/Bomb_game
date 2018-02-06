using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : PhotonBomb {

	public override void Set(Vector3 pos, Quaternion ro, float time, float p)
	{
		power = p;
		timer = time;

		rb.useGravity = true;
		GetComponent<Collider>().isTrigger = false;

		setActive = true;
		gameObject.SetActive(true);
		transform.position = pos;
		transform.rotation = ro;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.AddForce(transform.up * 0.5f * power);
		rb.AddForce(transform.forward * power);
		Invoke("GetHit", 0.3f);
	}

	protected override void Update()
	{
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "PhotonPlayer")
		{
			if (photonView.isMine)
			{
				photonView.RPC("Explosion", PhotonTargets.All);
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Block")
		{
			GetComponent<Collider>().isTrigger = true;
			transform.eulerAngles = new Vector3(90, 0, 0);
			rb.useGravity = false;
			rb.velocity = Vector3.zero;
		}
	}
}
