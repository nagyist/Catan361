using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager : Singleton<TradeManager> {
	private GameObject currentTrade = null;

	public void SendTradeRequest(string fromPlayerName, string toPlayerName) {
		GamePlayer player1 = GameManager.ConnectedPlayersByName [fromPlayerName].GetComponent<GamePlayer> ();
		GamePlayer player2 = GameManager.ConnectedPlayersByName [toPlayerName].GetComponent<GamePlayer> ();

		Trade newTrade = new Trade ();
		newTrade.Player1 = player1.myName;
		newTrade.Player2 = player2.myName;

		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdSendTradeRequest (SerializationUtils.ObjectToByteArray (newTrade));
		GameManager.GUI.PostStatusMessage ("You sent a trade request to " + player2.myName);
	}

	public void ReceivedAnswerForTradeRequest(Trade trade, bool answer) {
		if (!answer) {
			if (GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName == trade.Player1) {
				StartCoroutine (GameManager.GUI.ShowMessage (trade.Player2 + " declined your trade request."));
				GameManager.GUI.PostStatusMessage (trade.Player2 + " declined your trade request.");
			}

			return;
		}

		if (GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName == trade.Player1) {
			GameManager.GUI.PostStatusMessage (trade.Player2 + " accepted your trade request.");
		} else {
			GameManager.GUI.PostStatusMessage ("Trade with " + trade.Player1 + " started");
		}
		currentTrade = GameManager.GUI.ShowPlayer2PlayerTradeWindow (trade);
	}

	public void ReceivedTradeUpdate(Trade updatedTrade) {
		currentTrade.GetComponent<Player2PlayerTrade> ().currentTrade = updatedTrade;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
