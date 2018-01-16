using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	[SerializeField] private Player player = null;
	[SerializeField] private CameraController cameraScript = null;

	private bool bombSet = false;
	private float bombTime = 0.0f;

	private void Awake()
	{
		if(player.number != MainManager.playerNum)
		{
			this.enabled = false;
		}
	}

	private void Update()
	{
		if (!player.Control)
		{
			player.SetMoveDir(0,0);
			return;
		}

		player.SetMoveDir(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
		//player.SetRotate(Input.GetAxis("RotateVertical"),Input.GetAxis("RotateHorizontal"));
		cameraScript.SetRotate(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

		if ((Input.GetButtonDown("Jump")) || (Input.GetButtonDown("R2")))
		{
			player.Jump();
		}

		if ((Input.GetKeyDown(KeyCode.Z)) || (Input.GetButtonDown("R1") || Input.GetMouseButtonDown(0)))
		{
			bombSet = true;
		}
		if ((Input.GetKeyUp(KeyCode.Z)) || (Input.GetButtonUp("R1") || Input.GetMouseButtonUp(0)))
        {
			player.Bomb(bombTime);
			bombSet = false;
			bombTime = 0.0f;
		}

		if (bombSet)
			bombTime += Time.deltaTime;
	}
}
