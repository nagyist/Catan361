using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeButton : MonoBehaviour {

	private int PlayerIndex = 0;

	// Use this for initialization
	void Start () {
		this.PlayerIndex = transform.GetComponentInParent<PlayerResourcePanel> ().PlayerIndex;
		if (GameManager.ConnectedPlayers.IndexOf (GameManager.LocalPlayer) == this.PlayerIndex) {
			GetComponent<Button> ().enabled = false;
		}
	}

	private GamePlayer getPlayer() {
		return GameManager.ConnectedPlayers [this.PlayerIndex].GetComponent<GamePlayer> ();
	}

	void ClickRequestTrade() {
		GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer> ();
		TradeManager.Instance.InitiateTrade (localPlayer.myName, getPlayer ().myName);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
 