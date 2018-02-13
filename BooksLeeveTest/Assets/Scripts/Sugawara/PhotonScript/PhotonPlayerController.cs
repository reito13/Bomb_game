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
	public AnimStats animStats = AnimStats.WAIT;
	public bool setBomb = false;
	public BombType bombType = BombType.NONE;
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
	public bool damaged = false;
	public bool jumped = false; //ジャンプ直後に接地判定をしないようにするため
	public bool throwed = false; //爆弾を投げるアニメーションの直後に歩行アニメーションを挟まないようにするため
	public bool picked = false;

	public bool[] bombActiveFlag;
	public bool[] spBombActiveFlag;
	//-------------------------------------------------------------
	private Transform myTransform;
	[SerializeField] private Transform rotateTransform = null;
	[SerializeField] private Rigidbody myRb = null;
	[SerializeField] private GameObject cameraObj = null;
	[SerializeField] private GameObject bombLineObj = null;
	[SerializeField] private Canvas canvas = null;
	private GroundCheck groundScript = null;
	private CameraController cameraScript = null;
	private new PhotonView photonView;
	private PhotonTransformView photonTransformView;
	private TimeManager timeManager;
	private ThrowCalculation throwScript;
	[SerializeField] private BombLine bombLine = null;
	[SerializeField] private Transform bombPos = null;
	[SerializeField] private Animator animator = null;
	private BombLanding bombLanding;
	//-------------------------------------------------------------
	public Vector3 moveDir;

	private Vector3 startPosition;
	private int time = 60;

	//[SerializeField] private GameObject 
	public GameObject[] bombPrefabs = new GameObject[10];
	[System.NonSerialized] public PhotonBomb[] bombScripts = new PhotonBomb[10];
	public GameObject[] spBombPrefabs = new GameObject[9];
	[System.NonSerialized] public PhotonBomb[] spBombScripts = new PhotonBomb[9];

	public string playerName;

	public enum AnimStats
	{
		WAIT, RUN, LANDING, JUMP, THROW, DAMAGE, PICKUP, RIGHTRUN, LEFTRUN,
	}
	public enum BombType
	{
		NONE,NORMAL,MISSILE,ROCKET,METAL,MINE,SMOKE, DOT,THREEWAY
	}

	int i, j, bombArrayLength, spBombArrayLength;

	private void Awake()
	{ 
		myTransform = GetComponent<Transform>();
		groundScript = GetComponent<GroundCheck>();
		cameraScript = GetComponent<CameraController>();
		bombLanding = GetComponent<BombLanding>();
		throwScript = GetComponent<ThrowCalculation>();

		photonTransformView = GetComponent<PhotonTransformView>();
		photonView = PhotonView.Get(this);

		timeManager = GameObject.Find("GameManager").GetComponent<TimeManager>();

		bombArrayLength = bombPrefabs.Length;
		spBombArrayLength = spBombPrefabs.Length;
		Array.Resize(ref bombActiveFlag, bombArrayLength);
		Array.Resize(ref spBombActiveFlag, spBombArrayLength);

		for (i = 0; i < bombArrayLength; i++)
		{
			bombScripts[i] = bombPrefabs[i].GetComponent<PhotonBomb>();
			bombActiveFlag[i] = false;
		}

		for (i = 0; i < spBombArrayLength; i++)
		{
			spBombScripts[i] = spBombPrefabs[i].GetComponent<PhotonBomb>();
			spBombActiveFlag[i] = false;
		}

		if (!photonView.isMine)
		{
			GetComponent<Rigidbody>().isKinematic = true;
			bombLanding.targetTransform.gameObject.SetActive(false);
			bombLineObj.SetActive(false);
			return;
		}
		canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
		canvas.worldCamera = cameraObj.GetComponent<Camera>();
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
		bombLine.ValueSet(bombPos.position,throwScript.GetForce());
	}

	private void BombCheck()
	{
		if (photonView.isMine){
			for (i = 0; i < bombArrayLength; i++)
			{
				bombActiveFlag[i] = bombScripts[i].setActive;
			}
			for(i = 0; i < spBombArrayLength; i++)
			{
				spBombActiveFlag[i] = spBombScripts[i].setActive;
			}
		}
		else
		{
			for (i = 0; i < bombArrayLength; i++)
			{
				bombPrefabs[i].SetActive(bombActiveFlag[i]);
			}
			for (i = 0; i < spBombArrayLength; i++)
			{
				spBombPrefabs[i].SetActive(spBombActiveFlag[i]);
			}
		}
	}

	private void Move()
	{
		myTransform.Translate(moveDir * speed);

		if (grounded && !damaged && !throwed && !picked)
		{
		/*	if (moveDir.x > 0)
			{
				AnimationChange(AnimStats.RIGHTRUN);
			}
			else if (moveDir.x < 0)
			{
				AnimationChange(AnimStats.LEFTRUN);
			}
			else */if (moveDir.x != 0 || moveDir.z != 0)
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

		if(!grounded && groundScript.Grounded() && !jumped)
		{
			StartCoroutine(Landing());
		}
		grounded = groundScript.Grounded();

		if (grounded)
		{
			jumpCount = 2;
		}
	}

	private IEnumerator Landing()
	{
		grounded = true;
		AnimationChange(AnimStats.LANDING);
		yield return new WaitForSeconds(0.2f);
		animStats = AnimStats.WAIT;

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
			Invoke("JumpedOn",0.3f);
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

		if (!setBomb)
			yield break;

		for (j = 0; j < bombArrayLength; j++)
		{
			if (!bombPrefabs[j].activeSelf)
			{
				throwed = true;
				setBomb = false;
				//Invoke("ThrowedOn",0.6f);
				AnimationChange(AnimStats.THROW);
				Quaternion bombRo = myTransform.rotation;
				bombRo.eulerAngles = rotateTransform.eulerAngles;
				yield return new WaitForSeconds(0.4f);

				bombPrefabs[j].SetActive(true);
				StartCoroutine(bombScripts[j].Set(bombPos.position,myTransform.rotation, throwScript.GetForce()));
				//bombScripts[j].Set(bombPos.position, myTransform.rotation, 3.0f - time,bombLanding.GetPower());
				SoundManager.Instance.PlaySE("BombThrow");

				yield return new WaitForSeconds(0.4f);
				throwed = false;
				animStats = AnimStats.WAIT;

				yield break;
			}
		}
	}

	public IEnumerator SPBomb(float time)
	{
		if (!photonView.isMine)
			yield break;

		if (GameStatusManager.Instance.GameStart)
			yield break;

		if (bombType == BombType.NONE)
			yield break;

		int bombNumber = (int)bombType - 1;
		throwed = true;
		AnimationChange(AnimStats.THROW);
		Quaternion bombRo = myTransform.rotation;
		bombRo.eulerAngles = rotateTransform.eulerAngles;

		bombType = BombType.NONE;

		yield return new WaitForSeconds(0.4f);

		spBombPrefabs[bombNumber].SetActive(true);

		if (bombNumber == 1) //ロケット
		{
			Debug.Log("Ricket");
			Vector3 pos = bombPos.position;
			//pos.y = pos.y - 1.0f;
			Debug.Log(bombLanding.GetDistance());
			spBombScripts[bombNumber].Set(pos, myTransform.rotation, 3.0f - time, bombLanding.GetDistance()); 
		}

		else if (bombNumber == 2)
		{
			Debug.Log("Ricket");
			Vector3 pos = bombPos.position;
			pos.y = pos.y - 1.0f;
			Debug.Log(bombLanding.GetDistance());
			spBombScripts[bombNumber].Set(pos, myTransform.rotation, 3.0f - time, bombLanding.GetDistance());
		}

		else if (bombNumber == 7)
		{
			Debug.Log("3WAY");
			spBombPrefabs[bombNumber -1].SetActive(true);
			spBombPrefabs[bombNumber +1].SetActive(true);
			Vector3 bombPos1 = bombPos.position;
			bombPos1 -= transform.right * 0.2f;
			Vector3 bombPos2 = bombPos.position;
			bombPos2 += transform.right * 0.2f;
			//spBombScripts[bombNumber].Set(bombPos.position, myTransform.rotation, 3.0f - time, throwScript.GetForce());
			StartCoroutine(spBombScripts[bombNumber-1].Set(bombPos1, myTransform.rotation, throwScript.GetForce()));
			StartCoroutine(spBombScripts[bombNumber].Set(bombPos.position, myTransform.rotation, throwScript.GetForce()));
			StartCoroutine(spBombScripts[bombNumber +1].Set(bombPos2, myTransform.rotation, throwScript.GetForce()));
			//spBombScripts[bombNumber + 1].Set(bombPos1, myTransform.rotation, 3.0f - time, throwScript.GetForce());
			//spBombScripts[bombNumber + 2].Set(bombPos2, myTransform.rotation, 3.0f - time, throwScript.GetForce());
		}
		else
		{
			spBombScripts[bombNumber].Set(bombPos.position, myTransform.rotation, 3.0f - time, bombLanding.GetPower());
		}
		yield return new WaitForSeconds(0.2f);
		throwed = false;
		animStats = AnimStats.WAIT;
		SoundManager.Instance.PlaySE("BombThrow");
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
		myRb.velocity = Vector3.zero;
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

	private void PickUpOn()
	{
		picked = false;
		animStats = AnimStats.WAIT;

	}

	private void Fall()
	{
		/*myRb.velocity = Vector3.zero;
		myTransform.position = startPosition;
		Damage();*/
		GameStatusManager.Instance.GameEnd = true;
	}

	private void AnimationChange(AnimStats animation)
	{
		if(animator.GetCurrentAnimatorStateInfo(0).IsName(animation.ToString()) && !photonView.isMine) //操作キャラでなく、アニメーションに変化がないときはリターン
		{
			return;
		}

		if (animation == AnimStats.WAIT || animation == AnimStats.RUN || animation == AnimStats.RIGHTRUN || animation == AnimStats.LEFTRUN)
		{
			if (animStats == AnimStats.WAIT || animStats == AnimStats.RUN || animStats == AnimStats.JUMP || animStats == AnimStats.RIGHTRUN || animStats == AnimStats.LEFTRUN)
			{

				animator.Play(animation.ToString());
				//animator.CrossFade(animation.ToString(),0.1f);
				animStats = animation;
			}
		}
		else
		{
			//animator.Play(animation.ToString(), 0, 0.0f);
			animator.CrossFade(animation.ToString(), 0.5f,0,0.0f);
			animStats = animation;
		}
	}

	public void SetBombType(BombType type)
	{
		bombType= type;
	}

	public void SetBomb()
	{
		setBomb= true;
		AnimationChange(AnimStats.PICKUP);
		picked = true;
		Invoke("PickUpOn",0.5f);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(bombActiveFlag);
			stream.SendNext(spBombActiveFlag);

			stream.SendNext(animStats);

			stream.SendNext(MainManager.Instance.GetTime());

		}
		else
		{
			//データの受信
			bombActiveFlag = (bool[])stream.ReceiveNext();
			spBombActiveFlag = (bool[])stream.ReceiveNext();

			animStats = (AnimStats)stream.ReceiveNext();

			time = (int)stream.ReceiveNext();

		}

		if(!photonView.isMine)
			AnimationChange(animStats);
	}

}
