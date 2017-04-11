using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : Singleton<GameEventManager> {
	public bool IsEventMoveRobberPirateEntitySet = false;
	public string EventMoveRobberPirateEntityType = "";

	public bool IsEventBarbarianSet = false;
	public BarbarianInvasion CurrentBarbarianInvasion = null;

	public void HandleMoveRobberPirateDecision(string type) {
		if (IsEventMoveRobberPirateEntitySet) {
			return;
		}

		if (type == "robber") {
			EventMoveRobberPirateEntityType = "robber";
		} else if (type == "pirate") {
			EventMoveRobberPirateEntityType = "pirate";
		}

		this.IsEventMoveRobberPirateEntitySet = true;
		GameManager.GUI.PostStatusMessage ("Choose a tile to place the " + type + " on.");

	}

	public void HandleMoveRobberPirate(UIHex hex) {
		if (!IsEventMoveRobberPirateEntitySet) {
			return;
		}

		HexTile moveToHexTile = GameManager.Instance.GetCurrentGameState ().CurrentBoard [hex.HexGridCubePosition];
		if (EventMoveRobberPirateEntityType == "robber") {
			if (moveToHexTile.IsWater) {
				StartCoroutine (GameManager.GUI.ShowMessage ("You cannot place the robber on water tiles."));
				return;
			}
			else 
			{
				bool stolenRobber = false;
				foreach (string key in GameManager.Instance.GetCurrentGameState().CurrentIntersections.Intersections.Keys) {
					Intersection i = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.Intersections [key];
					if (i.unit != null && i.Owner != null && stolenRobber == false) 
					{
						if (i.unit.GetType () == typeof(Village)) 
						{
							if (GameManager.Instance.GetCurrentGameState ().CurrentBoard [i.adjTile1] == moveToHexTile || 
								GameManager.Instance.GetCurrentGameState ().CurrentBoard [i.adjTile2] == moveToHexTile || 
								GameManager.Instance.GetCurrentGameState ().CurrentBoard [i.adjTile3] == moveToHexTile) 
							{
								GamePlayer intersectionOwner = GameManager.ConnectedPlayersByName [i.Owner].GetComponent<GamePlayer>();
								GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer> ();
								if (intersectionOwner != localPlayer) 
								{
									ResourceCollection.PlayerResourcesCollection intersectionOwnerResources = intersectionOwner.GetPlayerResources ();
									ResourceCollection.PlayerResourcesCollection localPlayerResources = localPlayer.GetPlayerResources ();
									Dictionary <int, StealableType> resourceDictForRandomNum = new Dictionary <int, StealableType> () 
									{
										{ 1, StealableType.Resource_Brick },
										{ 2, StealableType.Resource_Grain },
										{ 3, StealableType.Resource_Lumber },
										{ 4, StealableType.Resource_Ore },
										{ 5, StealableType.Resource_Wool }
									};

									Dictionary <int, string> resourceReturnDict = new Dictionary <int, string> () 
									{
										{ 1, "Brick" },
										{ 2, "Grain" },
										{ 3, "Lumber" },
										{ 4, "Ore" },
										{ 5, "Wool" }
									};

									int randKey = Random.Range (1, 6);
									StealableType keyToSteal = resourceDictForRandomNum [randKey];
									intersectionOwnerResources [keyToSteal] --;
									localPlayerResources [keyToSteal] ++;
									stolenRobber = true;
									StartCoroutine(GameManager.GUI.ShowMessage ("You stole: " + resourceReturnDict[randKey] + " from " + i.Owner));
								}
							}
						}
					}
				}
			}
		} else if (EventMoveRobberPirateEntityType == "pirate") {
			if (!moveToHexTile.IsWater) {
				StartCoroutine (GameManager.GUI.ShowMessage ("You cannot place the pirate on land tiles."));
				return;
			}
			else 
			{
				bool stolenPirate = false;
				foreach (string key in GameManager.Instance.GetCurrentGameState().CurrentEdges.Edges.Keys) {
					Edge e = GameManager.Instance.GetCurrentGameState ().CurrentEdges.Edges [key];
					if (e.IsOwned != false && e.Owner != null && stolenPirate == false) 
					{
							if (GameManager.Instance.GetCurrentGameState ().CurrentBoard [e.adjTile1] == moveToHexTile || 
								GameManager.Instance.GetCurrentGameState ().CurrentBoard [e.adjTile2] == moveToHexTile) 
							{
								GamePlayer edgeOwner = GameManager.ConnectedPlayersByName [e.Owner].GetComponent<GamePlayer>();
								GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer> ();
								if (edgeOwner != localPlayer) 
								{
									ResourceCollection.PlayerResourcesCollection edgeOwnerResources = edgeOwner.GetPlayerResources ();
									ResourceCollection.PlayerResourcesCollection localPlayerResources = localPlayer.GetPlayerResources ();
									Dictionary <int, StealableType> resourceDictForRandomNum = new Dictionary <int, StealableType> () 
									{
										{ 1, StealableType.Resource_Brick },
										{ 2, StealableType.Resource_Grain },
										{ 3, StealableType.Resource_Lumber },
										{ 4, StealableType.Resource_Ore },
										{ 5, StealableType.Resource_Wool }
									};

									Dictionary <int, string> resourceReturnDict = new Dictionary <int, string> () 
									{
										{ 1, "Brick" },
										{ 2, "Grain" },
										{ 3, "Lumber" },
										{ 4, "Ore" },
										{ 5, "Wool" }
									};

									int randKey = Random.Range (1, 6);
									StealableType keyToSteal = resourceDictForRandomNum [randKey];
									edgeOwnerResources [keyToSteal] --;
									localPlayerResources [keyToSteal] ++;
									stolenPirate = true;
									StartCoroutine(GameManager.GUI.ShowMessage ("You stole: " + resourceReturnDict[randKey] + " from " + e.Owner));
								}
							}
						}
					}
				}
			}


		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdHandleMoveRobberPirateEntity (EventMoveRobberPirateEntityType, SerializationUtils.ObjectToByteArray(hex.HexGridCubePosition));
		IsEventMoveRobberPirateEntitySet = false;
	}

	public void TriggerNewBarbarianInvasion() {
		GameManager.Instance.GetCurrentGameState ().RpcClientPostStatusMessage ("BARBARIANS INVADED CATAN !");
		CurrentBarbarianInvasion = new BarbarianInvasion();
		CurrentBarbarianInvasion.ExecutePrimaryOutcome ();

		GameManager.Instance.GetCurrentGameState ().RpcClientPublishBarbarianInvasion (SerializationUtils.ObjectToByteArray (CurrentBarbarianInvasion));
	}

	public void TriggerNewGateEvent(RollDiceScript.EventDiceOutcome outcome) {
		GateEvent newGateEvent = new GateEvent (outcome);
		GameManager.Instance.GetCurrentGameState ().RpcClientPublishGateEvent (SerializationUtils.ObjectToByteArray (newGateEvent));
	}

	/*
	public void TriggerGoldPopup() {
		GameManager.Instance.GetCurrentGameState ().RpcClientPublishGateEvent (SerializationUtils.ObjectToByteArray (newGateEvent));
	}
	*/
}
