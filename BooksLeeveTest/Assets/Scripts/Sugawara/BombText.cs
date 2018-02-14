using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombText : MonoBehaviour {

	[SerializeField] private Text bombText = null;
	[SerializeField] private GameObject[] bombSprits = null;
	private int save = 0;

	public void TextUpdate(string str,int num) {
		bombText.text = str;
		if (num == -1)
		{
			bombSprits[save].SetActive(false);
		}
		else {
			save = num;
			bombSprits[num].SetActive(true);
		}
		
	}
}
