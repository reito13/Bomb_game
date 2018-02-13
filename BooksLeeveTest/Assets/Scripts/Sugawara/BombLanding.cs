using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLanding : MonoBehaviour {

	public Transform targetTransform = null;
	[SerializeField] Transform playerTransform = null;

	[SerializeField] private float speed = 10.0f;

	private float setY;

	private float offsetY;
	private float startPlayerY;

	//6.5:240 ~ 25.5:515 = 19:275
	//6:250 : 8 ~ 36:720 : 20


	private void Awake()
	{
		//offsetY = playerTransform.position.y - transform.position.y;
		offsetY = 1;
		startPlayerY = playerTransform.position.y;
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void Move() {
		targetTransform.position = targetTransform.position + (transform.forward * setY * speed);
		targetTransform.localPosition = new Vector3(targetTransform.localPosition.x,offsetY, targetTransform.localPosition.z);
	}

	public void SetBombLandingPosition(float y)
	{
		setY = y;
	}

	public float GetPower()
	{
		float distance = Vector3.Distance(targetTransform.position, playerTransform.position);

		return distance * 100;
	}

	public float GetDistance()
	{
		Debug.Log(Vector3.Distance(targetTransform.position, playerTransform.position));
		return Vector3.Distance(targetTransform.position, playerTransform.position);
	}


}
