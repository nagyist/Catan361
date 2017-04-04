using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int currentPlayerIndex = transform.GetComponentInParent<PlayerResourcePanel> ().PlayerIndex;
		if (GameManager.ConnectedPlayers.IndexOf (GameManager.LocalPlayer) == currentPlayerIndex) {
			GetComponent<Button> ().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
