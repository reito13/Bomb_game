using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;


public class P_Matching_2018_0131_base : Photon.MonoBehaviour {
    //Photon用マッチングシステム

    //マッチング用スポットへattachする
    [SerializeField] private SceneTransitionManager sceneManager = null;

    private bool matchingCompFlag = false;    //マッチング完了フラグ




    private int pNum = 0;     //下記のplayerNum変数に割り当てるプレイヤーナンバーを保持しておく為の当script内でのプレイヤーナンバー保持変数
                              //public int playerNum  MainManager.cs　により宣言されたプレイヤーナンバー保持変
    private int enemyNum = 0; //敵のpNum

    public int matchedPeople = 0;//ルーム内のマッチング済人数

    [SerializeField] private bool ready = false; //参戦準備完了（参戦ボタン）
    private bool matchingNow = false; //マッチング中かどうか


    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings("0.0");


    }

    private void Start()
    {

        /* ルームオプションの設定 */

        // RoomOptionsのインスタンスを生成
        RoomOptions roomOptions = new RoomOptions();

        // ルームに入室できる最大人数。0を代入すると上限なし。
        roomOptions.MaxPlayers = 4;

        // ルームへの入室を許可するか否か
        roomOptions.IsOpen = true;

        // ロビーのルーム一覧にこのルームが表示されるか否か
        roomOptions.IsVisible = true;

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //データの送信
            stream.SendNext(matchedPeople);
        }
        else
        {
            //データの受信
            this.matchedPeople = (int)stream.ReceiveNext();

        }
    }



    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update");
        Debug.Log(matchedPeople);




        // プレイヤーナンバー　= pNum; (setも)
        if (matchingNow == true)
        {
            PhotonNetwork.JoinRandomRoom();
            Debug.Log(matchedPeople + "人がルームに接続しています。");


        }

        if ((matchedPeople >= 3))
        {
            matchingCompFlag = true;
            Debug.Log("MatchingCompleated");

        }

        if (matchingCompFlag == true)
        {
            Debug.Log("MatchingCompFlag");
            if (Input.GetKey(KeyCode.Return))
            {

                //参戦準備完了
                ready = true;


            }


        }

        if (ready == true)
        {
            sceneManager.SceneTransition();  //メインシーンに遷移
        }


    }



    private void OnTriggerStay() //マッチングエリア滞在判定
    {
        matchingNow = true;



        Debug.Log("countOfPlayersInRooms = " + PhotonNetwork.countOfPlayersInRooms);
        matchedPeople = PhotonNetwork.countOfPlayersInRooms;
        Debug.Log("マッチングエリア内");
    }

    private void OnTriggerExit()
    {
        matchingNow = false;
        matchingCompFlag = false;
        PlayerNumberReset();
        Debug.Log("マッチングエリアから逸脱");
        PhotonNetwork.LeaveRoom();
    }


    void AssignPlayerNumber()//プレイヤーナンバー割当メソッド
    {

        //set int型変数　インクリメント　;
        //get int型変数 ;

        //if(int型変数　>= 2)
        {
            pNum = 2;
            //  set int型変数　= 5;
        }
        //  else
        {
            pNum = 1;
        }








    }

    void PlayerNumberReset() //プレイヤーナンバーリセット（再割当て用）メソッド
    {
        pNum = 0;
    }

    void EnemyPlayerNumGet()
    {
        //gettingNumRate秒ごとに取得
        {
            //get　相手のプレイヤーナンバー ;
            //enemyNum = 相手プレイヤーナンバー



        }

    }



    //  JoinRandomRoom()が失敗した(false)時に呼ばれる
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed");

        // ルームを作成（今回の実装では、失敗＝マスタークライアントなし、として「ルーム」を作成）
        PhotonNetwork.CreateRoom(null);
    }

    //  ルームに入れた時に呼ばれる（自分の作ったルームでも）
    void OnJoinedRoom()
    {
        Debug.Log("入室");
        matchedPeople++;
    }

    //接続が切断されたときに呼ばれる
    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("Disconnected from Photon.");
    }

    //接続失敗時に呼ばれる
    public void OnFailedToConnectToPhoton(object parameters)
    {
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.networkingPeer.ServerAddress);
    }


    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinRandomRoom();
    }

    //ルーム退出時に呼ばれる
    void OnLeftRoom()
    {
        Debug.Log("ルームから退出");
        matchedPeople--;
    }




}
