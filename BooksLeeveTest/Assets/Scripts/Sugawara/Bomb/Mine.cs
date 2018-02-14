using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : PhotonBomb {

	public override IEnumerator Set(Vector3 pos, Quaternion ro, Vector3 force)
	{
		rb.useGravity = true;
		GetComponent<Collider>().isTrigger = false;

		setActive = true;
		gameObject.SetActive(true);
		gameObject.layer = 8;

		transform.position = pos;
		transform.rotation = ro;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.AddForce(force, ForceMode.Impulse);

		yield return new WaitForSeconds(0.3f);
		GetHit();

		if (photonMode)
		{
			if (!photonView.isMine)
				yield break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "PhotonPlayer")
		{
			if (photonMode)
			{
				if (photonView.isMine)
				{
					photonView.RPC("Explosion", PhotonTargets.All);
				}
			}
			else
			{
				Explosion2();
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
			rb.angularVelocity = Vector3.zero;
		}
	}
}
