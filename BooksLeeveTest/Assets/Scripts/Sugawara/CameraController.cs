using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	public float xRotate;
	private float yRotate;

	[SerializeField] private float mouseSpeed = 0.0f;
	[SerializeField] private float cameraSpeed = 0.0f;

	[SerializeField] private float roteteLimitX = 0.0f;

	private Quaternion charaRotate;
	[SerializeField] private Transform charaTransform = null;
	private Quaternion cameraRotate;
	[SerializeField] private Transform cameraTransform = null;

	private void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		charaRotate = charaTransform.localRotation;
		cameraRotate = cameraTransform.localRotation;
	}
	public void SetRotate(float x, float y)
	{
		xRotate = x;
		yRotate = y;
	}

	public void RotatitonUpdate()
	{
		xRotate *= mouseSpeed * -1;
		yRotate *= mouseSpeed;
		//xRotate *= -1;
		charaRotate *= Quaternion.Euler(0f, yRotate, 0f);
		charaTransform.localRotation = Quaternion.Slerp(charaTransform.localRotation, charaRotate, cameraSpeed * Time.deltaTime);

		cameraRotate *= Quaternion.Euler(xRotate, 0f, 0f);
		cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, cameraRotate, cameraSpeed * Time.deltaTime);
	}
}
