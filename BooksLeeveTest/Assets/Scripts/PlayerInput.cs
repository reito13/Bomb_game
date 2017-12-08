using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	[SerializeField] private Player player = null;

	private bool bombSet = false;
	private float bombTime = 0.0f;

	private void Update()
	{
		if (!player.Control)
		{
			player.SetMoveDir(0,0);
			return;
		}

		player.SetMoveDir(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
		player.SetRotate(Input.GetAxis("RotateVertical"),Input.GetAxis("RotateHorizontal"));

		if (Input.GetButtonDown("Jump"))
		{
			player.Jump();
		}

		if (Input.GetKeyDown(KeyCode.Z))
		{
			bombSet = true;
		}
		if (Input.GetKeyUp(KeyCode.Z))
		{
			player.Bomb(bombTime);
			bombSet = false;
			bombTime = 0.0f;
		}

		if (bombSet)
			bombTime += Time.deltaTime;
	}
}
