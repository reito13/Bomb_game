using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using BookSleeve;
using System.Threading.Tasks;

namespace NetWork
{

	public class Test : MonoBehaviour
	{

		[SerializeField] private Transform playerTransfrom = null;
		private RedisConnection rConnection = null;

		private async void Awake()
		{
			RedisConnection rConnection = new RedisConnection("172.15.100.19");
			await rConnection.Open();
			StartCoroutine(TestIEnumerator());
		}

		public void startUpdate()
		{
			TestIEnumerator();
		}

		private IEnumerator TestIEnumerator()
		{
			yield return TransformUpdate();
		}

		private IEnumerator TimeManager()
		{
			//InputData();
			//OutPutData();
			yield return new WaitForSeconds(0.3f);
		}

		private async Task<bool> TransformUpdate()
		{
			//ゲームの開始時間を文字列に
			//部屋番号　＋　時間　+　プレイヤー
			Debug.Log("Start");

			var x = rConnection.Strings.Increment(db: 0, key: "posX", value: playerTransfrom.position.x);
			var y = rConnection.Strings.Increment(db: 0, key: "posY", value: playerTransfrom.position.y);
			var z = rConnection.Strings.Increment(db: 0, key: "posZ", value: playerTransfrom.position.z);

			await Task.WhenAll(x, y, z);

			var gX = rConnection.Strings.Get(db: 0, key: "posX");
			var gY = rConnection.Strings.Get(db: 0, key: "posY");
			var gZ = rConnection.Strings.Get(db: 0, key: "posZ");

			await Task.WhenAll(gX, gY, gZ);

			Debug.Log("X座標: " + gX + ",Y座標: " + gY + ",Z座標: " + gZ);

			return true;
		}
	}

	[System.Serializable]
	public static class Propaty
	{
		public static int hp;
		public static Vector3 pos;
	}

}