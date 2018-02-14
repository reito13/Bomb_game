using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayer : MonoBehaviour {

	//-------------------------------------------------------------
	[SerializeField] private float speed = 10.0f;
	[SerializeField] private float jumpForce = 10.0f;
	[SerializeField] private float bombPower = 1.0f;
	[SerializeField] private int jumpCount = 2;
	public AnimStats animStats = AnimStats.WAIT;
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
	private bool fall = false;

	//-------------------------------------------------------------
	[System.SerializableAttribute]
	public class BombListClass
	{
		public List<GameObject> list = new List<GameObject>();

	}
	[SerializeField]
	private List<BombListClass> bombListClass = new List<BombListClass>();
	//-------------------------------------------------------------
	private Transform myTransform;
	[SerializeField] private Transform rotateTransform = null;
	[SerializeField] private Rigidbody myRb = null;
	[SerializeField] private GameObject cameraObj = null;
	private GroundCheck groundScript = null;
	private CameraController cameraScript = null;
	private ThrowCalculation throwScript;
	[SerializeField] private BombLine bombLine = null;
	[SerializeField] private Transform bombPos = null;
	[SerializeField] private Animator animator = null;
	private BombLanding bombLanding;
	[SerializeField] private GameObject[] itemCarrots = null;
	private int saveCattotNum;
	//-------------------------------------------------------------
	public Vector3 moveDir;

	private Vector3 startPosition;
	private int time = 60;

	public string playerName;

	public enum AnimStats
	{
		WAIT, RUN, LANDING, JUMP, THROW, DAMAGE, PICKUP, RIGHTRUN, LEFTRUN,
	}
	public enum BombType
	{
		NONE, NORMAL, METAL, MINE, SMOKE, ROCKET, THREEWAY
	}

	int i, j, k;

	private void Awake()
	{
		myTransform = GetComponent<Transform>();
		groundScript = GetComponent<GroundCheck>();
		cameraScript = GetComponent<CameraController>();
		bombLanding = GetComponent<BombLanding>();
		throwScript = GetComponent<ThrowCalculation>();

		moveDir = Vector3.zero;
		startPosition = myTransform.position;
	}

	private void FixedUpdate()
	{
		//if (GameStatusManager.Instance.GameEnd)
			//return;

		Move();
		Rotate();
		GroundCheck();
		bombLine.ValueSet(bombPos.position, throwScript.GetForce());
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
				else */
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

		if (!grounded && groundScript.Grounded() && !jumped)
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
		if (jumpCount > 0)
		{
			grounded = false;
			jumped = true;
			jumpCount--;
			Invoke("JumpedOn", 0.3f);
			myRb.velocity = new Vector3(myRb.velocity.x, 0, myRb.velocity.z);
			myRb.AddForce(Vector3.up * jumpForce);

			AnimationChange(AnimStats.JUMP);
			SoundManager.Instance.PlaySE("Jump");
		}
	}

	public IEnumerator ThrowBomb()
	{
		//if (GameStatusManager.Instance.GameStart)
		//	yield break;

		if (bombType == BombType.NONE)
			yield break;

		int bombNumber = (int)bombType - 1;

		for (j = 0; j < bombListClass[bombNumber].list.Count; j++)
		{
			if (!bombListClass[bombNumber].list[j].activeSelf)
			{
				//----------------------------------------------------------
				throwed = true;
				AnimationChange(AnimStats.THROW);
				Quaternion bombRo = myTransform.rotation;
				bombRo.eulerAngles = rotateTransform.eulerAngles;
				bombType = BombType.NONE;

				itemCarrots[saveCattotNum].SetActive(false);

				yield return new WaitForSeconds(0.35f);
				//----------------------------------------------------------
				bombListClass[bombNumber].list[j].SetActive(true);
				switch (bombNumber)
				{
					case 4: //Rocket
						Debug.Log("Rocket");
						Vector3 pos = bombPos.position;
						StartCoroutine(bombListClass[bombNumber].list[j].GetComponent<Rocket>().Set(pos, myTransform.rotation, bombLanding.GetDistance()));
						break;

					case 5: //3way
						break;

					default:
						StartCoroutine(bombListClass[bombNumber].list[j].GetComponent<PhotonBomb>().Set(bombPos.position, myTransform.rotation, throwScript.GetForce()));
						break;
				}
				//----------------------------------------------------------
				SoundManager.Instance.PlaySE("BombThrow");
				yield return new WaitForSeconds(0.2f);
				throwed = false;
				animStats = AnimStats.WAIT;

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

	private void Fall()
	{
		/*myRb.velocity = Vector3.zero;
		Damage();*/
		//GameStatusManager.Instance.GameEnd = true;
	//	fall = true;
		//control = false;
		myTransform.position = startPosition;

	}

	[PunRPC]
	private void GameOverFlagSet(int num)
	{
		MainManager.Instance.playerGameOver[num] = true;
	}

	private void AnimationChange(AnimStats animation)
	{
	/*	if (animator.GetCurrentAnimatorStateInfo(0).IsName(animation.ToString())) //操作キャラでなく、アニメーションに変化がないときはリターン
		{
			return;
		}*/

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
			animator.CrossFade(animation.ToString(), 0.5f, 0, 0.0f);
			animStats = animation;
		}
	}

	public void SetBombType(BombType type)
	{
		bombType = type;
	}

	public IEnumerator SetBombType(int num)
	{
		if (bombType != BombType.NONE)
			yield break;
		bombType = (BombType)num;
		saveCattotNum = num - 1;
		itemCarrots[saveCattotNum].SetActive(true);
		picked = true;
		AnimationChange(AnimStats.PICKUP);

		yield return new WaitForSeconds(0.5f);

		picked = false;
		animStats = AnimStats.WAIT;
	}

}
