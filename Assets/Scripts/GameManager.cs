using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : Singleton<GameManager> {
	const int MAX_PLAYER = 4;
	public static GameManager Instance = new GameManager ();

	public int MockedCurrentPlayer = 0;
	public GameObject localPlayer { get; private set; }
	public List<GameObject> connectedPlayers { get; private set; }
	public HexGrid gameBoard;
	public GUIInterface gui;
	public GameTurn currentTurn { get; private set; }

	protected GameManager() {
		connectedPlayers = new List<GameObject> ();
	}

    void Awake () {
        gameBoard = GetComponent<HexGrid>();
        gui = GetComponent<GUIInterface>();
		currentTurn = GetComponent<GameTurn> ();
    }

	public void PlayerConnected(GameObject player) {
		connectedPlayers.Add (player);
	}

	public void SetLocalPlayer(GameObject player) {
		localPlayer = player;
	}

	public int GetPlayerIndex(GameObject player) {
		return connectedPlayers.IndexOf (player);
	}

	public int GetLocalPlayerIndex() {
		return MockedCurrentPlayer;
		//return GetPlayerIndex (localPlayer);
	}

    public void RollDice() {
        int diceResult = Random.Range(1, 15);
        StartCoroutine(gui.ShowMessage("Player X rolled " + diceResult));
    }

	public bool isInSetupPhase() {
		return true;
	}

	public void currentPlayerTakeTurn() {
		if (!currentTurn.LocalPlayerTakeTurn()) { return; }
	}

	public void currentPlayerEndTurn() {
		if (!currentTurn.LocalPlayerEndTurn()) { return; }
	}

	void Start () {
		currentTurn.MaximumPlayer = connectedPlayers.Count;
		//currentTurn.MaximumPlayer = 2;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
