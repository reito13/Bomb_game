using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	[SerializeField] float power = 10.0f;
	[SerializeField] Vector3 dir = Vector3.zero;
	[SerializeField] GameObject explosion = null;
	private Player playerScript;

	public void Set(float time,Player script)
	{
		playerScript = script;
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * power);
		rb.AddForce(transform.up * power * 0.7f);
		Invoke("Explosion", time);
	}

	private void Explosion()
	{
		playerScript.BombCount = 1;
		Instantiate(explosion,transform.position,transform.rotation);
		Destroy(this.gameObject);
	}

}
