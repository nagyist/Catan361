using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/* this class is used to manage:
 *      1. all of the GamePlayer game objects
 *      2. the current game's state
 *      3. the rolldice function
 */

public class GameManager : Singleton<GameManager> {

    // initialize some variables
	public static GameObject LocalPlayer = null;
	public static GameState CurrentGameState = null;
	public static int PlayerCount = 0;
	public GUIInterface gui;

    // note: players are added in two different data structures
    // the ConnectedPlayers List is a list containing all the game objects
    public static List<GameObject> ConnectedPlayers = new List<GameObject>();
    // the ConnectedPlayersByName is a dictionary linking all of the connected players to a given name: Player1, Player2, etc..
    public static Dictionary<string, GameObject> ConnectedPlayersByName = new Dictionary<string, GameObject>();

    protected GameManager() {
		
	}

    // awake function used to initialize variables before the game starts
    void Awake () {
        gui = GetComponent<GUIInterface>();
    }

    // this function will add the player argument to the GameManager's data structures
	public static void PlayerConnected(GameObject player) {
		player.GetComponent<GamePlayer> ().myName = "Player" + (++PlayerCount);
		ConnectedPlayers.Add (player);
		ConnectedPlayersByName.Add (player.GetComponent<GamePlayer> ().myName, player);
	}

    // this function sets the argument as the GameManager's local player 
	public static void SetLocalPlayer(GameObject player) {
		LocalPlayer = player;
	}

    public void RollDice() {
        
    }

    // this function returns true is the current game's state isn't null
	public bool GameStateReady() {
		return CurrentGameState != null;
	}

    // returns the current game's state
	public GameState GetCurrentGameState() {
		return CurrentGameState;
	}

    // returns true is the game state is ready for the stage defined by the status argument
	public bool GameStateReadyAtStage(GameState.GameStatus status) {
		return GameStateReady () && GetCurrentGameState ().CurrentStatus >= status;
	}

    // this function is used when a player wishes to take their turn
	public bool CurrentPlayerTakeTurn() {
        // if the player isn't allowed to take their turn, return false
		if (!GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsLocalPlayerAllowedToTakeTurn()) {
			StartCoroutine (GameManager.Instance.gui.ShowMessage ("Cannot take turn"));
			return false;
		}

        // if the player can take their turn, they can place a road and a settlment
		if (GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsInSetupPhase ()) {
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().placedRoad = false;
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().placedSettlement = false;
		} 

        // tells the local player to take their turn 
		LocalPlayer.GetComponent<GamePlayer> ().CmdTakeTurn ();
		return true;
	}

    // this function is used when a player wishes to end their turn
	public bool CurrentPlayerEndTurn() {

        // if the player isn't currently taking their turn, show error and return false
		if (!GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsLocalPlayerTurn ()) {
			StartCoroutine (GameManager.Instance.gui.ShowMessage ("Cannot end turn"));
			return false;
		}

        // calls the local player's cmd function to end their turn
		LocalPlayer.GetComponent<GamePlayer> ().CmdEndTurn ();
		return true;
	}

    // this function is used update player resources accordingly
    public bool RollDice(int roll) {

        GamePlayer player = LocalPlayer.GetComponent<GamePlayer>();

        // returns false if game isn't ready
        if (!GameManager.Instance.GameStateReadyAtStage (GameState.GameStatus.GRID_CREATED))
			return false;

		/* 
         * TODO:
		 *      Figure out edge colletion bug
		 *      Different color for edges/intersections
         *      Give the resources to all of the players (easy fix will do later)
		 * 
         */

        // go through all the intersections
		foreach (string key in GameManager.Instance.GetCurrentGameState().CurrentIntersections.Intersections.Keys) {
			Intersection i = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.Intersections [key];

            // if the intetersecion has a settlement and that settlement is owned by teh local player
			if (i.SettlementLevel > 0 && i.SettlementOwner == player.myName) {
                // go through all the adjacent hexes
				List<Vec3> adjHexes = new List<Vec3> (new Vec3[] { i.adjTile1, i.adjTile2, i.adjTile3 });
				foreach (Vec3 hex in adjHexes) {
					HexTile tile = GameManager.Instance.GetCurrentGameState ().CurrentBoard [hex];

                    // continue if the tile number's don't match the rolled number
					if (tile.IsWater) { continue; }
					if (tile.SelectedNum != roll) { continue; }

                    // to to see if the player's resrouces already contain the key for the resource
                    // if so then add the appropriate amount of resources
					if (player.playerResources.ContainsKey (tile.Resource))
                    {
						player.playerResources [tile.Resource] = player.playerResources [tile.Resource] + i.SettlementLevel;
					}
                    // if the player's resource don't already contain the key for the resource then add it
                    else
                    {
						player.playerResources.Add (tile.Resource, i.SettlementLevel);
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
