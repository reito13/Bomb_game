using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonBomb : Photon.MonoBehaviour {

	[SerializeField] float power = 10.0f;
	[SerializeField] float rotatePower = 10.0f;

	[SerializeField] GameObject explosion = null;
	[SerializeField] GameObject explosion2 = null;

	[SerializeField] public PhotonPlayerController playerScript = null;

	[SerializeField] Rigidbody rb = null;

	public bool setActive = false;
	public bool setExplosion = false;

	private new PhotonView photonView;

	private float timer = 0.0f;

	private void Awake()
	{
		photonView = PhotonView.Get(this);
		if (!photonView.isMine)
		{
			rb.isKinematic = true;
			return;
		}
		//transform.parent = GameObject.Find("Bombs").transform;
	}

	public void Set(Vector3 pos, Quaternion ro, float time, float p)
	{
		power = p;
		timer = time;
		photonView = PhotonView.Get(this);
		setActive = true;
		gameObject.SetActive(true);
		transform.position = pos;
		transform.rotation = ro;
		//power = power * (1 + transform.localRotation.x);
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.AddForce(transform.up * 0.5f * power);
		rb.AddForce(transform.forward * power);
		//rb.angularVelocity = rotatePower * transform.right;
		Invoke("GetHit", 0.3f);
		//Invoke("ExplosionSet", time);
	}
	public void Set(Vector3 pos, Quaternion ro, float time)
	{
		timer = time;
		photonView = PhotonView.Get(this);
		setActive = true;
		gameObject.SetActive(true);
		transform.position = pos;
		transform.rotation = ro;
		//power = power * (1 + transform.localRotation.x);
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.AddForce(transform.up * 0.5f * power);
		rb.AddForce(transform.forward * power);
		//rb.angularVelocity = rotatePower * transform.right;
		Invoke("GetHit", 0.3f);
		//Invoke("ExplosionSet", time);
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
	//	photonView.RPC("Explosion",PhotonTargets.All);
	}

	[PunRPC]
	public void Explosion()
	{
		setActive = false;
		SoundManager.Instance.PlaySE("Explosion");

		GameObject obj = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
		obj = Instantiate(explosion2, transform.position, transform.rotation) as GameObject;

		Destroy(obj, 1.0f);
		//setExplosion = false;
		gameObject.SetActive(false);
	}

	private void Update()
	{
		timer -= Time.deltaTime;
		if (timer <= 0)
		{
			if (photonView.isMine)
			{
				photonView.RPC("Explosion", PhotonTargets.All);
			}
		}
	}

}