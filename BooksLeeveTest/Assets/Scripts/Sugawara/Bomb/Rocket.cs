using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : PhotonBomb {

	private Vector3 startPos;
	private float setDistance;
	private float distance;
	private bool set = false;

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

		distance = Vector3.Distance(startPos, transform.position);
		if (distance > setDistance)
		{
			Debug.Log(startPos + "," + transform.position);
			Debug.Log(distance);
			if (photonView.isMine)
			{
				photonView.RPC("Explosion", PhotonTargets.All);
			}
		}
	}
}
