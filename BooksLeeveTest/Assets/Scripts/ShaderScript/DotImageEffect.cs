﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnitySampleAssets.ImageEffects;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/DotImgeEffect")]
public class DotImageEffect : ImageEffectBase
{
	[Range(1, 10)]
	public int Downsample = 3;  // 解像度を落とす回数+1。1回ごとに解像度を半分に落とす。

	[Range(0.001f, 0.1f)]
	public float ValidDepthThreshold = 0.01f;  // 深度が近いドットを平均化するときの閾値

	[Range(0, 5f)]
	public float EdgeLuminanceThreshold = 0.8f; // エッジ部と判定するための輝度差。この値以上離れていたらエッジとみなす。

	[Range(0, 1f)]
	public float SharpEdge = 0.5f;  // 平均化する際のコントラスト強調パラメータ

	[Range(-5, 5f)]
	public float SampleDistance = 1f;  // テクスチャサンプリング距離

	public List<float> SliceDepth;


	void Update()
	{
		// 深度の取得を有効にする
		GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
	}

	[ImageEffectOpaque]
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		material.SetFloat("_ValidDepthThreshold", ValidDepthThreshold);
		material.SetFloat("_EdgeLuminanceThreshold", EdgeLuminanceThreshold);
		material.SetFloat("_SharpEdge", SharpEdge);

		var RT = new RenderTexture[Downsample];
		for (int i = 0; i < Downsample; i++)
		{
			RT[i] = RenderTexture.GetTemporary(Screen.width / (int)Mathf.Pow(2, i), Screen.height / (int)Mathf.Pow(2, i), 0, RenderTextureFormat.ARGBHalf);
			RT[i].filterMode = FilterMode.Point;
		}
		// RT0のAチャンネルに、深度*(1-α)を格納する。
		// 深度*(1-α)は優先度として作用する。
		Graphics.Blit(source, RT[0], material, 0);

		// 1回毎に解像度を半分に落としドット化する
		for (int i = 1; i < Downsample; i++)
		{
			material.SetVector("_PixelSize", new Vector4(1.0f / RT[i - 1].width, 1.0f / RT[i - 1].height, 0, 0) * SampleDistance);

			// ドット絵変換
			Graphics.Blit(RT[i - 1], RT[i], material, 3);
		}

		if (SliceDepth.Count == 0 || SliceDepth.Count != Downsample)
		{
			// すべて同じドット粒度
			Graphics.Blit(RT[Downsample - 1], destination);
		}
		else
		{
			SliceDepth[0] = 1.01f;

			var RTD = new RenderTexture[Downsample];
			for (int i = 0; i < Downsample; i++)
			{
				RTD[i] = RenderTexture.GetTemporary(RT[i].width, RT[i].height, 0, RenderTextureFormat.RHalf);
				RTD[i].filterMode = FilterMode.Point;
			}


			for (int i = 0; i < Downsample; i++)
			{
				if (i == 0)
				{
					var FullDepth = RenderTexture.GetTemporary(Screen.width, Screen.height, 0, RenderTextureFormat.RHalf);
					Graphics.Blit(source, FullDepth, material, 1);
					material.SetVector("_PixelSize", new Vector4(1.0f / Screen.width, 1.0f / Screen.height, 0, 0));
					Graphics.Blit(FullDepth, RTD[0], material, 2);
					RenderTexture.ReleaseTemporary(FullDepth);
				}
				else
				{
					material.SetVector("_PixelSize", new Vector4(1.0f / RTD[i - 1].width, 1.0f / RTD[i - 1].height, 0, 0));
					Graphics.Blit(RTD[i - 1], RTD[i], material, 2);
				}
			}

			for (int i = Downsample - 1; i >= 1; i--)
			{
				material.SetFloat("_SliceDepth", SliceDepth[i]);
				material.SetTexture("_MinDepth", RTD[Downsample - 1]);
				Graphics.Blit(RT[i], RT[i - 1], material, 4);
			}
			Graphics.Blit(RT[0], destination);


			for (int i = 0; i < Downsample; i++)
			{
				RenderTexture.ReleaseTemporary(RTD[i]);
			}
		}

		for (int i = 0; i < Downsample; i++)
		{
			RenderTexture.ReleaseTemporary(RT[i]);
		}
	}

}