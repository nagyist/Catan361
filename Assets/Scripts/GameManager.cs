using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : Singleton<GameManager> {
	public static GameObject LocalPlayer = null;
	public static List<GameObject> ConnectedPlayers = new List<GameObject> ();
	public static GameState CurrentGameState = null;
	public static int PlayerCount = 0;

	public GUIInterface gui;

	protected GameManager() {
		
	}

    void Awake () {
        gui = GetComponent<GUIInterface>();
		//currentTurn = GetComponent<GameTurn> ();
    }

	public void StartGame() {
		
	}

	public static void PlayerConnected(GameObject player) {
		player.GetComponent<GamePlayer> ().myName = "Player" + (++PlayerCount);
		ConnectedPlayers.Add (player);
	}

	public static void SetLocalPlayer(GameObject player) {
		LocalPlayer = player;
	}

    public void RollDice() {
        
    }

	public bool GameStateReady() {
		return CurrentGameState != null;
	}

	public GameState GetCurrentGameState() {
		return CurrentGameState;
	}

	public bool GameStateReadyAtStage(GameState.GameStatus status) {
		return GameStateReady () && GetCurrentGameState ().CurrentStatus >= status;
	}

	public bool CurrentPlayerTakeTurn() {
		if (!GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsLocalPlayerAllowedToTakeTurn()) {
			StartCoroutine (GameManager.Instance.gui.ShowMessage ("Cannot take turn"));
			return false;
		}

		LocalPlayer.GetComponent<GamePlayer> ().CmdTakeTurn ();
		return true;
	}

	public bool CurrentPlayerEndTurn() {
		if (!GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsLocalPlayerTurn ()) {
			StartCoroutine (GameManager.Instance.gui.ShowMessage ("Cannot end turn"));
			return false;
		}

		LocalPlayer.GetComponent<GamePlayer> ().CmdEndTurn ();
		return true;
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
