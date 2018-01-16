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

		MoveInputSet();
		RotateInputSet();

		if (player.number == MainManager.playerNum) //このスクリプトをアタッチしたプレイヤーキャラの番号が操作キャラの番号であるとき
		{

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
		}
		
		if (bombSet)
			bombTime += Time.deltaTime;

		MoveInputGet();
		if (player.number == MainManager.playerNum)
		{
			NormalRotateInput();
		}
		else
		{
			RotateInputGet();
		}
	}

	private void MoveInputSet()
	{
		if (player.number == MainManager.playerNum)
		{
			inputController.MoveSet(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), player.number);
		}
	}

	private void RotateInputSet()
	{
		if (player.number == MainManager.playerNum)
		{
			inputController.RotateSet(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), player.number);
		}
	}

	private async void MoveInputGet()
	{
		player.SetMoveDir(await inputController.MoveGet("X", player.number), await inputController.MoveGet("Y", player.number));
	}

	private async void RotateInputGet()
	{
		cameraScript.SetRotate(await inputController.RotateGet("X",player.number), await inputController.RotateGet("Y", player.number));
	}

	private void NormalRotateInput()
	{
		cameraScript.SetRotate(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
	}
}
