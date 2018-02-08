﻿using UnityEngine;
using System.Collections;

public class PhotonTest :Photon.MonoBehaviour
{

	[SerializeField] private Transform[] playerPosition = null;

	[SerializeField] private Transform[] itemManagerPosition = null;

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
		PhotonNetwork.JoinRandomRoom();
	}

	// ルームに入室すると呼ばれる
	void OnJoinedRoom()
	{
		Debug.Log("ルームへ入室しました。");

		GameObject player = PhotonNetwork.Instantiate("PlayerSet", playerPosition[PhotonNetwork.player.ID-1].position, transform.rotation, 0);
		PhotonNetwork.Instantiate("ItemManager",itemManagerPosition[PhotonNetwork.player.ID-1].position,transform.rotation,0);
	}

	// ルームの入室に失敗すると呼ばれる
	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("ルームの入室に失敗しました。");

		// ルームがないと入室に失敗するため、その時は自分で作る
		// 引数でルーム名を指定できる
		PhotonNetwork.CreateRoom("myRoomName");

	}

}