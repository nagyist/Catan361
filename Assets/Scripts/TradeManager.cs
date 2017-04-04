using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager : Singleton<TradeManager> {
	public Trade InitiateTrade(string fromPlayerName, string toPlayerName) {
		GamePlayer player1 = GameManager.ConnectedPlayersByName [fromPlayerName].GetComponent<GamePlayer> ();
		GamePlayer player2 = GameManager.ConnectedPlayersByName [toPlayerName].GetComponent<GamePlayer> ();

		Trade newTrade = new Trade ();
		newTrade.Player1 = player1.myName;
		newTrade.Player2 = player2.myName;

		GameManager.Instance.GetCurrentGameState ().RpcClientTradeRequest (SerializationUtils.ObjectToByteArray(newTrade));

		return newTrade;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
