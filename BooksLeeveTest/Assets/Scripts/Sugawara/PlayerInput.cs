using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

	[SerializeField] private Player player = null;
	[SerializeField] private CameraController cameraScript = null;

	private bool bombSet = false;
	private float bombTime = 0.0f;

	private InputController inputController;

	private void Awake()
	{
		inputController = GetComponent<InputController>();
	}

	private void Update()
	{
		if (!player.Control)
		{
			player.SetMoveDir(0,0);
			return;
		}

		MoveInput();
		RotateInput();

		if (player.number == MainManager.playerNum) //このスクリプトをアタッチしたプレイヤーキャラの番号が操作キャラの番号であるとき
		{
			if ((Input.GetButtonDown("Jump")) || (Input.GetButtonDown("R2")))
			{
				player.Jump();
				//inputController.JumpFlagSet(player.number);
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
		}
		
		if (bombSet)
			bombTime += Time.deltaTime;

		JumpInputGet();
	}

	private void MoveInput()
	{
		if (player.number == MainManager.playerNum)
		{
			player.SetMoveDir(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		}
	}

	private void RotateInput()
	{
		if (player.number == MainManager.playerNum)
		{
			cameraScript.SetRotate(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
		}
	}

	private async void JumpInputGet()
	{
		if (await inputController.JumpFlagGet(player.number))
		{
			player.Jump();
		}
	}
}
