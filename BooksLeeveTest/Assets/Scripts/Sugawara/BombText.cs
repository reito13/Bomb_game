using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombText : MonoBehaviour {

	[SerializeField] private Text bombText = null;

	public void TextUpdate(string str) {
		bombText.text = str;
	}
}
