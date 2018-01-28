using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

public class SyncPlayer : BaseAsyncLoop {

    [SerializeField] JsonTest jsonScript = null; 

    private Transform playerTransform1; //1PのTransform
    private Transform playerTransform2; //2PのTransform

	[System.NonSerialized] public float lerpRate = 1; //2ベクトル間を補間する
	public Vector3 syncPos; //サーバーからGetした全プレイヤーの座標を格納する変数。座標移動の補間に使用する
	public float syncRotateY;

	private string setKey; //データをセットするときのキー
	private string getKey; //データをゲットするときのキー

	private string jsonSet; //セットするJsonデータ
	private string jsonGet; //ゲットしたJsonデータ

	private int myPlayerNum = 1; //自分が操作しているプレイヤーの番号 MainManagerのPlayerNumが入る

	AsyncPlayerData playerData;

	protected override void Awake()
	{

		playerTransform1 = GameObject.Find("1Player").transform;
		playerTransform2 = GameObject.Find("2Player").transform;

		myPlayerNum = MainManager.playerNum;

		playerData = new AsyncPlayerData();
		PlayerDataSet();

		setKey = "," + myPlayerNum + "PlayerTransform";
		getKey = "," + ((myPlayerNum == 1) ? 2 : 1) + "PlayerTransform";

		Debug.Log(setKey);
		Debug.Log(getKey);

		Task task = TransformCoroutine();
	}

	protected override async Task TransformCoroutine()
	{
		//await RedisSingleton.Instance.RedisSet(count + setKey, jsonSet);
		//await RedisSingleton.Instance.RedisSet(count + setKey, jsonSet);

		while (true)
		{
			CountStart();
			count++;
			Debug.Log(count);
			//await Set(count);
			//await RedisSingleton.Instance.RedisSet(count + setKey, jsonSet);
			Debug.Log("a");
			await Task.Delay(100);
			//jsonGet = await RedisSingleton.Instance.RedisGet(count + getKey);
			Debug.Log(jsonGet);
			//await Get(count);
			CountEnd();
		}
	}

	private void FixedUpdate()
	{
		PlayerDataSet();
		Json();
		if (myPlayerNum == 1)
		{
			LerpPosition(playerTransform2);
			LerpRotation(playerTransform2);
		}
		else
		{
			LerpPosition(playerTransform1);
			LerpRotation(playerTransform1);
		}
	}

	protected override async Task Set(int count)
	{
		await RedisSingleton.Instance.RedisSet(count + setKey, jsonSet);
	}

	protected override async Task Get(int count)
	{
		jsonGet = await RedisSingleton.Instance.RedisGet(count + getKey);
	}

	private void Json()
	{
		jsonSet = jsonScript.GetJson(playerData); //このクラスのインスタンスを渡し、それをJson形式にしたものをjsonSetに代入
		if (jsonGet != null) {
			syncPos = jsonScript.GetPosition(jsonGet);
			syncRotateY = jsonScript.GetEularAngelY(jsonGet);
		}
	}

	private void LerpPosition(Transform playerTransform) //プレイヤーの移動を補間する処理。TransformGet()のfor文から呼び出され、numにはfor文のiが入る
	{
		playerTransform.position = Vector3.Lerp(playerTransform.position, syncPos, Time.deltaTime * lerpRate);
	}

	private void LerpRotation(Transform playerTransform)
	{
		Vector3 ro = playerTransform.eulerAngles;
		ro.y = Mathf.Lerp(ro.y, syncRotateY, Time.deltaTime * lerpRate);
		playerTransform.eulerAngles = ro;
	}

	private void PlayerDataSet()
	{
		if (myPlayerNum == 1)
		{
			playerData.syncPos = playerTransform1.position;
			playerData.syncRoY = playerTransform1.eulerAngles.y;
		}
		else
		{
			playerData.syncPos = playerTransform2.position;
			playerData.syncRoY = playerTransform2.eulerAngles.y;
		}
	} 
}
