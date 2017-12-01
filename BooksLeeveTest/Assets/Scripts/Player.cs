using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private Transform myTransform;
	[SerializeField] private Transform rotateTransform = null;

	private Vector3 moveDir;
	private float rotateX, rotateY;

	[SerializeField] private float speed = 10.0f;

	[SerializeField] private GameObject bombPrefab = null;
	[SerializeField] private Transform bombPos = null;

	private void Start()
	{
		myTransform = GetComponent<Transform>();
		moveDir = Vector3.zero;
	}

	private void Update()
	{
		
	}

	private void FixedUpdate()
	{
		Move();
		Rotate();
	}

	private void Move()
	{
		myTransform.Translate(moveDir * speed);
	}
	private void Rotate()
	{
		rotateTransform.Rotate(Vector3.right * rotateX);
		myTransform.Rotate(Vector3.up * rotateY);
		Quaternion ro = rotateTransform.rotation;
		ro.x = Mathf.Clamp(ro.x,-0.6427f,0.6427f);
		rotateTransform.rotation = ro;
	}

	public void Bomb()
	{
		Quaternion bombRo = myTransform.rotation;
		//bombRo.x = rotateTransform.eulerAngles.x;
		GameObject bomb = Instantiate(bombPrefab,bombPos.position,bombRo) as GameObject;
		bomb.GetComponent<Bomb>().Set(3.0f);
	}

	public void SetMoveDir(float x,float z)
	{
		moveDir.x = x;
		moveDir.z = z;
	}

	public void SetRotate(float x,float y)
	{
		rotateX = x;
		rotateY = y;
	}

}
