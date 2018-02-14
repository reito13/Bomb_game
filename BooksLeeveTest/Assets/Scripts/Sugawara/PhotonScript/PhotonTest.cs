using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PhotonTest : Photon.MonoBehaviour
{
	private RoomInfo[] rooms;


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

		Debug.Log(PhotonNetwork.room.Name);
		SceneManager.LoadScene("Test2");

	}

	// ルームの入室に失敗すると呼ばれる
	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("ルームの入室に失敗しました。");

		// ルームがないと入室に失敗するため、その時は自分で作る
		// 引数でルーム名を指定できる
		PhotonNetwork.CreateRoom("myRoomName");

		Debug.Log("ルームを作成しました。");

	}
	/*void OnReceivedRoomListUpdate()
	{
		rooms = PhotonNetwork.GetRoomList();
		Debug.Log("ルーム数" + rooms.Length);
		if(rooms.Length == 1)
		{
			SceneManager.LoadScene("Test2");
		}
	}*/
}