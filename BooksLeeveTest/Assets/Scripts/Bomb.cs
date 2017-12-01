using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	[SerializeField] float power = 10.0f;
	[SerializeField] Vector3 dir = Vector3.zero;
	[SerializeField] GameObject explosion = null;

	public void Set(float time)
	{
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * power);
		//rb.AddForce(transform.up * power * 0.3f);
		Invoke("Explosion", time);
	}

	private void Explosion()
	{
		Instantiate(explosion,transform.position,transform.rotation);
		Destroy(this.gameObject);
	}

}
