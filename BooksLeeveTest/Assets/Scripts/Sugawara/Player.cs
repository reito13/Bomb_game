using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField] private int number = 1;

	private Transform myTransform;
	[SerializeField] private Transform rotateTransform = null;
	[SerializeField] private Rigidbody myRb = null;
	private Vector3 moveDir;

	private Vector3 startPosition;

	[SerializeField] private GroundCheck groundScript = null;
	[SerializeField] private CameraController cameraScript = null;

	[SerializeField] private float speed = 10.0f;
	[SerializeField] private float jumpForce = 10.0f;
	[SerializeField] private float bombPower = 1.0f;

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
	private bool damaged = false;
	[SerializeField] private int bombCount = 3; //爆弾の同時使用可能個数
	public int BombCount
	{
		set { bombCount += value;
			MainManager.Instance.BombUpdate(bombCount);

		}
		get { return bombCount; }
	}

	[SerializeField] private GameObject bombPrefab = null;
	[SerializeField] private Transform bombPos = null;

	public bool grounded = false;
	public bool jumped = false;

	private void Awake()
	{
		myTransform = GetComponent<Transform>();
	}

	private void Start()
	{
		moveDir = Vector3.zero;
		startPosition = myTransform.position;
	}

	private void FixedUpdate()
	{
		if (GameStatusManager.Instance.GameEnd)
			return;

		Move();
		Rotate();
		GroundCheck();
	}

	private void Move()
	{
		myTransform.Translate(moveDir * speed);

		if(myTransform.position.y < -10.0f)
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

	public void Jump()
	{
		if (grounded || !jumped)
		{
			if (!grounded)
				jumped = true;
			myRb.velocity = new Vector3(myRb.velocity.x,0,myRb.velocity.z);
			myRb.AddForce(Vector3.up * jumpForce);

			SoundManager.Instance.PlaySE("Jump");
		}
	}

	public void Bomb(float time)
	{
		if (GameStatusManager.Instance.GameStart)
			return;

		if (bombCount > 0)
		{
			SoundManager.Instance.PlaySE("BombThrow");

			Quaternion bombRo = myTransform.rotation;
			bombRo.eulerAngles = rotateTransform.eulerAngles;
			GameObject bomb = Instantiate(bombPrefab, bombPos.position, bombRo) as GameObject;
			bomb.GetComponent<Bomb>().Set(number,3.0f - time,this);
			BombCount = -1;
		}
	}

	public void SetMoveDir(float x,float z)
	{
		moveDir.x = x;
		moveDir.z = z;
	}

	public void Damage(Transform collT)
	{
		damaged = true;
		control = false;
		Vector3 dir = transform.position - collT.position;
		Debug.Log(dir);
		dir.x = (((dir.x >= 0) ? 3 : -3) - dir.x) * 1;
		dir.y = 1.3f;
		dir.z = (((dir.z >= 0) ? 3 : -3) - dir.z) * 1;
		Debug.Log(dir);
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

	private void ControlOn() {
		control = true;
	}

	private void Fall()
	{
		myRb.velocity = Vector3.zero;
		StartCoroutine(MainManager.Instance.TakeScore(number, true, 0.3f));
		myTransform.position = startPosition;

		SoundManager.Instance.PlaySE("ScoreDown");
	}

}
