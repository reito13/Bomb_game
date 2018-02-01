using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayer : MonoBehaviour {

	private Transform myTransform;
	[SerializeField] private Transform rotateTransform = null;
	[SerializeField] private Rigidbody myRb = null;

	[SerializeField] private GameObject cameraObj = null;

	private Vector3 moveDir;

	private Vector3 startPosition;

	private GroundCheck groundScript = null;
	private CameraController cameraScript = null;

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

	[SerializeField] private GameObject bombPrefabs = null;

	[SerializeField] private Transform bombPos = null;

	public bool grounded = false;
	public bool jumped = false;

	[SerializeField] private Animator animator = null;

	private bool bombSet = false;
	private float bombTime = 0.0f;

	public enum AnimStats
	{
		WAIT, RUN, LANDING, JUMP, THROW, DAMAGE,
	}

	private void Awake()
	{
		myTransform = GetComponent<Transform>();
		groundScript = GetComponent<GroundCheck>();
		cameraScript = GetComponent<CameraController>();
	}

	private void Start()
	{
		moveDir = Vector3.zero;
		startPosition = myTransform.position;
	}

	private void FixedUpdate()
	{
		Move();
		Rotate();
		GroundCheck();
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

	public void Jump()
	{
		if (grounded || !jumped)
		{
			if (!grounded)
				jumped = true;
			myRb.velocity = new Vector3(myRb.velocity.x, 0, myRb.velocity.z);
			myRb.AddForce(Vector3.up * jumpForce);

			AnimationChange(AnimStats.JUMP);
			SoundManager.Instance.PlaySE("Jump");

		}
	}

	public void Bomb(float time)
	{
		SoundManager.Instance.PlaySE("BombThrow");

		Quaternion bombRo = myTransform.rotation;
		bombRo.eulerAngles = rotateTransform.eulerAngles;

		GameObject bomb = Instantiate(bombPrefabs)as GameObject;
		bomb.GetComponent<MenuBomb>().Set(bombPos.position, bombRo, 3.0f - time);
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

	private void Update()
	{
		if (!Control)
		{
			SetMoveDir(0, 0);
			return;
		}

		MoveInput();
		RotateInput();

		if ((Input.GetButtonDown("Jump")) || (Input.GetButtonDown("R2")))
		{
			Jump();
			//player.photonView.RPC("Jump",PhotonTargets.AllViaServer);
		}

		if ((Input.GetKeyDown(KeyCode.Z)) || (Input.GetButtonDown("R1") || Input.GetMouseButtonDown(0)))
		{
			bombSet = true;
		}
		if ((Input.GetKeyUp(KeyCode.Z)) || (Input.GetButtonUp("R1") || Input.GetMouseButtonUp(0)))
		{
			Bomb(bombTime);
			bombSet = false;
			bombTime = 0.0f;
		}

		if (bombSet)
			bombTime += Time.deltaTime;

	}

	private void MoveInput()
	{
		SetMoveDir(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
	}

	private void RotateInput()
	{
		cameraScript.SetRotate(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
	}

}
