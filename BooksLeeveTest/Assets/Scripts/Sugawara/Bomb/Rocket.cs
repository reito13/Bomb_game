using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : PhotonBomb {

	private Vector3 startPos;
	private float setDistance;
	private float distance;

	private void Start()
	{

	}
	public override void Set(Vector3 pos, Quaternion ro, float time, float p)
	{
		Debug.Log(p);
		transform.position = pos;
		transform.rotation = ro;
		startPos = transform.position;
		setActive = true;
		gameObject.SetActive(true);
		setDistance = p;
		rb.useGravity = false;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.AddForce(transform.forward * power);
		Invoke("GetHit", 0.3f);
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
		if (distance > setDistance)
		{
			if (photonView.isMine)
			{
				photonView.RPC("Explosion", PhotonTargets.All);
			}
		}
	}
}
