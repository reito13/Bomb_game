using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

public class PlayerTransformManager : BaseAsyncLoop {

	[SerializeField] private Transform[] playerTransforms = null; //全プレイヤーのTransform
	[SerializeField] float lerpRate = 15; //2ベクトル間を補間する
	private Vector3 syncPos; //サーバーからGetした全プレイヤーの座標を格納する変数。座標移動の補間に使用する
	private float syncRotateY;

	private int i,j; //ループを回す用の変数
	private int transformCount; //playerTransformsの配列数
	private int myPlayerNum; //自分が操作するプレイヤーの番号 例:1なら1P

	private string setKey; //データをセットするときのキー
	private string getKey; //データをゲットするときのキー
	private string str; //Redisから読み込んだ分割前のプレイヤー情報
	private float[] splitStr; //プレイヤー情報を分割しfloatにしたもの 要素0がX,1がY,2がZの座標,3がYの角度

	protected override void Awake()
	{
		transformCount = playerTransforms.Length;
		myPlayerNum = MainManager.playerNum;
		myPlayerNum--;
		setKey = "PlayerTransform" + myPlayerNum;
		getKey = "PlayerTransform" + ((myPlayerNum == 0) ? 1 : 0);
		Task task = TransformCoroutine();
	}

	private void FixedUpdate()
	{
		for (j = 0; j < transformCount; j++)
		{
			if (myPlayerNum != j)
			{
				Split();
				LerpPosition(j);
				LerpRotation(j);
			}
		}
	}

	protected override async Task TransformCoroutine()
	{
		while (true)
		{
			if (RedisSingleton.Instance.connect)
			{
				//CountStart();
				await Set();
				//CountEnd();
				await Get();
			}
			await Task.Delay(100);
		}
	}

	protected override async Task Set()
	{
		await RedisSingleton.Instance.RedisSet(setKey, playerTransforms[myPlayerNum].position.x.ToString("f2") + "," + playerTransforms[myPlayerNum].position.y.ToString("f2") + ","
			+ playerTransforms[myPlayerNum].position.z.ToString("f2") + "," + playerTransforms[myPlayerNum].eulerAngles.y.ToString("f2"));
	}

	protected override async Task Get()
	{
		str = await RedisSingleton.Instance.RedisGet(getKey);

	}

	private void Split()
	{
		if (str == null)
			return;

		splitStr = str.Split(',').Select(s => float.Parse(s)).ToArray();
		syncPos = new Vector3(splitStr[0], splitStr[1], splitStr[2]);
		syncRotateY = splitStr[3];

	}

	private void LerpPosition(int num) //プレイヤーの移動を補間する処理。TransformGet()のfor文から呼び出され、numにはfor文のiが入る
	{
		playerTransforms[num].position = Vector3.Lerp(playerTransforms[num].position, syncPos, Time.deltaTime * lerpRate);
	}

	private void LerpRotation(int num)
	{
		Vector3 ro = playerTransforms[num].eulerAngles;
		//quaternion.y = Mathf.Lerp(quaternion.y,syncRotateY,Time.deltaTime * lerpRate);
		//quaternion.y = syncRotateY;
		//playerTransforms[num].rotation = Quaternion.Slerp(playerTransforms[num].rotation,quaternion,Time.deltaTime);

		//ro.y = syncRotateY;
		ro.y = Mathf.Lerp(ro.y,syncRotateY,Time.deltaTime * lerpRate);
		//playerTransforms[num].rotation = Quaternion.Euler(ro);
		playerTransforms[num].eulerAngles = ro;
	}

}