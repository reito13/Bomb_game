using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPhotonManager : Photon.MonoBehaviour
{
	public int[] roomCount = new int[3];

	public GameObject[] roomPlayers2 = new GameObject[2];
	public GameObject[] roomPlayers3 = new GameObject[3];
	public GameObject[] roomPlayers4 = new GameObject[4];

	[SerializeField] private string roomPath = "";

	private void Awake()
	{
		PhotonNetwork.ConnectUsingSettings("1.0"); //Photonに接続
	}

	public void Join(int players,GameObject player)
	{
		roomPath = "Room" + players.ToString();
		/*bool b = OnJoinRoom2(str);
		if (!b)
		{
			OnCreateRoom2(str);
		}*/
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.IsOpen = true;     // 部屋を開くか  
		roomOptions.IsVisible = true;  // 一覧に表示するか  
		roomOptions.MaxPlayers = (byte)players;        // 最大参加人数
		Debug.Log(roomOptions.MaxPlayers);

		PhotonNetwork.JoinOrCreateRoom(roomPath,roomOptions,new TypedLobby());
		//PhotonNetwork.CreateRoom(roomPath);

	}


	public void Leave()
	{

	}

	private void OnJoinedLobby()
	{
		Debug.Log("ロビーに入りました。");
	}

	private void OnJoinedRoom()
	{
		Debug.Log("ルーム" + roomPath + "へ入室しました。");

		Debug.Log(PhotonNetwork.room.Name);
	//	SceneManager.LoadScene("Test2");

	}

	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("ルームの入室に失敗しました。");

		// ルームがないと入室に失敗するため、その時は自分で作る
		// 引数でルーム名を指定できる
		//PhotonNetwork.CreateRoom("myRoomName");

	//	Debug.Log("ルームを作成しました。");

	}

	void OnReceivedRoomListUpdate()
	{
		//ルーム一覧を取る
		RoomInfo[] rooms = PhotonNetwork.GetRoomList();
		if (rooms.Length == 0)
		{
			Debug.Log("ルームが一つもありません");
		}
		else
		{
			//ルームが1件以上ある時ループでRoomInfo情報をログ出力
			for (int i = 0; i < rooms.Length; i++)
			{
				Debug.Log("RoomName:" + rooms[i].Name);
				Debug.Log("userName:" + rooms[i].CustomProperties["userName"]);
				Debug.Log("userId:" + rooms[i].CustomProperties["userId"]);
			}
		}
	}

}
