using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

	[SerializeField] private SceneTransitionManager sceneManager = null;

	public void ButtonPush(int num)
	{
		MainManager.playerNum = num;
		sceneManager.SceneTransition();
	}

}
