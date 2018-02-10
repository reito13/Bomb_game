using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StageCreate))]//拡張するクラスを指定
public class StageCreateEditor : Editor {

	public override void OnInspectorGUI()
	{
		//元のInspector部分を表示
		base.OnInspectorGUI();

		//targetを変換して対象を取得
		StageCreate stageCreate = target as StageCreate;

		//PublicMethodを実行する用のボタン
		if (GUILayout.Button("Create"))
		{
			stageCreate.SetUp();
		}
		if (GUILayout.Button("Destroy"))
		{
			stageCreate.Reset();
		}
	}

}  
