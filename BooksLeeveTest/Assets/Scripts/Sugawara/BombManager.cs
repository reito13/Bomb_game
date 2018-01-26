using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

public class BombManager : BaseAsyncLoop {

	public Transform[] bombs = new Transform[6];
	private Bomb[] bombScripts = new Bomb[6];

	[SerializeField] float lerpRate = 15; //2ベクトル間を補間する

	private int i,j;
	
	private int myPlayerNum;
	private int otherPlayerNum;

	private string setKey; //データをセットするときのキー
	private string getKey; //データをゲットするときのキー
	private string str; //Redisから読み込んだ分割前のプレイヤー情報
	private float[] splitStr; //プレイヤー情報を分割しfloatにしたもの 要素0がX,1がY,2がZの座標,3がYの角度

	private string flagSetKey;
	private string flagGetKey;
	private string flagStr;
	private bool[] splitFlagStr;

	private Vector3[] syncPos = new Vector3[3];//サーバーからGetした全プレイヤーの座標を格納する変数。座標同期の補間に使用する
	private Vector3[] syncRo = new Vector3[3];//サーバーからGetした全プレイヤーの角度を格納する変数。角度同期の補間に使用する

	protected override void Awake()
	{
		for (i = 0; i < 6; i++)
		{
			bombScripts[i] = bombs[i].GetComponent<Bomb>();
		}

		myPlayerNum = MainManager.playerNum;
		myPlayerNum--;
		otherPlayerNum = (myPlayerNum == 0) ? 3 : 0;

		setKey = "Bomb," + myPlayerNum;
		getKey = "Bomb," + ((myPlayerNum == 0) ? 1 : 0);

		flagSetKey = "BombFlag," + myPlayerNum;
		flagGetKey = "BombFlag," + ((myPlayerNum == 0) ? 1 : 0);

		myPlayerNum *= 3;
		Task task = TransformCoroutine();
		Task flagTask = FlagCoroutine();
	}

	private void FixedUpdate()
	{
		for (int i = otherPlayerNum; i < otherPlayerNum + 3; i++)
		{
			Split();
			LerpPosition(i);
			LerpRotation(i);
		}
	}

	private async Task FlagCoroutine()
	{

		while (true)
		{
			if (RedisSingleton.Instance.connect)
			{
				//CountStart();
				await FlagSet();
				await FlagGet();
				//CountEnd();
			}
			await Task.Delay(100);
		}
	}

	protected override async Task Set(int count)
	{
		await RedisSingleton.Instance.RedisSet(setKey,bombs[myPlayerNum].position.x.ToString("f2") + "," + bombs[myPlayerNum].position.y.ToString("f2") + "," + bombs[myPlayerNum].position.z.ToString("f2") + "," +
			bombs[myPlayerNum].eulerAngles.x.ToString("f2") + "," + bombs[myPlayerNum].eulerAngles.y.ToString("f2") + "," + bombs[myPlayerNum].eulerAngles.z.ToString("f2") + "," +
			bombs[myPlayerNum+1].position.x.ToString("f2") + "," + bombs[myPlayerNum+1].position.y.ToString("f2") + "," + bombs[myPlayerNum+1].position.z.ToString("f2") + "," +
			bombs[myPlayerNum+1].eulerAngles.x.ToString("f2") + "," + bombs[myPlayerNum+1].eulerAngles.y.ToString("f2") + "," + bombs[myPlayerNum+1].eulerAngles.z.ToString("f2") + "," +
			bombs[myPlayerNum+2].position.x.ToString("f2") + "," + bombs[myPlayerNum+2].position.y.ToString("f2") + "," + bombs[myPlayerNum+2].position.z.ToString("f2") + "," +
			bombs[myPlayerNum+2].eulerAngles.x.ToString("f2") + "," + bombs[myPlayerNum+2].eulerAngles.y.ToString("f2") + "," + bombs[myPlayerNum+2].eulerAngles.z.ToString("f2"));
	}

	protected override async Task Get(int count)
	{ 
		str = await RedisSingleton.Instance.RedisGet(getKey);
	}

	private async Task FlagSet()
	{
		await RedisSingleton.Instance.RedisSet(flagSetKey, bombScripts[myPlayerNum].setActive.ToString() + "," + bombScripts[myPlayerNum].setExplosion.ToString()
			+ "," + bombScripts[myPlayerNum + 1].setActive.ToString() + "," + bombScripts[myPlayerNum + 1].setExplosion.ToString()
			+ "," + bombScripts[myPlayerNum + 2].setActive.ToString() + "," + bombScripts[myPlayerNum + 2].setExplosion.ToString());
	}

	private async Task FlagGet()
	{
		if (myPlayerNum == 0)
		{
			flagStr = await RedisSingleton.Instance.RedisGet(flagSetKey) + "," + await RedisSingleton.Instance.RedisGet(flagGetKey);
		}
		else
		{
			flagStr = await RedisSingleton.Instance.RedisGet(flagGetKey) + "," + await RedisSingleton.Instance.RedisGet(flagSetKey);
		}

		FlagCheck();

	}

	private void Split()
	{
		if (str == null)
			return;

		splitStr = str.Split(',').Select(s => float.Parse(s)).ToArray();

		syncPos[0] = new Vector3(splitStr[0], splitStr[1], splitStr[2]);
		syncRo[0] = new Vector3(splitStr[3], splitStr[4], splitStr[5]);
		syncPos[1] = new Vector3(splitStr[6], splitStr[7], splitStr[8]);
		syncRo[1] = new Vector3(splitStr[9], splitStr[10], splitStr[11]);
		syncPos[2] = new Vector3(splitStr[12], splitStr[13], splitStr[14]);
		syncRo[2] = new Vector3(splitStr[15], splitStr[16], splitStr[17]);

	}

	private async Task GetExplosion(int num)
	{
		string setExplosion = await RedisSingleton.Instance.RedisGet("Bomb,SetExplosion," + num.ToString());
		if (setExplosion == "true")
		{
			bombScripts[num].Explosion();
		}
	}

	private void LerpPosition(int num) //プレイヤーの移動を補間する処理。TransformGet()のfor文から呼び出され、numにはfor文のiが入る
	{
		bombs[num].position = Vector3.Lerp(bombs[num].position, syncPos[num - otherPlayerNum], Time.deltaTime * lerpRate);
	}

	private void LerpRotation(int num)
	{
		Vector3 ro = bombs[num].eulerAngles;
		ro.x = Mathf.Lerp(ro.x, syncRo[num - otherPlayerNum].x, Time.deltaTime * lerpRate);
		ro.y = Mathf.Lerp(ro.y, syncRo[num - otherPlayerNum].y, Time.deltaTime * lerpRate);
		ro.z = Mathf.Lerp(ro.z, syncRo[num - otherPlayerNum].z, Time.deltaTime * lerpRate);

		//ro = syncRo[num - otherPlayerNum];

		//Mathf.Lerp(ro.y, syncRotateY, Time.deltaTime * lerpRate);
		//bombs[num].rotation = Quaternion.Euler(ro);
		bombs[num].eulerAngles = ro;
	}

	private void FlagCheck()
	{
		if (flagStr == null)
			return;

		splitFlagStr = flagStr.Split(',').Select(b => bool.Parse(b)).ToArray();
		Debug.Log(flagStr);
		for(j = 0; j < 6; j++)
		{
			bombs[j].gameObject.SetActive(splitFlagStr[j * 2]);
			if(splitFlagStr[j*2 + 1] && bombs[j].gameObject.activeSelf)
			{
				bombScripts[j].Explosion();
			}
		}
	}
}
