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
	public PhotonTransformView photonTransformView;

	public float timer = 0.0f;

	private bool set = false;

	private float count = 0;

	protected virtual void Awake()
	{
		if (!set)
		{
			photonView = PhotonView.Get(this);
			photonTransformView = GetComponent<PhotonTransformView>();
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
		count = 0;
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
		count += Time.deltaTime;

		timer -= Time.deltaTime;

		if (photonView.isMine)
		{
			//現在の移動速度
			Vector3 velocity = rb.velocity;
			//移動速度を指定
			photonTransformView.SetSynchronizedValues(speed: velocity, turnSpeed: 0);
		}

		if (timer <= 0)
		{
			if (photonView.isMine)
			{
				photonView.RPC("Explosion", PhotonTargets.All);
			}
		}
	}

	/*private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Block")
		{
			Debug.Log(count);
		}
	}*/

}