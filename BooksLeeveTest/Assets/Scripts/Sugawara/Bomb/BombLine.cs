using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TakashiCompany.Unity.Util;

public class BombLine : MonoBehaviour {

	public Vector3 pos;
	public Vector3 force;
	[SerializeField] private float mass = 0;
	[SerializeField] private Vector3 gravity = Vector3.zero;
	[SerializeField] private float gravityScale = 0;
	public float time;
	private Vector3 objPos;
	[SerializeField] private Transform[] objs = null;

	private void Awake()
	{
		for (int i = 0; i < objs.Length; i++)
		{
			objs[i].localScale *= 1 + (i * 0.1f);
		}
	}

	private void FixedUpdate()
	{
		LineDraw();
	}

	private void LineDraw()
	{
		for (int i = 0;i<objs.Length;i++) {
			objPos = TrajectoryCalculate.Force(pos, force, mass, gravity, gravityScale, 0.1f * (i+1));
			objs[i].position = objPos;
		}
	}

	public void ValueSet(Vector3 p,Vector3 f)
	{
		pos = p;
		force = f;
		//距離250:時間0.85秒
		//    700.:    1.89
		//    475.:    1.35
	}
}
