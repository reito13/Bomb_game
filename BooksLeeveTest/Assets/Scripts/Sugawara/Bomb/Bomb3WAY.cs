using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb3WAY : PhotonBomb {

	[SerializeField] private bool right = false;

	public override IEnumerator Set(Vector3 pos, Quaternion ro, Vector3 force)
	{
		setActive = true;
		gameObject.SetActive(true);
		gameObject.layer = 8;

		transform.position = pos;
		transform.rotation = ro;

		if (right)
		{
			transform.Rotate(new Vector3(0, 15, 0));
		}
		else
		{
			transform.Rotate(new Vector3(0, -15, 0));
		}
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.AddForce(force, ForceMode.Impulse);

		yield return new WaitForSeconds(0.3f);
		GetHit();

		if (!photonView.isMine)
			yield break;

		yield return new WaitForSeconds(2.7f);
		photonView.RPC("Explosion", PhotonTargets.All);
	}
}
