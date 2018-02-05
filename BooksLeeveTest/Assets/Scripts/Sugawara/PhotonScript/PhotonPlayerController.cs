using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PhotonPlayerController : Photon.MonoBehaviour {

	//-------------------------------------------------------------
	[Range(1, 4)] public int number = 1;

	[SerializeField] private float speed = 10.0f;
	[SerializeField] private float jumpForce = 10.0f;
	[SerializeField] private float bombPower = 1.0f;
	[SerializeField] private int jumpCount = 2;
	//-------------------------------------------------------------
	[SerializeField] private bool control = true;
	public bool Control
	{
		set
		{
			control = value;
		}

		get
		{
			return control;
		}
	}

	public bool grounded = false;
	private bool damaged = false;
	private bool jumped = false; //ジャンプ直後に接地判定をしないようにするため
	private bool throwed = false; //爆弾を投げるアニメーションの直後に歩行アニメーションを挟まないようにするため

	public bool[] bombActiveFlag;
	//-------------------------------------------------------------
	private Transform myTransform;
	[SerializeField] private Transform rotateTransform = null;
	[SerializeField] private Rigidbody myRb = null;
	[SerializeField] private GameObject cameraObj = null;
	private GroundCheck groundScript = null;
	private CameraController cameraScript = null;
	private new PhotonView photonView;
	private PhotonTransformView photonTransformView;
	private TimeManager timeManager;
	[SerializeField] private Transform bombPos = null;
	[SerializeField] private Animator animator = null;
	private BombLanding bombLanding;
	//-------------------------------------------------------------
	public Vector3 moveDir;

	private Vector3 startPosition;
	private int time = 60;

	public GameObject[] bombPrefabs = new GameObject[10];
	[System.NonSerialized] public PhotonBomb[] bombScripts = new PhotonBomb[10];

	public string playerName;

	public enum AnimStats
	{
		WAIT, RUN, LANDING, JUMP, THROW, DAMAGE,
	}

	public AnimStats animStats = AnimStats.WAIT;

	int i,j,arrayLength;

	private void Awake()
	{

		myTransform = GetComponent<Transform>();
		groundScript = GetComponent<GroundCheck>();
		cameraScript = GetComponent<CameraController>();
		bombLanding = GetComponent<BombLanding>();

		photonTransformView = GetComponent<PhotonTransformView>();
		photonView = PhotonView.Get(this);

		timeManager = GameObject.Find("GameManager").GetComponent<TimeManager>();

		arrayLength = bombPrefabs.Length;
		Array.Resize(ref bombActiveFlag, arrayLength);
		for (i = 0; i < arrayLength; i++)
		{
			bombScripts[i] = bombPrefabs[i].GetComponent<PhotonBomb>();
			bombActiveFlag[i] = false;
		}

		if (!photonView.isMine)
		{
			GetComponent<Rigidbody>().isKinematic = true;
			bombLanding.bombLandTransform.gameObject.SetActive(false);
			return;
		}
	}

	private void Start()
	{
		
		moveDir = Vector3.zero;
		startPosition = myTransform.position;
	}

	private void FixedUpdate()
	{
		BombCheck();

		if (!photonView.isMine)
		{
			cameraObj.SetActive(false);
			return;
		}

		if (GameStatusManager.Instance.GameEnd)
			return;

		Move();
		Rotate();
		GroundCheck();

	}

	private void BombCheck()
	{
		if (photonView.isMine){
			for (i = 0; i < arrayLength; i++)
			{
				bombActiveFlag[i] = bombScripts[i].setActive;
			}
		}
		else
		{
			for (i = 0; i < arrayLength; i++)
			{
				bombPrefabs[i].SetActive(bombActiveFlag[i]);
			}
		}
	}

	private void Move()
	{
		myTransform.Translate(moveDir * speed);

		if (grounded && !damaged && !throwed)
		{
			if (moveDir.x != 0 || moveDir.z != 0)
			{
				AnimationChange(AnimStats.RUN);
				//SoundManager.Instance.PlaySE("");
			}
			else
			{
				AnimationChange(AnimStats.WAIT);
			}
		}

		if (myTransform.position.y < -10.0f)
		{
			Fall();
			Damage();
		}

	}

	private void Rotate()
	{
		cameraScript.RotatitonUpdate();
	}

	private void GroundCheck()
	{
		if (jumped)
			return;

		grounded = groundScript.Grounded();
		if (grounded)
		{
			jumpCount = 2;
		}
	}
	public void Jump()
	{
		if (!photonView.isMine)
			return;

		if (jumpCount > 0)
		{
			grounded = false;
			jumped = true;
			jumpCount--;
			Invoke("JumpedOn",0.2f);
			myRb.velocity = new Vector3(myRb.velocity.x, 0, myRb.velocity.z);
			myRb.AddForce(Vector3.up * jumpForce);

			AnimationChange(AnimStats.JUMP);
			SoundManager.Instance.PlaySE("Jump");
		}
	}

	public IEnumerator Bomb(float time)
	{
		if (!photonView.isMine)
			yield break;

		if (GameStatusManager.Instance.GameStart)
			yield break;

		for (j = 0; j < arrayLength; j++)
		{
			if (!bombPrefabs[j].activeSelf)
			{
				throwed = true;
				//Invoke("ThrowedOn",0.6f);
				AnimationChange(AnimStats.THROW);
				Quaternion bombRo = myTransform.rotation;
				bombRo.eulerAngles = rotateTransform.eulerAngles;
				Debug.Log(bombLanding.GetPower(myTransform));
				yield return new WaitForSeconds(0.4f);

				bombPrefabs[j].SetActive(true);
				//bombScripts[j].Set(bombPos.position, myTransform.rotation, 3.0f - time);
				bombScripts[j].Set(bombPos.position, myTransform.rotation, 3.0f - time,bombLanding.GetPower(myTransform));
				yield return new WaitForSeconds(0.2f);
				throwed = false;
				SoundManager.Instance.PlaySE("BombThrow");

				yield break;
			}
		}
	}

	public void SetMoveDir(float x, float z)
	{
		moveDir.x = x;
		moveDir.z = z;
	}

	public void Damage(Transform collT)
	{
		if (damaged)
			return;
		damaged = true;
		control = false;
		Vector3 dir = transform.position - collT.position;
		dir.x = (((dir.x >= 0) ? 3 : -3) - dir.x) * 1;
		dir.y = 1.3f;
		dir.z = (((dir.z >= 0) ? 3 : -3) - dir.z) * 1;
		myRb.AddForce(dir * bombPower, ForceMode.Impulse);
		AnimationChange(AnimStats.DAMAGE);
		Invoke("Damaged", 2.0f);
		Invoke("ControlOn", 0.7f);
	}

	public void Damage()
	{
		if (damaged)
			return;
		damaged = true;
		control = false;
		
		AnimationChange(AnimStats.DAMAGE);
		Invoke("Damaged", 2.0f);
		Invoke("ControlOn", 0.7f);
	}


	private void Damaged()
	{
		damaged = false;
	}

	private void ControlOn()
	{
		control = true;
	}
	
	private void JumpedOn()
	{
		jumped = false;
	}

	private void ThrowedOn()
	{
		throwed = false;
	}

	private void Fall()
	{
		myRb.velocity = Vector3.zero;
		myTransform.position = startPosition;
	}

	private void AnimationChange(AnimStats animation)
	{
		if(animator.GetCurrentAnimatorStateInfo(0).IsName(animation.ToString()) && !photonView.isMine) //操作キャラでなく、アニメーションに変化がないときはリターン
		{
			return;
		}

		if (animation == AnimStats.WAIT || animation == AnimStats.RUN)
		{
			animator.Play(animation.ToString());
			animStats = animation;
		}
		else
		{
			animator.Play(animation.ToString(), 0, 0.0f);
			animStats = animation;
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(bombActiveFlag);

			stream.SendNext(animStats);

			stream.SendNext(MainManager.Instance.GetTime());

		}
		else
		{
			//データの受信
			bombActiveFlag = (bool[])stream.ReceiveNext();

			animStats = (AnimStats)stream.ReceiveNext();

			time = (int)stream.ReceiveNext();

		}
		/*if (PhotonNetwork.player.ID != 1 && !photonView.isMine)
		{
			MainManager.Instance.TimeUpdate(time);
		}*/

		if(!photonView.isMine)
			AnimationChange(animStats);
	}

}
