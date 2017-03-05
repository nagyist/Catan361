using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    public HexGrid gameBoard;
	public GamePlayer currentPlayer;
	public GUIInterface gui;
	private int roundNo = 0;

	protected GameManager() { }

    void Awake () {
        gameBoard = GetComponent<HexGrid>();
        currentPlayer = GetComponent<GamePlayer>();
        gui = GetComponent<GUIInterface>();
    }

    public void RollDice() {
        int diceResult = Random.Range(1, 6);
        StartCoroutine(gui.ShowMessage("Player X rolled " + diceResult));
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
