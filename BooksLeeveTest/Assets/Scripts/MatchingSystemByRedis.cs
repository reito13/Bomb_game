using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Redisによるマッチングシステム


public class MatchingSystemByRedis : MonoBehaviour
{

    private bool matchingCompFlag = false;    //ルームは満員か（マッチング完了フラグ）

    [SerializeField] private int rooms = 4;      //ルーム数
    [SerializeField] private int players = 2;    //参戦可能人数（プレイヤー数）

    private string roomName ;//　ルーム名／ルーム番号
                             //フレンドマッチにおける、合言葉マッチングの仕組みを後に実装する為、string型で宣言し、"roonName"としておく。
    private int pNum;        //下記のplayerNum変数に割り当てるプレイヤーナンバーを保持しておく為の当script内でのプレイヤーナンバー保持変数
  //public int playerNum  MainManager.cs　により宣言されたプレイヤーナンバー保持変数

    

    

    [SerializeField] private bool ready = false; //参戦準備完了（参戦ボタン）

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        MainManager.playerNum = pNum;
	}

    private void AssignId () //ID割り当てメソッド
    {

    }
}
