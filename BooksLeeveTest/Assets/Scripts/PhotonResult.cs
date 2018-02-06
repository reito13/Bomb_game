using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonResult : MonoBehaviour {

    //プレイヤーオブジェクト
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private GameObject player3;
    [SerializeField] private GameObject player4;

    //順位床
    [SerializeField] private GameObject firstPosition;//金
    [SerializeField] private GameObject secondPosition;//銀
    [SerializeField] private GameObject thirdPosition;//銅
    [SerializeField] private GameObject fourthPosition;//黒

    //プレイヤーごとの順位
    [SerializeField] private int p1Rank = 1;//1Pの順位
    [SerializeField] private int p2Rank = 2;//2Pの順位
    [SerializeField] private int p3Rank = 3;//3Pの順位
    [SerializeField] private int p4Rank = 4;//4Pの順位

    //X座標定数
    private static float constant1p = 0.0f;
    private static float constant2p = 5.0f;
    private static float constant3p = 10.0f;
    private static float constant4p = 15.0f;
    //Y座標定数

    private static float constantY = 5.0f;

    //Z座標定数
    private static float constant1st = -10.0f;
    private static float constant2nd = -5.0f;
    private static float constant3rd = 0.0f;
    private static float constant4th = 5.0f;




    //プレイヤーの座標
    private float pos1P = constant1st;
    private float pos2P = constant2nd;
    private float pos3P = constant3rd;
    private float pos4P = constant4th;

    //順位床の座標
    private float first = constant1p;
    private float second = constant2p;
    private float third = constant3p;
    private float fourth = constant4p;

    //勝者テキスト
    private string constantText = "WINNER : ";
    [SerializeField]private Text winnerText;



    // Use this for initialization
    void Start()
    {
        RankConvert();
        PlayerPost();
        Ranking();
        WinnerText();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void PlayerPost()//プレイヤー位置確定メソッド
    {

        Instantiate(player1, new Vector3(constant1p, constantY, pos1P), Quaternion.identity);
        Instantiate(player2, new Vector3(constant2p, constantY, pos2P), Quaternion.identity);
        Instantiate(player3, new Vector3(constant3p, constantY, pos3P), Quaternion.identity);
        Instantiate(player4, new Vector3(constant4p, constantY, pos4P), Quaternion.identity);
    }

    void Ranking()//順位床割当メソッド
    {
        Instantiate(firstPosition, new Vector3(first, 0.0f, 0.0f), Quaternion.identity);
        Instantiate(secondPosition, new Vector3(second, 0.0f, 0.0f), Quaternion.identity);
        Instantiate(thirdPosition, new Vector3(third, 0.0f, 0.0f), Quaternion.identity);
        Instantiate(fourthPosition, new Vector3(fourth, 0.0f, 0.0f), Quaternion.identity);
    }


    //勝者表示メソッド
    void WinnerText()
    {
        if (p1Rank == 1)
        {
            winnerText.text = constantText + "1P";
        }
        else if (p2Rank == 1)
        {
            winnerText.text = constantText + "2P";
        }
        else if (p3Rank == 1)
        {
            winnerText.text = constantText + "3P";

        }
        else if (p4Rank == 1)
        {
            winnerText.text = constantText + "4P";
        }
        else
        {
            winnerText.text = "DRAW";
        }
    }

    //順位変換メソッド
    #region
    void RankConvert()//順位変換メソッド
    {
        switch (p1Rank)
        {
            case 1:
                pos1P = constant1st;
                first = constant1p;
                break;
            case 2:
                pos1P = constant2nd;
                second = constant1p;
                break;
            case 3:
                pos1P = constant3rd;
                third = constant1p;
                break;
            case 4:
                pos1P = constant4th;
                fourth = constant1p;
                break;
            default:
                break;
        }

        switch (p2Rank)
        {
            case 1:
                pos2P = constant1st;
                first = constant2p;
                break;
            case 2:
                pos2P = constant2nd;
                second = constant2p;
                break;
            case 3:
                pos2P = constant3rd;
                third = constant2p;
                break;
            case 4:
                pos2P = constant4th;
                fourth = constant2p;
                break;
            default:
                break;
        }

        switch (p3Rank)
        {
            case 1:
                pos3P = constant1st;
                first = constant3p;
                break;
            case 2:
                pos3P = constant2nd;
                second = constant3p;
                break;
            case 3:
                pos3P = constant3rd;
                third = constant3p;
                break;
            case 4:
                pos3P = constant4th;
                fourth = constant3p;
                break;
            default:
                break;
        }

        switch (p4Rank)
        {
            case 1:
                pos4P = constant1st;
                first = constant4p;
                break;
            case 2:
                pos4P = constant2nd;
                second = constant4p;
                break;
            case 3:
                pos4P = constant3rd;
                third = constant4p;
                break;
            case 4:
                pos4P = constant4th;
                fourth = constant4p;
                break;
            default:
                break;
        }

    }
    #endregion


}
