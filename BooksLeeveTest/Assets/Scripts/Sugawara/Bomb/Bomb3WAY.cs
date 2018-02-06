using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb3WAY : PhotonBomb {

	[SerializeField] private bool right = false;
	public override void Set(Vector3 pos, Quaternion ro, float time, float p)
	{
		power = p;
		timer = time;
		setActive = true;
		gameObject.SetActive(true);
		transform.position = pos;
		transform.rotation = ro;
		if (right)
		{
			transform.Rotate(new Vector3(0,15,0));
		}
		else
		{
			transform.Rotate(new Vector3(0,-15,0));
		}
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.AddForce(transform.up * 0.5f * power);
		rb.AddForce(transform.forward * power);
		Invoke("GetHit", 0.3f);
	}
}
