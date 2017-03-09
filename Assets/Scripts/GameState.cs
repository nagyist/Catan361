using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

[Serializable]
public class GameboardSyncMessage {
	public Dictionary<Vec3, HexTile> CurrentBoard;
	public EdgeCollection CurrentEdges;
	public IntersectionCollection CurrentIntersections;
}

public class GameState : NetworkBehaviour {
	public enum GameStatus {
		NOT_READY = -1,
		GRID_CREATED = 0,
		GAME_TURN_SYNC = 1
	}

	public static int transId = 0;
	public GameStatus CurrentStatus { get; private set; }
	public Dictionary<Vec3, HexTile> CurrentBoard;
	public GameTurn CurrentTurn = new GameTurn();
	public EdgeCollection CurrentEdges;
	public IntersectionCollection CurrentIntersections;

	void Start() {
		GameManager.CurrentGameState = this;
		CurrentStatus = GameStatus.NOT_READY;

		NetworkTransmitter networkTransmitter = GetComponent<NetworkTransmitter> ();

		if (isServer) {
			GetComponent<HexGrid> ().CreateHexGrid (this);
			GetComponent<HexGrid> ().CreateUIHexGrid ();
			CurrentStatus = GameStatus.GRID_CREATED;

			InvokeRepeating ("SyncGameBoard", 2.0f, 60.0f);

			SyncGameTurns ();
			CurrentStatus = GameStatus.GAME_TURN_SYNC;
		} else if (isClient) {
			networkTransmitter.OnDataCompletelyReceived += ClientRpcGameBoardSynchronized;
		}
	}

	[Client]
	public void ClientRpcGameBoardSynchronized(int transmissionId, byte[] boardSerialized) {
		Debug.Log ("Received client call rpcGameBoard");
		GameboardSyncMessage syncMsg = SerializationUtils.ByteArrayToObject (boardSerialized) as GameboardSyncMessage;

		CurrentBoard = syncMsg.CurrentBoard;
		CurrentEdges = syncMsg.CurrentEdges;
		CurrentIntersections = syncMsg.CurrentIntersections;

		GetComponent<HexGrid>().CreateUIHexGrid();
		CurrentStatus = GameStatus.GRID_CREATED;
	}

	[ClientRpc]
	public void RpcSynchronizeGameTurns(byte[] gameTurnsSerialized) {
		if (isServer) {
			return;
		}

		CurrentTurn = SerializationUtils.ByteArrayToObject (gameTurnsSerialized) as GameTurn;
	}
	
	[ClientRpc]
	public void RpcClientRolledDice(GameObject player, int diceResult) {
		Debug.Log ("Player rolled " + diceResult);
	}

	public void SyncGameBoard() {
		NetworkTransmitter networkTransmitter = GetComponent<NetworkTransmitter> ();
		GameboardSyncMessage syncMsg = new GameboardSyncMessage ();
		syncMsg.CurrentBoard = CurrentBoard;
		syncMsg.CurrentEdges = CurrentEdges;
		syncMsg.CurrentIntersections = CurrentIntersections;

		networkTransmitter.SendBytesToClients (++transId, SerializationUtils.ObjectToByteArray (syncMsg));
	}

	public void SyncGameTurns() {
		RpcSynchronizeGameTurns (SerializationUtils.ObjectToByteArray(CurrentTurn));
	}

	// Update is called once per frame
	void Update () {
		
	}
}
