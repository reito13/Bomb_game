using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	[SerializeField] float power = 10.0f;
	[SerializeField] float rotatePower = 10.0f;

	[SerializeField] GameObject explosion = null;
	[SerializeField] GameObject explosion2 = null;

	[SerializeField] private Player playerScript = null;
	[SerializeField] private int number = 0;

	[SerializeField] Rigidbody rb = null;

	public bool setActive = false;
	public bool setExplosion = false;

	private void Awake()
	{
		transform.parent = GameObject.Find("Bombs").transform;
	}

	public void Set(Vector3 pos,Quaternion ro,float time)
	{
		setActive = true;
		gameObject.SetActive(true);
		transform.position = pos;
		transform.rotation = ro;
		power = power * (1 + transform.localRotation.x);
		rb.AddForce(transform.forward * power);
		rb.angularVelocity = new Vector3(-1,0,0) * rotatePower;
		Invoke("GetHit",0.3f);
		Invoke("ExplosionSet", time);
	}

	private void GetHit()
	{
		gameObject.layer = 9;
	}

	public void ExplosionSet()
	{
		if (!gameObject.activeSelf)
			return;

		//setExplosion = true;
		Explosion();
	}

	public void Explosion()
	{
		SoundManager.Instance.PlaySE("Explosion");

		playerScript.BombCount = 1;

		GameObject obj = Instantiate(explosion, transform.position, transform.rotation)as GameObject;
		obj.GetComponent<ExplosionStage>().Set(number);
		obj = Instantiate(explosion2,transform.position,transform.rotation)as GameObject;
		obj.GetComponent<ExplosionObject>().Set(number);

		Destroy(obj,1.0f);
		setActive = false;
		setExplosion = false;
		gameObject.SetActive(false);
	}

}
