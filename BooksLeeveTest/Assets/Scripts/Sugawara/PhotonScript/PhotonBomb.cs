using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonBomb : Photon.MonoBehaviour {

	public GameObject explosion = null;
	public GameObject explosion2 = null;

	public Rigidbody rb = null;
	public bool setActive = false;

	public bool photonMode = true;

	public new PhotonView photonView;
	public PhotonTransformView photonTransformView;

	private bool set = false;

	protected virtual void Awake()
	{
		if (!photonMode)
		{
			gameObject.SetActive(false);
			return;
		}

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

	public virtual IEnumerator Set(Vector3 pos, Quaternion ro, Vector3 force)
	{
		setActive = true;
		gameObject.SetActive(true);
		gameObject.layer = 8;

		transform.position = pos;
		transform.rotation = ro;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.AddForce(force, ForceMode.Impulse);

		yield return new WaitForSeconds(0.3f);
		GetHit();

		if (photonMode)
		{
			if (!photonView.isMine)
				yield break;

		}
		yield return new WaitForSeconds(2.7f);
		if (photonMode)
		{
			photonView.RPC("Explosion", PhotonTargets.All);
		}
		else
		{
			Explosion2();
		}
	}

	public virtual void Set(Vector3 pos, Quaternion ro, float time, float p)
	{

		setActive = true;
		gameObject.SetActive(true);
		transform.position = pos;
		transform.rotation = ro;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		rb.AddForce(transform.forward * p);

		rb.AddForce(transform.up * 0.5f * p);
		//rb.angularVelocity = rotatePower * transform.right;
		Invoke("GetHit", 0.3f);
	}

	public void GetHit()
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

	public void Explosion2()
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
		if (!photonMode)
			return;

		if (photonView.isMine)
		{
			//現在の移動速度
			Vector3 velocity = rb.velocity;
			//移動速度を指定
			photonTransformView.SetSynchronizedValues(speed: velocity, turnSpeed: 0);
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