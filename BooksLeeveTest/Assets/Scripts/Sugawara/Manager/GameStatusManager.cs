﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatusManager : SingletonMonoBehaviour<GameStatusManager> {

	private bool pause = false;
	public bool Pause
	{
		get
		{
			return pause;
		}
		set
		{
			pause = value;
		}
	}

	private bool gameStart = true;
	public bool GameStart
	{
		get
		{
			return gameStart;
		}
		set
		{
			gameStart = value;
		}
	}

	private bool gameEnd = false;
	public bool GameEnd
	{
		get
		{
			return gameEnd;
		}
		set
		{
			gameEnd = value;
		}
	}

	public bool NormalState
	{
		get
		{
			if (pause || gameStart || gameEnd)
				return false;
			else
				return true;
		}
	}
}
