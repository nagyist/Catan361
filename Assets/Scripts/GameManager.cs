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
	public static GUIInterface GUI = null;
	public static int PlayerCount = 0;

    // note: players are added in two different data structures
    // the ConnectedPlayers List is a list containing all the game objects
    public static List<GameObject> ConnectedPlayers = new List<GameObject>();
    // the ConnectedPlayersByName is a dictionary linking all of the connected players to a given name: Player1, Player2, etc..
    public static Dictionary<string, GameObject> ConnectedPlayersByName = new Dictionary<string, GameObject>();

    protected GameManager() {
		
	}

    // awake function used to initialize variables before the game starts
    void Awake () {
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
			StartCoroutine (GameManager.GUI.ShowMessage ("Cannot take turn"));
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
			StartCoroutine (GameManager.Instance.GetComponent<GUIInterface>().ShowMessage ("Cannot end turn"));
			return false;
		}

        // calls the local player's cmd function to end their turn
		LocalPlayer.GetComponent<GamePlayer> ().CmdEndTurn ();
		return true;
	}

	public void RollEventDice(RollDiceScript.EventDiceOutcome outcome) {
		if (outcome == RollDiceScript.EventDiceOutcome.Barbarian) {
			GameManager.Instance.GetCurrentGameState ().CurrentBarbarianEvent.BarbarianCounter--;
			LocalPlayer.GetComponent<GamePlayer> ().CmdUpdateBarbarianEvent(SerializationUtils.ObjectToByteArray(GameManager.Instance.GetCurrentGameState().CurrentBarbarianEvent));

			if (GameManager.Instance.GetCurrentGameState ().CurrentBarbarianEvent.BarbarianInvasionTriggered ()) {
				GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdTriggerBarbarianInvasion ();
			} else {
				GameManager.Instance.GetCurrentGameState ().RpcClientShowMessage ("Barbarians are getting closer ...", 1.75f);
			}
		} else { // gates
			string gateColor = "";
			if (outcome == RollDiceScript.EventDiceOutcome.City_Gates_Blue) {
				gateColor = "blue";
			} else if (outcome == RollDiceScript.EventDiceOutcome.City_Gates_Green) {
				gateColor = "green";
			} else if (outcome == RollDiceScript.EventDiceOutcome.City_Gates_Red) {
				gateColor = "red";
			}

			GameManager.Instance.GetCurrentGameState().RpcClientPostStatusMessage("Event dice landed on " + gateColor + " gates");
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdTriggerGateEvent (GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName, SerializationUtils.ObjectToByteArray (outcome));
		}
	}

    // this function is used update player resources accordingly
    public bool RollDice(int roll) {
		// returns false if game isn't ready
        if (!GameManager.Instance.GameStateReadyAtStage (GameState.GameStatus.GRID_CREATED))
			return false;

		// roll the dice
		StartCoroutine (GameManager.GUI.ShowMessage("You rolled " + roll));
		GetCurrentGameState ().RpcClientPostStatusMessage (LocalPlayer.GetComponent<GamePlayer>().myName + " rolled " + roll);

		if (roll == 7 && GameManager.Instance.GetCurrentGameState().CurrentBarbarianEvent.BarbarianInvasionCounter > 0) {
			GameManager.GUI.ShowMoveRobberPiratePopup ();
			return true;
		}

		/* 
         * TODO:
		 *      Figure out edge colletion bug
		 *      Different color for edges/intersections
         *      Give the resources to all of the players (easy fix will do later)
		 * 
         */


		GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer> ();
        // go through all the intersections
		foreach (string key in GameManager.Instance.GetCurrentGameState().CurrentIntersections.Intersections.Keys) {
			Intersection i = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.Intersections [key];

            // check to see if it isn't empty
			if (i.unit != null && i.Owner != null)
            {
                // check to see if its a village
                if (i.unit.GetType() == typeof(Village))
                {
                    Village currentVillage = (Village)(i.unit);
                    int amountToAdd = 0;
                    if (currentVillage.myKind == Village.VillageKind.Settlement)
                        amountToAdd = 1;
                    else if (currentVillage.myKind == Village.VillageKind.City)
                        amountToAdd = 2;

					GamePlayer intersectionOwner = GameManager.ConnectedPlayersByName [i.Owner].GetComponent<GamePlayer>();

					List<Vec3> adjHexes = new List<Vec3>(new Vec3[] { i.adjTile1, i.adjTile2, i.adjTile3 });
					foreach (Vec3 hex in adjHexes)
					{
						HexTile tile = GameManager.Instance.GetCurrentGameState().CurrentBoard[hex];
						RobberPiratePlacement robberPlacement = GameManager.Instance.GetCurrentGameState ().CurrentRobberPosition;

						int newAmountResource = amountToAdd;
						int newAmountCommodity = 1;

						// continue if the tile number's don't match the rolled number
						// if (tile.IsWater) { continue; }
						if (tile.SelectedNum != roll && tile.SelectedNum2 != roll && tile.SelectedNum3 != roll && tile.SelectedNum4 != roll && tile.SelectedNum5 != roll) { continue; }

						ResourceCollection.PlayerResourcesCollection playerResources = intersectionOwner.GetPlayerResources ();

						if (robberPlacement.PlacementPos.Equals(hex)) 
						{ continue; }

						if (playerResources.ContainsKey (tile.Resource) && tile.Resource != StealableType.Resource_Gold) 
						{
							newAmountResource = playerResources [tile.Resource] + amountToAdd;

							if (intersectionOwner == localPlayer) 
							{
								intersectionOwner.gotNoResources = false;
							}
								
						} else if (playerResources.ContainsKey (tile.Resource) && tile.IsFishingGround == true) {
							newAmountResource = playerResources [tile.Resource] + tile.FishingReturnNum;

							if (intersectionOwner == localPlayer) 
							{
								intersectionOwner.gotNoResources = false;
							}
						} else if (tile.Resource == StealableType.Resource_Gold) {
							GameManager.GUI.ShowGoldPopup ();

							if (intersectionOwner == localPlayer) 
							{
								intersectionOwner.gotNoResources = false;
							}
						} 

						if (playerResources.ContainsKey (tile.Commodity) && tile.Commodity != StealableType.None && currentVillage.myKind == Village.VillageKind.City) {
							newAmountResource = playerResources [tile.Resource] + 1;
							newAmountCommodity = playerResources [tile.Commodity] + 1;
							intersectionOwner.CmdUpdateResource (tile.Resource, newAmountResource);
							intersectionOwner.CmdUpdateResource (tile.Commodity, newAmountCommodity);
							if (intersectionOwner == localPlayer) {
								intersectionOwner.gotNoResources = false;
							}
						} else {
							intersectionOwner.CmdUpdateResource (tile.Resource, newAmountResource);
						}
					}
                }
            }
		}

		foreach (GameObject player in ConnectedPlayers) {
			if (player.GetComponent<GamePlayer> ().gotNoResources && player.GetComponent<GamePlayer> ().hasAqueduct) 
			{
				//Player gets to pick a resource or commodity of their choice

			} 
			else 
			{
				player.GetComponent<GamePlayer> ().gotNoResources = true;
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
