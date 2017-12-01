using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformData : MonoBehaviour {

	public Transform transformData;

	public void SetTransform(Transform t)
	{
		transformData = t;
	}

	public Transform GetTransform()
	{
		return transformData;
	}

}
