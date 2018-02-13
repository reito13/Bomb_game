using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : PhotonBomb {

	private Vector3 startPos;
	private float setDistance;
	private float distance;

	[SerializeField] private float speed = 10.0f;

	public IEnumerator Set(Vector3 pos, Quaternion ro, float d)
	{
		transform.position = pos;
		transform.rotation = ro;
		startPos = transform.position;
		setActive = true;
		distance = 0;
		gameObject.SetActive(true);
		setDistance = d;
		rb.useGravity = false;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.AddForce(transform.forward * speed,ForceMode.Impulse);
		yield return new WaitForSeconds(0.3f);
		GetHit();
	}
	
	protected override void Update()
	{
		if (!setActive)
			return;

		if (photonView.isMine)
		{
			//現在の移動速度
			Vector3 velocity = rb.velocity;
			//移動速度を指定
			photonTransformView.SetSynchronizedValues(speed: velocity, turnSpeed: 0);
		}

		distance = Vector3.Distance(startPos, transform.position);

		Debug.Log(distance);
		if (distance > setDistance)
		{
			if (photonView.isMine)
			{
				photonView.RPC("Explosion", PhotonTargets.All);
			}
		}
	}
}
