using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : Singleton<GameManager> {
	public static GameObject LocalPlayer = null;
	public static List<GameObject> ConnectedPlayers = new List<GameObject> ();
	public static Dictionary<string, GameObject> ConnectedPlayersByName = new Dictionary<string, GameObject> ();
	public static GameState CurrentGameState = null;
	public static int PlayerCount = 0;

	public GUIInterface gui;

	protected GameManager() {
		
	}

    void Awake () {
        gui = GetComponent<GUIInterface>();
    }

	public static void PlayerConnected(GameObject player) {
		player.GetComponent<GamePlayer> ().myName = "Player" + (++PlayerCount);
		ConnectedPlayers.Add (player);
		ConnectedPlayersByName.Add (player.GetComponent<GamePlayer> ().myName, player);
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

		if (GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsInSetupPhase ()) {
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().placedRoad = false;
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().placedRoad = false;
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

	public bool RollDice(int roll) {
		if (!GameManager.Instance.GameStateReadyAtStage (GameState.GameStatus.GRID_CREATED)) {
			return false;
		}

		// update player resources accordingly
		GamePlayer player = LocalPlayer.GetComponent<GamePlayer> ();
		foreach (string key in GameManager.Instance.GetCurrentGameState().CurrentIntersections.Intersections.Keys) {
			Intersection i = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.Intersections [key];
			if (i.SettlementCount > 0 && i.SettlementOwner == player.myName) {
				List<Vec3> adjHexes = new List<Vec3> (new Vec3[] { i.adjTile1, i.adjTile2, i.adjTile3 });
				foreach (Vec3 hex in adjHexes) {
					HexTile tile = GameManager.Instance.GetCurrentGameState ().CurrentBoard [hex];
					if (tile.IsWater) { continue; }
					if (tile.SelectedNum != roll) { continue; }

					if (player.playerResources.ContainsKey (tile.Resource)) {
						player.playerResources [tile.Resource] = player.playerResources [tile.Resource] + i.SettlementCount;
					} else {
						player.playerResources.Add (tile.Resource, i.SettlementCount);
					}
				}
			}
		}

		return true;
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
