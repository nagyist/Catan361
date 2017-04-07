using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarbourTrade : MonoBehaviour {

	public Harbour harbour;
	public Button confirmButton;
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

			GamePlayer player = GameManager.LocalPlayer.GetComponent<GamePlayer>();
			//player.playerResources[selected]++;
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

		if (resourceDict.ContainsKey(resourceOffered)) 
		{
			int newRes = playerResources [resourceDict[resourceOffered]] - 2;
			player.CmdUpdateResource (resourceDict [resourceOffered], newRes);
			//player.playerResources [resourceDict[resourceOffered]] = newRes;
		}

		if (playerResources.ContainsKey (harbour.returnedResource))
		{
			player.CmdUpdateResource (harbour.returnedResource, playerResources[harbour.returnedResource] + 1);
			//player.playerResources[StealableType.Resource_Brick] =  player.playerResources[StealableType.Resource_Brick] - int.Parse(brickNumLost);
		}


	}

	void TaskOnClick(){
		Debug.Log ("You have clicked the button!");
		resourceRedistribution (resourceSelected);
	}

	// Use this for initialization
	void Start () {
		Button btn = confirmButton.GetComponent<Button>();
		btn.onClick.AddListener (TaskOnClick);
	}

	// Update is called once per frame
	void Update () {

	}

}
	

