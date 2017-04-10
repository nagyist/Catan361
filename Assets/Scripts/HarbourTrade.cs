using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarbourTrade : MonoBehaviour {

	public int exchangeRate{ get; set; }
	public StealableType returnedResource { get; set; }
	public int returnedAmount { get; set; }
	public int count = 1;
	//public Button confirmButton;
	private string resourceSelected;

		public void accepted()
		{
			ToggleGroup toggles = gameObject.GetComponentInChildren<ToggleGroup>();
			IEnumerator<Toggle> togglesEnum = toggles.ActiveToggles().GetEnumerator();
			togglesEnum.MoveNext();
			Toggle toggle = togglesEnum.Current;
			resourceSelected = toggle.transform.parent.name;
			StealableType selected;

			if (resourceSelected.Equals("Brick"))
			{
				selected = StealableType.Resource_Brick;
			}
			else if (resourceSelected.Equals("Grain"))
			{
				selected = StealableType.Resource_Grain;
			}
			else if (resourceSelected.Equals("Lumber"))
			{
				selected = StealableType.Resource_Lumber;
			}
			else if (resourceSelected.Equals("Ore"))
			{
				selected = StealableType.Resource_Ore;
			}
			else 
			{
				selected = StealableType.Resource_Wool;
			}

			Debug.Log("Player selected: " + resourceSelected);

			resourceRedistribution (resourceSelected);

		}

	public void resourceRedistribution (string resourceOffered)
	{
		GamePlayer player = GameManager.LocalPlayer.GetComponent<GamePlayer> ();

		Dictionary <string, StealableType> resourceDict = new Dictionary <string, StealableType> ();
		resourceDict.Add ("Brick", StealableType.Resource_Brick);
		resourceDict.Add ("Grain", StealableType.Resource_Grain);
		resourceDict.Add ("Ore", StealableType.Resource_Ore);
		resourceDict.Add ("Wool", StealableType.Resource_Wool);
		resourceDict.Add ("Lumber", StealableType.Resource_Lumber);

		ResourceCollection.PlayerResourcesCollection playerResources = player.GetPlayerResources ();

		if (resourceDict.ContainsKey(resourceOffered) && playerResources[resourceDict[resourceOffered]] >= exchangeRate && count > 0) 
		{
			int newRes = playerResources [resourceDict[resourceOffered]] - exchangeRate;
			player.CmdUpdateResource (resourceDict [resourceOffered], newRes);
		}

		if (playerResources.ContainsKey (returnedResource) && returnedResource != StealableType.None) 
		{
			player.CmdUpdateResource (returnedResource, playerResources [returnedResource] + 1);
			GameManager.GUI.HideHarbourTradePopup ();
		} 
		else if (returnedResource == StealableType.None && count > 0) 
		{
			count--;
			GameManager.GUI.HideHarbourTradePopup ();
			StartCoroutine(GameManager.GUI.ShowMessage ("Please select the desired return resource."));
			GameManager.GUI.ShowHarbourTradePopup ();
		} 
		else 
		{
			player.CmdUpdateResource (resourceDict[resourceOffered], playerResources [resourceDict[resourceOffered]] + 1);
			GameManager.GUI.HideHarbourTradePopup ();
			count++;
		}
			
	}

}
	

