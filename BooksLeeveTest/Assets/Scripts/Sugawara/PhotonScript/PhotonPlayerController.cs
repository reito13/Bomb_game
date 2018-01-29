using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonPlayerController : Photon.MonoBehaviour {

	[Range(1, 4)] public int number = 1;

	private Transform myTransform;
	[SerializeField] private Transform rotateTransform = null;
	private Rigidbody myRb;

	[SerializeField] private GameObject cameraObj = null;

	private Vector3 moveDir;

	private Vector3 startPosition;

	private GroundCheck groundScript = null;
	private CameraController cameraScript = null;

	[SerializeField] private float speed = 10.0f;
	[SerializeField] private float jumpForce = 10.0f;
	[SerializeField] private float bombPower = 1.0f;

	[SerializeField] private bool control = true;

	private PhotonView photonView;
	private PhotonTransformView photonTransformView;
	private Vector3 syncPos;
	private float syncRoY;

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
	private bool damaged = false;
	[SerializeField] private int bombCount = 3; //爆弾の同時使用可能個数
	public int BombCount
	{
		set
		{
			bombCount += value;
			if (bombCount > 3)
				bombCount = 3;
			MainManager.Instance.BombUpdate(bombCount);

		}
		get { return bombCount; }
	}

	[System.NonSerialized] public GameObject[] bombPrefabs = new GameObject[3];
	[System.NonSerialized] public PhotonBomb[] bombScripts = new PhotonBomb[3];
	[SerializeField] private Transform bombPos = null;

	public bool grounded = false;
	public bool jumped = false;

	[SerializeField] private Animator animator = null;

	public enum AnimStats
	{
		WAIT, RUN, LANDING, JUMP, THROW, DAMAGE,
	}

	private void Set()
	{
		photonTransformView = GetComponent<PhotonTransformView>();
		photonView = PhotonView.Get(this);

		if (!photonView.isMine)
		{
			GetComponent<Rigidbody>().isKinematic = true;
			return;
		}

		myRb = GetComponent<Rigidbody>();
		myTransform = GetComponent<Transform>();
		groundScript = GetComponent<GroundCheck>();
		cameraScript = GetComponent<CameraController>();
		for (int i = 0; i < 3; i++)
		{
			bombScripts[i] = bombPrefabs[i].GetComponent<PhotonBomb>();
		}

		if (number != MainManager.playerNum)
		{
			cameraObj.SetActive(false);
		}
	}

	private void Start()
	{
		if (!photonView.isMine)
			return;
		moveDir = Vector3.zero;
		startPosition = myTransform.position;
	}

	private void FixedUpdate()
	{
		if (photonView.isMine)
		{
			if (GameStatusManager.Instance.GameEnd)
				return;

			Move();
			Rotate();
			GroundCheck();
		}
	}

	private void Move()
	{
		myTransform.Translate(moveDir * speed);

		if (grounded)
		{
			if (moveDir.x != 0 || moveDir.z != 0)
			{
				AnimationChange(AnimStats.RUN);
			}
			else
			{
				AnimationChange(AnimStats.WAIT);
			}

		}

		if (myTransform.position.y < -10.0f)
		{
			Fall();
		}

	}

	private void Rotate()
	{
		cameraScript.RotatitonUpdate();
	}

	private void GroundCheck()
	{
		grounded = groundScript.Grounded();
		if (grounded)
		{
			jumped = false;
		}
	}
	[PunRPC]
	public void Jump()
	{
		if (photonView.isMine)
		{

			if (grounded || !jumped)
			{
				if (!grounded)
					jumped = true;
				myRb.velocity = new Vector3(myRb.velocity.x, 0, myRb.velocity.z);
				myRb.AddForce(Vector3.up * jumpForce);

				//AnimationChange(AnimStats.JUMP);
				SoundManager.Instance.PlaySE("Jump");

			}
		}
	}

	public void Bomb(float time)
	{
		if (!photonView.isMine)
			return;

		if (GameStatusManager.Instance.GameStart)
			return;

		if (bombCount > 0)
		{
			SoundManager.Instance.PlaySE("BombThrow");

			Quaternion bombRo = myTransform.rotation;
			bombRo.eulerAngles = rotateTransform.eulerAngles;

			bombPrefabs[bombCount - 1].SetActive(true);
			bombScripts[bombCount - 1].Set(bombPos.position, bombRo, 3.0f - time);
			BombCount = -1;
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
		Invoke("Damaged", 2.0f);
		Invoke("ControlOn", 0.7f);
		StartCoroutine(MainManager.Instance.TakeScore(number, false, 0.3f));

		SoundManager.Instance.PlaySE("ScoreDown");
	}


	private void Damaged()
	{
		damaged = false;
	}

	private void ControlOn()
	{
		control = true;
	}

	private void Fall()
	{
		myRb.velocity = Vector3.zero;
		StartCoroutine(MainManager.Instance.TakeScore(number, true, 0.3f));
		myTransform.position = startPosition;

		SoundManager.Instance.PlaySE("ScoreDown");
	}

	private void AnimationChange(AnimStats animation)
	{
		if (animation == AnimStats.WAIT || animation == AnimStats.RUN)
		{
			animator.Play(animation.ToString());
		}
		else
		{
			animator.Play(animation.ToString(), 0, 0.0f);
		}
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting)
		{
			stream.SendNext(myRb.velocity);
		}
		else
		{
			myRb.velocity = (Vector3)stream.ReceiveNext();
		}
	}

}
