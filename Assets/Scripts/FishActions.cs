using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishActions : MonoBehaviour {

	public Vec3 HexPos1 { get; set; }
	public Vec3 HexPos2 { get; set; }
	public bool IsSelected = false; 

	public void showFishMenu ()
	{
		GameManager.GUI.ShowFishPopup ();
	}

	public void accepted()
	{
		ToggleGroup toggles = gameObject.GetComponentInChildren<ToggleGroup>();
		IEnumerator<Toggle> togglesEnum = toggles.ActiveToggles().GetEnumerator();
		togglesEnum.MoveNext();
		Toggle toggle = togglesEnum.Current;
		string actionSelected = toggle.transform.parent.name;
		string entityType = "";
		GamePlayer player = GameManager.LocalPlayer.GetComponent<GamePlayer> ();
		ResourceCollection.PlayerResourcesCollection playerResources = player.GetPlayerResources ();


		if (actionSelected.Equals("MoveRobber") && playerResources[StealableType.Resource_Fish] >= 2)
		{
			playerResources [StealableType.Resource_Fish] = playerResources [StealableType.Resource_Fish] - 2;
			entityType = "robber";
			GameEventManager.Instance.HandleMoveRobberPirateDecision (entityType);
			GameManager.GUI.HideFishPopup ();
		}
		else if (actionSelected.Equals("MovePirate") && playerResources[StealableType.Resource_Fish] >= 2)
		{
			playerResources [StealableType.Resource_Fish] = playerResources [StealableType.Resource_Fish] - 2;
			entityType = "pirate";
			GameEventManager.Instance.HandleMoveRobberPirateDecision (entityType);
			GameManager.GUI.HideFishPopup ();
		}

		else if (actionSelected.Equals("StealResource"))
		{
			//TODO: mimick the stealing of resources from robber - player to steal from is completely random
			bool stolenRobber = false;
			foreach (GameObject playerToSteal in GameManager.ConnectedPlayers) {
				if (!stolenRobber) {
					GamePlayer instancePlayerToSteal = playerToSteal.GetComponent<GamePlayer> ();
					GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer> ();
					if (instancePlayerToSteal != localPlayer) {
						ResourceCollection.PlayerResourcesCollection playerToStealResources = instancePlayerToSteal.GetPlayerResources ();
						ResourceCollection.PlayerResourcesCollection localPlayerResources = localPlayer.GetPlayerResources ();
						Dictionary <int, StealableType> resourceDictForRandomNum = new Dictionary <int, StealableType> () {
							{ 1, StealableType.Resource_Brick },
							{ 2, StealableType.Resource_Grain },
							{ 3, StealableType.Resource_Lumber },
							{ 4, StealableType.Resource_Ore },
							{ 5, StealableType.Resource_Wool }
						};

						Dictionary <int, string> resourceReturnDict = new Dictionary <int, string> () {
							{ 1, "Brick" },
							{ 2, "Grain" },
							{ 3, "Lumber" },
							{ 4, "Ore" },
							{ 5, "Wool" }
						};

						int randKey = Random.Range (1, 6);
						StealableType keyToSteal = resourceDictForRandomNum [randKey];
						playerToStealResources [keyToSteal]--;
						localPlayerResources [keyToSteal]++;
						stolenRobber = true;
						GameManager.GUI.HideFishPopup ();
						StartCoroutine (GameManager.GUI.ShowMessage ("You stole: " + resourceReturnDict [randKey] + " from " + instancePlayerToSteal.myName));
					}
				}
			}
		}

		else if (actionSelected.Equals("BuildRoadShip"))
		{
			StartCoroutine(GameManager.GUI.ShowMessage("Please place a road."));
			Edge currentEdge = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(HexPos1, HexPos2);
			bool setupPhase = GameManager.Instance.GetCurrentGameState().CurrentTurn.IsInSetupPhase();

			player.placedRoad = false;
			GameManager.GUI.HideFishPopup ();

			if (player.placedRoad)
			{
				StartCoroutine(GameManager.GUI.ShowMessage("You already placed a road."));
				return;
			}

			/*
			if (!isConnectedToOwnedUnit())
			{
				StartCoroutine(GameManager.GUI.ShowMessage("You must place a road adjacent to any intersection!"));
				return;
			}
			*/

			// only consume resources if not in setup phase
			if (!setupPhase)
			{
				Dictionary<StealableType, int> requiredRes;

				if (currentEdge.IsShip())
				{
					requiredRes = new Dictionary<StealableType, int>() {
						{ StealableType.Resource_Fish, 5}
					};
				}
				else
				{
					requiredRes = new Dictionary<StealableType, int>() {
						{ StealableType.Resource_Fish, 5}
					};
				}

				if (!player.HasEnoughResources(requiredRes))
				{
					StartCoroutine(GameManager.GUI.ShowMessage("You don't have enough fish."));
					return;
				}

				player.CmdConsumeResources(requiredRes);
			}

			if (currentEdge.IsShip())
				StartCoroutine(GameManager.GUI.ShowMessage("You have placed a ship."));
			else
				StartCoroutine(GameManager.GUI.ShowMessage("You have placed a road."));

			GameManager.GUI.guiCanvas.transform.FindChild("SelectionTooltip").gameObject.SetActive(true);

			// player.placedRoad = true;
			player.CmdBuildRoad(SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2 }));
		}

		/*
		else if (actionSelected.Equals("OldBoot")
		{
				
		}
		*/

		Debug.Log("Player selected: " + actionSelected);

	}
		
}
