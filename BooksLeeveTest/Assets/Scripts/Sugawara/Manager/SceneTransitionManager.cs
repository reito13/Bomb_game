using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : SingletonMonoBehaviour<SceneTransitionManager> {

	private enum SceneName
	{
		Title,Menu,OthersRoom,FriendsRoom,Main,Result
	}

	[SerializeField] private SceneName sceneName = SceneName.Title;

	public void SceneTransition()
	{
	SceneManager.LoadScene(sceneName.ToString());
	}

	public void SceneTransition(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
