using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : Singleton<GameManager> {
	const int MAX_PLAYER = 4;

	public GameTurn currentTurn;
	public GUIInterface gui;

	protected GameManager() {
		//connectedPlayers = new List<GameObject> ();
	}

    void Awake () {
        gui = GetComponent<GUIInterface>();
		//currentTurn = GetComponent<GameTurn> ();
    }

	public void StartGame() {
		
	}

	public void PlayerConnected(GameObject player) {
		//connectedPlayers.Add (player);
	}

	public void SetLocalPlayer(GameObject player) {
		//localPlayer = player;
	}

	public int GetPlayerIndex(GameObject player) {
		return 0;
	}

	public int GetLocalPlayerIndex() {
		return 0;
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
		//if (!currentTurn.LocalPlayerTakeTurn()) { return; }
	}

	public void currentPlayerEndTurn() {
		//if (!currentTurn.LocalPlayerEndTurn()) { return; }
	}

	public bool GameStateReady() {
		GameObject gameStateObj = GameObject.FindGameObjectWithTag ("GameState");
		if (gameStateObj == null) { return false; }
		GameState gameState = gameStateObj.GetComponent<GameState> ();
		if (gameState == null) { return false; }
		return true;
	}

	public GameState GetCurrentGameState() {
		return GameObject.FindGameObjectWithTag ("GameState").GetComponent<GameState>();
	}

	void Start () {
		//currentTurn.MaximumPlayer = connectedPlayers.Count;
		//currentTurn.MaximumPlayer = 2;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
