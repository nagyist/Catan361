using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressCardButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void ClickOpenProgressCard() {
		string playerName = GetComponentInParent<PlayerResourcePanel> ().PlayerName;
		GameManager.GUI.ShowProgressCardWindowOfUser (playerName);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
