using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

public class SyncBomb : BaseAsyncLoop
{
	[SerializeField] private JsonTest jsonScript = null;

	public Transform[] bombs = new Transform[6];
	private Bomb[] bombScripts = new Bomb[6];

	[SerializeField] float lerpRate = 15; //2ベクトル間を補間する

	private int i, j;

	private int myPlayerNum;
	private int otherPlayerNum;

	private string setKey; //データをセットするときのキー
	private string getKey; //データをゲットするときのキー

	private string jsonSet; //セットするJsonデータ
	private string jsonGet; //ゲットしたJsonデータ

	private Vector3[] syncPos = new Vector3[6];//サーバーからGetした全プレイヤーの座標を格納する変数。座標同期の補間に使用する
	private Vector3[] syncRo = new Vector3[6];//サーバーからGetした全プレイヤーの角度を格納する変数。角度同期の補間に使用する
	private bool[] syncActiveFlag = new bool[6];
	private bool[] syncExplosionFlag = new bool[6];

	private SyncBombData bombData;

	protected override void Awake()
	{
		for (i = 0; i < 6; i++)
		{
			bombScripts[i] = bombs[i].GetComponent<Bomb>();
		}

		bombData = new SyncBombData();
		for(int i = 0; i < 6; i++)
		{
			DataSet(i);
		}

		myPlayerNum = MainManager.playerNum;
		myPlayerNum--;
		otherPlayerNum = (myPlayerNum == 0) ? 3 : 0;

		setKey = "," + myPlayerNum + ",Bomb";
		getKey = "," + ((myPlayerNum == 0) ? 1 : 0) + ",Bomb";

		myPlayerNum *= 3;
		Task task = TransformCoroutine();
	}

	protected override async Task TransformCoroutine()
	{
		await Task.Delay(3000);
		Debug.Log(getKey);
		await redis.RedisSet(1 + setKey, jsonSet);
		await redis.RedisSet(1 + getKey, jsonSet);

		while (true)
		{
			//CountStart();
			count++;
			await redis.RedisSet(count + setKey, jsonSet);
			//await Task.Delay(100);
			jsonGet = await redis.RedisGet(count + setKey);
			//CountEnd();
			Debug.Log(jsonGet);
		}
	}

	private void FixedUpdate()
	{
		Json();		
		for (int i = otherPlayerNum; i < otherPlayerNum + 3; i++)
		{
			DataSet(i);
			LerpPosition(i);
			LerpRotation(i);
			FlagChack(i);
		}
	}

	private void DataSet(int num)
	{
		bombData.syncPos[num] = bombs[num].position;
		bombData.syncRo[num] = bombs[num].eulerAngles;
		bombData.activeFlag[num] = bombs[num].gameObject.activeSelf;
		bombData.explosionFlag[num] = bombScripts[num].setExplosion;
	}

	private void Json()
	{
		jsonSet = jsonScript.GetJson(bombData); //データクラスのインスタンスを渡し、それをJson形式にしたものをjsonSetに代入
		SyncBombData getBombData = jsonScript.GetBombData(jsonGet);
		if (jsonGet != null)
		{
			syncPos = getBombData.syncPos;
			syncRo = getBombData.syncRo;
			syncActiveFlag = getBombData.activeFlag;
			syncExplosionFlag = getBombData.explosionFlag;
		}
	}

	private void LerpPosition(int num) //プレイヤーの移動を補間する処理。TransformGet()のfor文から呼び出され、numにはfor文のiが入る
	{
		bombs[num].position = Vector3.Lerp(bombs[num].position, syncPos[num], Time.deltaTime * lerpRate);
	}

	private void LerpRotation(int num)
	{
		Vector3 ro = bombs[num].eulerAngles;
		ro.x = Mathf.Lerp(ro.x, syncRo[num].x, Time.deltaTime * lerpRate);
		ro.y = Mathf.Lerp(ro.y, syncRo[num].y, Time.deltaTime * lerpRate);
		ro.z = Mathf.Lerp(ro.z, syncRo[num].z, Time.deltaTime * lerpRate);

		bombs[num].eulerAngles = ro;
	}

	private void FlagChack(int num)
	{
		if (bombs[num].gameObject.activeSelf)
		{
			if (syncExplosionFlag[num])
			{
				bombScripts[num].Explosion();
				syncExplosionFlag[num] = false;
			}

			if (!syncActiveFlag[num])
			{
				bombs[num].gameObject.SetActive(false);
			}
		}
		else
		{
			if (syncActiveFlag[num])
			{
				bombs[num].gameObject.SetActive(true);
			}
		}
	}
}
