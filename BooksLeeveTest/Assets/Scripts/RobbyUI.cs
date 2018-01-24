using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//マッチングロビー用UI

public class RobbyUI : MonoBehaviour {

    GameObject ButtonPrefab;


    // Use this for initialization
    void Start()
    {
        Transform list = transform.Find("List");
        for (int i = 0; i <= 5; i++)
        {
            //プレハブからボタンを生成
            GameObject listButton = Instantiate(ButtonPrefab) as GameObject;
            //Vertical Layout Group の子にする
            listButton.transform.SetParent(list, false);
            listButton.transform.Find("Text").GetComponent<Text>().text = i.ToString();
            //以下、追加---------
            int n = i;
            //引数に何番目のボタンかを渡す
            listButton.GetComponent<Button>().onClick.AddListener(() => MyOnClick(n));
        }
    }
    void MyOnClick(int index)
    {
        print(index);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
