using UnityEngine;
using System.Collections;

public class PhotonTest :Photon.MonoBehaviour
{

	[SerializeField] private Vector3[] playerPosition;
	private PhotonPlayerController photonPlayer;
	private GameObject[] bombs = new GameObject[3];
	private PhotonBomb[] bombScripts = new PhotonBomb[3];

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

		for (int i = 0; i < 3; i++)
		{
			bombs[i] = PhotonNetwork.Instantiate("Bomb", transform.position, transform.rotation, 0);
			bombScripts[i] = bombs[i].GetComponent<PhotonBomb>();
		}

		GameObject player = PhotonNetwork.Instantiate("Player", playerPosition[0], transform.rotation, 0);

		for (int i = 0; i < 3; i++)
		{
			photonPlayer.bombPrefabs[i] = bombs[i];
		}
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