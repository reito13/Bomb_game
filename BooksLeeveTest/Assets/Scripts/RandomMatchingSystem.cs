using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*

//ランダムマッチングシステム
//マッチング用スポットへattachする


public class RondomMatchingSystem : SingletonMonoBehaviour<RondomMatchingSystem>
{
    
    private bool matchingCompFlag = false;    //マッチング完了フラグ

    //[SerializeField] private int rooms = 4;      //ルーム数
    [SerializeField] private int players = 2;    //参戦可能人数（プレイヤー数）
    [SerializeField] private int gettingNumRate = 3;//相手プレイヤーナンバー取得周期（秒）

    private string roomName;//　ルーム名／ルーム番号
                            //フレンドマッチにおける、合言葉マッチングの仕組みを後に実装する為、string型で宣言し、"roonName"としておく。



    private int pNum = 0;     //下記のplayerNum変数に割り当てるプレイヤーナンバーを保持しておく為の当script内でのプレイヤーナンバー保持変数
                              //public int playerNum  MainManager.cs　により宣言されたプレイヤーナンバー保持変数

    private int enemyNum = 0; //敵のpNum

    [SerializeField] private bool ready = false; //参戦準備完了（参戦ボタン）
    private bool matchingNow = false; //マッチング中かどうか


    void Start()
    {

    }
    void MyOnClick(int index)
    {
        print(index);
    }
    // Update is called once per frame
    void Update()
    {
        // プレイヤーナンバー　= pNum; (setも)
        if (matchingNow == true)
        {
            AssignPlayerNumber();
            EnemyPlayerNumGet();
        }

        if ((pNum + enemyNum = 3))
        {
            matchingCompFlag = true;
        }

        if (matchingCompFlag == true)
        {
            //if (参戦完了ボタン押)
             // {
             //      ready = true;
             // }
            
        }

        if (ready == true)
        {
            //メインシーンに遷移
        }


    }

    private void AssignId() //ID割り当てメソッド
    {

    }

    private void OnTriggerStay() //マッチングエリア滞在判定
    {
        matchingNow = true;
    }

    private void OnTriggerExit()
    {
        matchingNow = false;
        matchingCompFlag = false;
        PlayerNumberReset();
    }


    void AssignPlayerNumber()//プレイヤーナンバー割当メソッド
    {

        //set int型変数　インクリメント　;
        //get int型変数 ;

        //if(int型変数　>= 2)
        {
           pNum = 2;
           set int型変数　= 5;
        }
      //  else
        {
            pNum = 1;
        }



        




    }

    void PlayerNumberReset() //プレイヤーナンバーリセット（再割当て用）メソッド
    {
        //set int型変数 = 0;
        //pNum = 0
    }

    void EnemyPlayerNumGet()
    {
        //gettingNumRate秒ごとに取得
        {
        //get　相手のプレイヤーナンバー ;
        //enemyNum = 相手プレイヤーナンバー



        }
        
    }

}
*/