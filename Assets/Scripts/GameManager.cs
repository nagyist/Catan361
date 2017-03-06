using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
	const int CURRENT_PLAYER = 0;
	const int MAX_PLAYER = 4;

	public GamePlayer[] players = new GamePlayer[MAX_PLAYER];
	public HexGrid gameBoard;
	public GUIInterface gui;
	public int currentPlayerTurn = -1;
	public int nextPlayerTurn = CURRENT_PLAYER;
	public int roundNo = 0;

	protected GameManager() { }

    void Awake () {
        gameBoard = GetComponent<HexGrid>();
		players[CURRENT_PLAYER] = GetComponent<GamePlayer>();
        gui = GetComponent<GUIInterface>();
    }

    public void RollDice() {
        int diceResult = Random.Range(1, 6);
        StartCoroutine(gui.ShowMessage("Player X rolled " + diceResult));
    }

	public bool isInSetupPhase() {
		return this.roundNo == 0;
	}

	public int startNextPlayerTurn() {
		currentPlayerTurn = nextPlayerTurn;
		nextPlayerTurn = nextPlayerTurn + 1 % MAX_PLAYER;
		StartCoroutine(gui.ShowMessage("Starting turn for player " + currentPlayerTurn));
		return currentPlayerTurn;
	}

	public bool isCurrentPlayerTurn() {
		return currentPlayerTurn == CURRENT_PLAYER;
	}

	void Start () {
		startNextPlayerTurn ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
