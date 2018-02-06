using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonBomb : Photon.MonoBehaviour {

	public float power = 10.0f;
	//[SerializeField] float rotatePower = 10.0f;

	public GameObject explosion = null;
	public GameObject explosion2 = null;

	public PhotonPlayerController playerScript = null;

	public Rigidbody rb = null;

	public bool setActive = false;

	public new PhotonView photonView;

	public float timer = 0.0f;

	private bool set = false;

	protected virtual void Awake()
	{
		if (!set)
		{
			photonView = PhotonView.Get(this);
			if (!photonView.isMine)
			{
				rb.isKinematic = true;
				return;
			}
			set = true;
			gameObject.SetActive(false);
		}
	}

	public virtual void Set(Vector3 pos, Quaternion ro, float time, float p)
	{
		power = p;
		timer = time;
		setActive = true;
		gameObject.SetActive(true);
		transform.position = pos;
		transform.rotation = ro;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.AddForce(transform.up * 0.5f * power);
		rb.AddForce(transform.forward * power);
		//rb.angularVelocity = rotatePower * transform.right;
		Invoke("GetHit", 0.3f);
	}
	public virtual void Set(Vector3 pos, Quaternion ro, float time)
	{
		timer = time;
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
	}

	private void GetHit()
	{
		gameObject.layer = 9;
	}

	[PunRPC]
	public void Explosion()
	{
		setActive = false;
		SoundManager.Instance.PlaySE("Explosion");

		GameObject obj = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
		obj = Instantiate(explosion2, transform.position, transform.rotation) as GameObject;

		Destroy(obj, 1.0f);
		gameObject.SetActive(false);
	}

	protected virtual void Update()
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