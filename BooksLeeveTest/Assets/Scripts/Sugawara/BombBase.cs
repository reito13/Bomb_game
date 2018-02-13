using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBase : MonoBehaviour
{

	public GameObject explosion = null;
	public GameObject explosion2 = null;

	public Rigidbody rb = null;

	public void Explosion()
	{
		SoundManager.Instance.PlaySE("Explosion");

		GameObject obj = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
		obj = Instantiate(explosion2, transform.position, transform.rotation) as GameObject;

		Destroy(obj, 1.0f);
		Destroy(this.gameObject);
	}

	public virtual IEnumerator Set(Vector3 pos, Quaternion ro, Vector3 force)
	{
		gameObject.SetActive(true);
		gameObject.layer = 8;

		transform.position = pos;
		transform.rotation = ro;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.AddForce(force, ForceMode.Impulse);

		yield return new WaitForSeconds(0.3f);
		GetHit();
		yield return new WaitForSeconds(2.7f);
		Explosion();

	}

	private void GetHit()
	{
		gameObject.layer = 9;
	}

}
