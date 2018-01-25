using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//マッチングロビー用UI

public class RobbyUI : MonoBehaviour
{

    [SerializeField]

    private GameObject btnPref;  //ボタンプレハブ

    //ボタン表示数

    const int BUTTON_COUNT = 10;

    void Start()

    {

        //Content取得(ボタンを並べる場所)

        RectTransform content = GameObject.Find("Canvas/Scroll View/Viewport/Content").GetComponent<RectTransform>();

        //Contentの高さ決定

        //(ボタンの高さ+ボタン同士の間隔)*ボタン数

        float btnSpace = content.GetComponent<VerticalLayoutGroup>().spacing;

        float btnHeight = btnPref.GetComponent<LayoutElement>().preferredHeight;

        content.sizeDelta = new Vector2(0, (btnHeight + btnSpace) * BUTTON_COUNT);

        for (int i = 1; i <= BUTTON_COUNT; i++)

        {

            int no = i;

            //ボタン生成

            GameObject btn = (GameObject)Instantiate(btnPref);

            //ボタンをContentの子に設定

            btn.transform.SetParent(content, false);

            //ボタンのテキスト変更

            btn.transform.GetComponentInChildren<Text>().text = "ルーム" + no.ToString();

            //ボタンのクリックイベント登録

            btn.transform.GetComponent<Button>().onClick.AddListener(() => OnClick(no));

        }

    }

    public void OnClick(int no)

    {

        Debug.Log(no);

    }

}


