using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class PhotonMatching : Photon.MonoBehaviour {

    //Photon用マッチングシステム

    //マッチング用スポットへattachする
    [SerializeField] private SceneTransitionManager sceneManager = null;

    private bool matchingCompFlag = false;    //マッチング完了フラグ

    public int matchedPeople = 0;//ルーム内のマッチング済人数

    public bool joinedFlag = false;//入室済みフラグ

    [SerializeField] private int ready = 0; //参戦準備完了（参戦ボタン）


    void Awake()
    {
        PhotonNetwork.ConnectUsingSettings ("0.0");


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
        Debug.Log(matchedPeople+"人がマッチング中");
        




        if ((matchedPeople >= 2))
        {
            matchingCompFlag = true;
            Debug.Log("MatchingCompleated");
            Debug.Log(matchedPeople + "人がマッチング中");

        }

        if (matchingCompFlag == true)
        {
            Debug.Log("MatchingCompFlag");
            Debug.Log("ENTERキーを入力で参戦準備完了");
            if (Input.GetKey(KeyCode.Return))
            {

                //参戦準備完了
                ready++ ;


            }


        }

        if (ready == matchedPeople)
        {
            sceneManager.SceneTransition();  //メインシーンに遷移
        }


    }



    private void OnTriggerStay() //マッチングエリア滞在判定
    {
        if (joinedFlag == false)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        Debug.Log(matchedPeople);

        Debug.Log("マッチングエリア内");
    }

    private void OnTriggerExit()
    {

        matchingCompFlag = false;
        
        Debug.Log("マッチングエリアから逸脱");
        PhotonNetwork.LeaveRoom();

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
        joinedFlag = true;
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
        joinedFlag = false;
        matchedPeople--;
    }




}

