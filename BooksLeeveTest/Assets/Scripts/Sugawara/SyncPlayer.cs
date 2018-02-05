using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

public class SyncPlayer : BaseAsyncLoop {

    [SerializeField] private JsonTest jsonScript = null; 

    [SerializeField] private Transform playerTransform1; //1PのTransform
    [SerializeField] private Transform playerTransform2; //2PのTransform

	[SerializeField] private float lerpRate = 1; //2ベクトル間を補間する
	public Vector3 syncPos; //サーバーからGetした全プレイヤーの座標を格納する変数。座標移動の補間に使用する
	public float syncRotateY;

	private string setKey; //データをセットするときのキー
	private string getKey; //データをゲットするときのキー

	private string jsonSet; //セットするJsonデータ
	private string jsonGet; //ゲットしたJsonデータ

	private int myPlayerNum = 1; //自分が操作しているプレイヤーの番号 MainManagerのPlayerNumが入る

	private SyncPlayerData playerData;

	protected override void Awake()
	{
		count = 0;

		myPlayerNum = MainManager.playerNum;

		playerData = new SyncPlayerData();
		PlayerDataSet();

		setKey = "," + myPlayerNum + "," + "PlayerTransform";
		getKey = "," + ((myPlayerNum == 1) ? 2 : 1) + "," + "PlayerTransform";

		Task task = TransformCoroutine();
	}

	protected override async Task TransformCoroutine()
	{
		await Task.Delay(3000);
		await redis.RedisSet(count + setKey, jsonSet);
		await redis.RedisSet(count + getKey, jsonSet);

		while (true)
		{
			CountStart();
			count++;
			await redis.RedisSet(count + setKey, jsonSet);
			//await redis.RedisSet(setKey, jsonSet);
			await Task.Delay(100);
			jsonGet = await redis.RedisGet(count + getKey);
			//jsonGet = await redis.RedisGet(getKey);
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

	private void Json()
	{
		jsonSet = jsonScript.GetJson(playerData); //データクラスのインスタンスを渡し、それをJson形式にしたものをjsonSetに代入
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
