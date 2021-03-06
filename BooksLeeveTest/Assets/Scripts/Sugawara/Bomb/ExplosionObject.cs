﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObject : MonoBehaviour
{
	[SerializeField] private float power = 5.0f;
	private Rigidbody rb;

	private int number;

	private void Awake()
	{
		Destroy(this.gameObject,0.5f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			other.transform.parent.GetComponent<Player>().Damage(transform);
		}
		else if (other.tag == "PhotonPlayer")
		{
			other.transform.parent.GetComponent<PhotonPlayerController>().Damage(transform);
		}
		else if (other.tag == "MenuPlayer")
		{
			other.transform.parent.GetComponent<MenuPlayer>().Damage(transform);
		}
		else if (other.tag == "Carrot")
		{
			other.GetComponent<BombBase>().Explosion();
		}
		else if(other.tag == "StageObject")
		{
			rb = other.GetComponent<Rigidbody>();
			Vector3 dir = transform.position - other.transform.position;
			dir.x = (((dir.x >= 0) ? 3 : -3) - dir.x) * -1;
			dir.y = 3.5f;
			dir.z = (((dir.z >= 0) ? 3 : -3) - dir.z) * -1;
			rb.AddForce(dir * power, ForceMode.Impulse);

		}
	}

}
