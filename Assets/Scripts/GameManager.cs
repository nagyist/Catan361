using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
	const int MAX_PLAYER = 4;

	public int currentPlayer = 0;
	public GamePlayer[] players = new GamePlayer[MAX_PLAYER];
	public HexGrid gameBoard;
	public GUIInterface gui;
	public GameTurn currentTurn { get; private set; }

	protected GameManager() { }

    void Awake () {
        gameBoard = GetComponent<HexGrid>();
		players[currentPlayer] = GetComponent<GamePlayer>();
        gui = GetComponent<GUIInterface>();
		currentTurn = new GameTurn (MAX_PLAYER);
    }

    public void RollDice() {
        int diceResult = Random.Range(1, 6);
        StartCoroutine(gui.ShowMessage("Player X rolled " + diceResult));
    }

	public bool isInSetupPhase() {
		return true;
	}

	public void currentPlayerTakeTurn() {
		if (!currentTurn.TakeTurn ()) { return; }

		StartCoroutine (gui.ShowMessage ("Player " + currentPlayer + " took turn!"));
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
