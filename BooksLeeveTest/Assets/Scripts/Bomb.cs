using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	[SerializeField] float power = 10.0f;
	[SerializeField] Vector3 dir = Vector3.zero;
	[SerializeField] GameObject explosion = null;
	[SerializeField] GameObject explosion2 = null;
	private Player playerScript;

	public void Set(float time,Player script)
	{
		playerScript = script;
		Rigidbody rb = GetComponent<Rigidbody>();
		Debug.Log(transform.localRotation.x);
		power = power * (1 + transform.localRotation.x);
		rb.AddForce(transform.forward * power);
		//rb.AddForce(transform.up * power * 0.7f);
		Invoke("Explosion", time);
	}

	private void Explosion()
	{
		playerScript.BombCount = 1;
		Instantiate(explosion, transform.position, transform.rotation);
		GameObject obj = Instantiate(explosion2,transform.position,transform.rotation)as GameObject;
		Destroy(obj,1.0f);
		Destroy(this.gameObject);
	}

}
