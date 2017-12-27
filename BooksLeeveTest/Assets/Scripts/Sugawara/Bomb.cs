using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	[SerializeField] float power = 10.0f;
	[SerializeField] float rotatePower = 10.0f;
	[SerializeField] Vector3 dir = Vector3.zero;
	[SerializeField] GameObject explosion = null;
	[SerializeField] GameObject explosion2 = null;
	private Player playerScript;
	private int number;

	public void Set(int num,float time,Player script)
	{
		number = num;
		playerScript = script;
		Rigidbody rb = GetComponent<Rigidbody>();
		power = power * (1 + transform.localRotation.x);
		rb.AddForce(transform.forward * power);
		rb.angularVelocity = new Vector3(-1,0,0) * rotatePower;
		Invoke("GetHit",0.3f);
		Invoke("Explosion", time);
	}

	private void GetHit()
	{
		gameObject.layer = 9;
	}

	private void Explosion()
	{
		SoundManager.Instance.PlaySE("Explosion");

		playerScript.BombCount = 1;
		GameObject obj = Instantiate(explosion, transform.position, transform.rotation)as GameObject;
		obj.GetComponent<Explosion>().Set(number);
		obj = Instantiate(explosion2,transform.position,transform.rotation)as GameObject;
		Destroy(obj,1.0f);
		Destroy(this.gameObject);
	}

}
