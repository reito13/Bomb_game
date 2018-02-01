using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBomb : MonoBehaviour {

	[SerializeField] float power = 10.0f;
	[SerializeField] float rotatePower = 10.0f;

	[SerializeField] GameObject explosion = null;
	[SerializeField] GameObject explosion2 = null;

	[SerializeField] Rigidbody rb = null;


	public void Set(Vector3 pos, Quaternion ro, float time)
	{
		transform.position = pos;
		transform.rotation = ro;
		power = power * (1 + transform.localRotation.x);
		rb.AddForce(transform.forward * power);
		Debug.Log(rb.velocity);
		//rb.angularVelocity = new Vector3(-1, 0, 0) * rotatePower;
		Invoke("GetHit", 0.3f);
		Invoke("ExplosionSet", time);
	}

	private void GetHit()
	{
		gameObject.layer = 9;
	}

	public void ExplosionSet()
	{
		Explosion();
	}

	public void Explosion()
	{
		SoundManager.Instance.PlaySE("Explosion");

		GameObject obj = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
		obj.GetComponent<ExplosionStage>().Set(1);
		obj = Instantiate(explosion2, transform.position, transform.rotation) as GameObject;
		obj.GetComponent<ExplosionObject>().Set(1);

		Destroy(obj, 1.0f);
		Destroy(this.gameObject);
	}
}
