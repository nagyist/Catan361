using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayerEntry : MonoBehaviour {

	public string PlayerName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerName == GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName) {
			transform.FindChild ("Toggle").GetComponent<Toggle> ().enabled = false;
		}

		transform.FindChild ("Text").GetComponent<Text> ().text = PlayerName;
	}
}
