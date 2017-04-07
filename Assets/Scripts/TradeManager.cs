using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager : Singleton<TradeManager> {
	public void SendTradeRequest(string fromPlayerName, string toPlayerName) {
		GamePlayer player1 = GameManager.ConnectedPlayersByName [fromPlayerName].GetComponent<GamePlayer> ();
		GamePlayer player2 = GameManager.ConnectedPlayersByName [toPlayerName].GetComponent<GamePlayer> ();

		Trade newTrade = new Trade ();
		newTrade.Player1 = player1.myName;
		newTrade.Player2 = player2.myName;

		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdSendTradeRequest (SerializationUtils.ObjectToByteArray (newTrade));
		GameManager.GUI.PostStatusMessage ("You sent a trade request to " + player2);
	}

	public void ReceivedAnswerForTradeRequest(Trade trade, bool answer) {
		if (!answer) {
			StartCoroutine (GameManager.GUI.ShowMessage (trade.Player2 + " declined your trade request."));
			GameManager.GUI.PostStatusMessage (trade.Player2 + " declined your trade request.");
			return;
		}

		GameManager.GUI.PostStatusMessage (trade.Player2 + " accepted your trade request.");
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
