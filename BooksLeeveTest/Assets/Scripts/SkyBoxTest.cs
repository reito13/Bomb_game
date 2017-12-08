using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SkyBoxTest : MonoBehaviour
{
	public GameObject skyboxCamera;//スカイボックス用カメラ
	public Material[] skyBoxMaterial;//スカイボックスマテリアル配列

	/// スカイボックスの変更
	/// 《使い方》
	/// ①ヒエラルキーへMainCameraとSkyboxCameraを用意
	/// ②SkyboxCameraへskyboxコンポーネントを追加、AndioListenerコンポーネントを削除
	/// ③MainCameraの設定（Clear Flags:depth only、depth:-1）
	/// ④SkyboxCameraの設定（Clear Flags:skybox、depth:-2）
	/// ⑤変数skyboxCameraへスカイボックス用カメラをアサイン
	/// ⑥配列skyBoxMaterialへスカイボックス用マテリアルをアサイン
	/// ⑦メソッドを呼び出し引数へ変更したいskyBoxMaterialの配列ナンバーを指定
	public void SkyboxChange(int no)
	{
		try
		{
			skyboxCamera.GetComponent<Skybox>().material = skyBoxMaterial[no];
		}
		catch
		{
			Debug.LogError("スカイボックス変更に失敗しました。");
		}
	}
}