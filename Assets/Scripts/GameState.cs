using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

/*
 * the GameState holds information about:
 *      1. the game's map (hexes, intersection, edges)
 *      2. the game's state (ready, grid created, game turn synchronized)
 *      3. the transmitter id used for networking
 *      4. the Rcp functions used for networking
 *          - an rpc function is called on all players
 *          - it is executed by the server, not the client
 *      5. synchronization functions for the game's board and player turns
 */ 


// serializable means that the object can be converted into a stream of bytes
// used for networking purposes
[Serializable]

// this class is used as the gameboard's sync message
// it holds information about the map
public class GameboardSyncMessage {
	public Dictionary<Vec3, HexTile> CurrentBoard;
	public EdgeCollection CurrentEdges;
	public IntersectionCollection CurrentIntersections;
}

// this class is used to define the game's state
public class GameState : NetworkBehaviour {
    
    // enum defining game status 
	public enum GameStatus {
		NOT_READY = -1,
		GRID_CREATED = 0,
		GAME_TURN_SYNC = 1
	}

    // holds the network transmitter's id
	public static int transId = 0;
	
    // variables to hold the game state's information
	public Dictionary<Vec3, HexTile> CurrentBoard;
	public EdgeCollection CurrentEdges;
	public IntersectionCollection CurrentIntersections;
    public GameTurn CurrentTurn = new GameTurn();
    public GameStatus CurrentStatus { get; private set; }

    // called once for initialization
    void Start() {
        // set the current status as not ready
		GameManager.CurrentGameState = this;
		CurrentStatus = GameStatus.NOT_READY;

        // get the network transmitter
		NetworkTransmitter networkTransmitter = GetComponent<NetworkTransmitter> ();

        // this block will only be executed by the server
		if (isServer)
        {
            // create the hex grid, and the UI hex grid
			GetComponent<HexGrid> ().CreateHexGrid (this);
			GetComponent<HexGrid> ().CreateUIHexGrid ();
			GetComponent<HexGrid> ().buildHarbours ();
			//GetComponent<HexGrid> ().buildFishingGroundsUI ();

            // set the game's status as grid created
			CurrentStatus = GameStatus.GRID_CREATED;

            // this will call game the SyncGameBoard function at the set interval
			InvokeRepeating ("SyncGameBoard", 2.0f, 300f);

            // synchronize the game turns and set the status
			SyncGameTurns ();
			CurrentStatus = GameStatus.GAME_TURN_SYNC;
		}
        // this block will only be executed by clients
        else if (isClient)
        {   
			networkTransmitter.OnDataCompletelyReceived += RpcGameBoardSynchronized;
		}
	}

    /* ClientRpc function calls are called by the server and run on clients
     * function names must have Rcp in their names
     * ClientRpc tag means funcions returns immediately if it is not a client
     * this function is used for game board synchronization
     */
    [ClientRpc]
	public void RpcGameBoardSynchronized(int transmissionId, byte[] boardSerialized) {

		Debug.Log ("Received client call rpcGameBoard");
        // create the sync message variable
        // holds the game's board is a serialized version
		GameboardSyncMessage syncMsg = SerializationUtils.ByteArrayToObject (boardSerialized) as GameboardSyncMessage;

        // set the client's map the values of the same values as the sync message
		CurrentBoard = syncMsg.CurrentBoard;
		CurrentEdges = syncMsg.CurrentEdges;
		CurrentIntersections = syncMsg.CurrentIntersections;

        // create the UI map based on teh values received
		GetComponent<HexGrid>().CreateUIHexGrid();
        // set the game status as grid created
		CurrentStatus = GameStatus.GRID_CREATED;
	}

    // this rcp function is to publish edges to clients
    [ClientRpc]
	public void RpcPublishEdge(byte[] vec3Serialized, byte[] newEdgeSerialized) {
        // get the edge position and edge game object
		Vec3[] edgePos = SerializationUtils.ByteArrayToObject (vec3Serialized) as Vec3[];
		Edge newEdge = SerializationUtils.ByteArrayToObject(newEdgeSerialized) as Edge;
        // add the edge to current edges
		CurrentEdges.setEdge (edgePos [0], edgePos [1], newEdge);
	}

    // this rcp function is used to push intersection in the same manner as edges
	[ClientRpc]
	public void RpcPublishIntersection(byte[] vec3Serialized, byte[] newIntersectionSerialized) {
        // get the intersection position and intersection game object
		Vec3[] intersectionPos = SerializationUtils.ByteArrayToObject (vec3Serialized) as Vec3[];
		Intersection newIntersection = SerializationUtils.ByteArrayToObject (newIntersectionSerialized) as Intersection;
        // add it to current intersections
		CurrentIntersections.setIntersection (intersectionPos, newIntersection);
	}

    // this rcp function is used to synchronize game turns across clients
	[ClientRpc]
	public void RpcSynchronizeGameTurns(byte[] gameTurnsSerialized) {
        // not sure if redundant as the ClientRpc tag is already there
		if (isServer) {
			return;
		}
        // set the current turn to the received arugment
		CurrentTurn = SerializationUtils.ByteArrayToObject (gameTurnsSerialized) as GameTurn;
	}
	
    // this rcp function is used when a player rolsl the dice
    // doesn't seem to do anything aside from creating a debug message for now 
    // review needed
	[ClientRpc]
	public void RpcClientRolledDice(GameObject player, int diceResult) {
		Debug.Log ("Player rolled " + diceResult);
	}

	[ClientRpc]
	public void RpcClientShowMessage(string msg, float delay) {
		StartCoroutine (GameManager.GUI.ShowMessage (msg, delay));
	}

	[ClientRpc]
	public void RpcClientPostStatusMessage(string msg) {
		GameManager.GUI.PostStatusMessage (msg);
	}

    // this function si used to sync the gameboard
	public void SyncGameBoard() {

        // create a new network transmitter and sync message
		NetworkTransmitter networkTransmitter = GetComponent<NetworkTransmitter> ();
		GameboardSyncMessage syncMsg = new GameboardSyncMessage ();

        // add the board, edges, and intersections to the sync message
		syncMsg.CurrentBoard = CurrentBoard;
		syncMsg.CurrentEdges = CurrentEdges;
		syncMsg.CurrentIntersections = CurrentIntersections;

        // send the message to the clients
		networkTransmitter.SendBytesToClients (++transId, SerializationUtils.ObjectToByteArray (syncMsg));
	}

    // this function is used to synchronize game turns
    // it calls its own rcp function with it's own turn as the argument
	public void SyncGameTurns() {
		RpcSynchronizeGameTurns (SerializationUtils.ObjectToByteArray(CurrentTurn));
	}

	// Update is called once per frame
	void Update () {
		
	}
}
