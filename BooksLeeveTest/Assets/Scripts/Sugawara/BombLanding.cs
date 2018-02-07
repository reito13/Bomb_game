using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLanding : MonoBehaviour {

	public Transform bombLandTransform = null;
	[SerializeField] Transform playerTransform = null;

	[SerializeField] private Transform CheckUp = null;
	[SerializeField] private Transform Checkdown = null;

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
		bombLandTransform.position = bombLandTransform.position + (transform.forward * setY * speed);
		bombLandTransform.localPosition = new Vector3(bombLandTransform.localPosition.x,offsetY, bombLandTransform.localPosition.z);
		//bombLandTransform.position = bombLandTransform.position + (transform.forward * (playerTransform.position.y-startPlayerY) * speed);

	}

	public void SetBombLandingPosition(float y)
	{
		setY = y;
	}

	public float GetPower()
	{
		float distance = Vector3.Distance(bombLandTransform.position, playerTransform.position);
		float baseDistance = distance - 6.0f;
		float power = 250 + (baseDistance * 470/30);
		power = power + Mathf.Abs(15 - Mathf.Abs(baseDistance - 15)) * 5f;
		return power;
		//return Vector3.Distance(bombLandTransform.position, playerTransform.position);
	}

	public float GetDistance()
	{
		return Vector3.Distance(bombLandTransform.position, playerTransform.position);
	}


}
