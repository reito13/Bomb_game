﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	//---------------------------------------------------------------------------------------------------------
	[Range(1,4)] public int number = 1; //プレイヤー番号　ネットワーク対戦においてプレイヤーの識別に使用

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
	//---------------------------------------------------------------------------------------------------------

	private Transform myTransform; //Transformのキャッシュ
	[SerializeField] private Transform rotateTransform = null; //X軸回転をさせるオブジェクトのTransform
	[SerializeField] private Transform bombPos = null; //ボムの生成位置
	[SerializeField] private Rigidbody myRb = null; //RigidBodyのキャッシュ
	[SerializeField] private GameObject cameraObj = null; //MainCameraオブジェクト
	private Vector3 moveDir;
	private Vector3 startPosition;
	private GroundCheck groundScript = null;
	private CameraController cameraScript = null;
	[SerializeField] private GameObject[] bombPrefabs = new GameObject[10];
	private Bomb[] bombScripts = new Bomb[10];

	private bool damaged = false;
	public bool grounded = false;
	public bool jumped = false;

	[SerializeField] private Animator animator = null;

	public enum AnimStats
	{
		WAIT,RUN,LANDING,JUMP,THROW,DAMAGE,
	}

	private void Awake()
	{
		myTransform = GetComponent<Transform>();
		groundScript = GetComponent<GroundCheck>();
		cameraScript = GetComponent<CameraController>();

		for (int i = 0;i<3;i++)
		{
			bombScripts[i] = bombPrefabs[i].GetComponent<Bomb>();
		}

		if (number != MainManager.playerNum)
		{
			cameraObj.SetActive(false);
		}
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
			myRb.velocity = new Vector3(myRb.velocity.x,0,myRb.velocity.z);
			myRb.AddForce(Vector3.up * jumpForce);

			AnimationChange(AnimStats.JUMP);
			SoundManager.Instance.PlaySE("Jump");

		}
	}

	public void Bomb(float time)
	{
		if (GameStatusManager.Instance.GameStart)
			return;

		for (int i = 0; i < bombPrefabs.Length; i++)
		{
			if (!bombPrefabs[i].activeSelf)
			{
				Quaternion bombRo = myTransform.rotation;
				bombRo.eulerAngles = rotateTransform.eulerAngles;

				bombPrefabs[i].SetActive(true);
				bombScripts[i].Set(bombPos.position, bombRo, 3.0f - time);
				SoundManager.Instance.PlaySE("BombThrow");

				return;
			}
		}
	}

	public void SetMoveDir(float x,float z)
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
	}

	private void AnimationChange(AnimStats animation)
	{
		if(animation == AnimStats.WAIT || animation == AnimStats.RUN)
		{
			animator.Play(animation.ToString());
		}
		else
		{
			animator.Play(animation.ToString(),0,0.0f);
		}
	}

}
