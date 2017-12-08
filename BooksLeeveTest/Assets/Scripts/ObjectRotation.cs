using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{

	[SerializeField] private float speed = 1.0f;
	[SerializeField] private Vector3 rotate = Vector3.zero;
	private Transform myTransform;

	private void Start()
	{
		myTransform = GetComponent<Transform>();
	}

	private void Update()
	{
		myTransform.Rotate(rotate * speed);
	}
}