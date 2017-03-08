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
		GRID_CREATED = 0
	}

	[SyncVar]
	public bool serverReady = false;

	public GameStatus CurrentStatus { get; private set; }
	public Dictionary<Vec3, HexTile> CurrentBoard;
	public GameTurn CurrentTurn = new GameTurn();
	public EdgeCollection CurrentEdges;
	public IntersectionCollection CurrentIntersections;



	void Start() {
		NetworkTransmitter networkTransmitter = GetComponent<NetworkTransmitter> ();
		networkTransmitter.OnDataCompletelyReceived += ClientRpcGameBoardSynchronized;

		CurrentStatus = GameStatus.NOT_READY;

		if (isServer) {
			GetComponent<HexGrid>().CreateHexGrid(this);
			GetComponent<HexGrid>().CreateUIHexGrid();

			GameboardSyncMessage syncMsg = new GameboardSyncMessage ();
			syncMsg.CurrentBoard = CurrentBoard;
			syncMsg.CurrentEdges = CurrentEdges;
			syncMsg.CurrentIntersections = CurrentIntersections;

			StartCoroutine(networkTransmitter.SendBytesToClientsRoutine(0, SerializationUtils.ObjectToByteArray(syncMsg)));
			CurrentStatus = GameStatus.GRID_CREATED;
		}
	}

	[Client]
	public void ClientRpcGameBoardSynchronized(int transmissionId, byte[] boardSerialized) {
		GameboardSyncMessage syncMsg = SerializationUtils.ByteArrayToObject (boardSerialized) as GameboardSyncMessage;

		CurrentBoard = syncMsg.CurrentBoard;
		CurrentEdges = syncMsg.CurrentEdges;
		CurrentIntersections = syncMsg.CurrentIntersections;
		GetComponent<HexGrid>().CreateUIHexGrid();
		CurrentStatus = GameStatus.GRID_CREATED;
	}



	// Update is called once per frame
	void Update () {
		
	}
}
