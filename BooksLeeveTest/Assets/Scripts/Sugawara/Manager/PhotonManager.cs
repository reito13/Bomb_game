using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviour {

	[SerializeField] private MainManager mainManager = null;

	[SerializeField] private Transform[] playerPosition = null;//Playerの生成位置

	[SerializeField] private int players = 2;

	void Start()
	{
		// Photonに接続する(引数でゲームのバージョンを指定できる)
		PhotonNetwork.ConnectUsingSettings("1.0");
	}

	// ロビーに入ると呼ばれる
	void OnJoinedLobby()
	{
		Debug.Log("ロビーに入りました。");

		// ルームに入室する
		PhotonNetwork.JoinRandomRoom(); //ランダムで部屋に入る(OnjoinRoomへ飛ぶ)
	}

	// ルームに入室すると呼ばれる
	void OnJoinedRoom()
	{
		Debug.Log("ルームへ入室しました。");

		StartCoroutine(GameStartCoroutine());

	}

	// ルームの入室に失敗すると呼ばれる
	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("ルームの入室に失敗しました。");

		// ルームがないと入室に失敗するため、その時は自分で作る
		// 引数でルーム名を指定できる
		PhotonNetwork.CreateRoom("myRoomName");
	}

	private IEnumerator GameStartCoroutine()
	{
		while (true) {

			Debug.Log(PhotonNetwork.playerList.Length);
			if (PhotonNetwork.playerList.Length == players)
				break;

			yield return new WaitForSeconds(0.5f);
		}

		if (PhotonNetwork.player.ID == 1)
		{
			GameObject itemManager = PhotonNetwork.Instantiate("ItemManager", transform.position, transform.rotation, 0); //ItemManagerを生成する
		}

		yield return new WaitForSeconds(3.0f);

		Debug.Log(PhotonNetwork.player.ID);
		
		PhotonNetwork.Instantiate("PlayerSet", playerPosition[PhotonNetwork.player.ID - 1].position, transform.rotation, 0); //Playerを生成する

		mainManager.StartCoroutine(mainManager.MainCoroutine());
	}

}