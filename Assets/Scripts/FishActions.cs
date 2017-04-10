using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishActions : MonoBehaviour {

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
		/*
		else if (actionSelected.Equals("StealResource"))
		{
			
		}
		else if (actionSelected.Equals("BuildRoadShip"))
		{
			
		}
		else if (actionSelected.Equals("OldBoot")
		{
				
		}
		*/
		Debug.Log("Player selected: " + actionSelected);

	}
		
}
