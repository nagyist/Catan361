using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaritimeTradeWindow : MonoBehaviour {

	public GameObject brick;
	public GameObject grain;
	public GameObject ore;
	public GameObject wool;
	public GameObject lumber;
	public GameObject fish;
	public GameObject resource;
	public Button confirmButton;

	public Text [] brickNum;
	public Text [] grainNum;
	public Text [] oreNum;
	public Text [] woolNum;
	public Text [] lumberNum;
	public Text [] fishNum;
	public Text [] resourceType;

	public Dictionary <string, StealableType> resourceDict;

	public void resourceRedistribution (string resourceRequested, string brickNumLost, string grainNumLost, string oreNumLost, string woolNumLost, string lumberNumLost, string fishNumLost)
	{
		GamePlayer player = GameManager.LocalPlayer.GetComponent<GamePlayer> ();


		resourceDict = new Dictionary <string, StealableType> ();
		resourceDict.Add ("Brick", StealableType.Resource_Brick);
		resourceDict.Add ("Grain", StealableType.Resource_Grain);
		resourceDict.Add ("Ore", StealableType.Resource_Ore);
		resourceDict.Add ("Wool", StealableType.Resource_Wool);
		resourceDict.Add ("Lumber", StealableType.Resource_Lumber);
		resourceDict.Add ("Fish", StealableType.Resource_Fish);

		ResourceCollection.PlayerResourcesCollection playerResources = player.GetPlayerResources ();

		if (resourceDict.ContainsKey (resourceRequested) && !player.hasTradingHouse) {
			int newRes = playerResources [resourceDict [resourceRequested]];

			if (playerResources.ContainsKey (StealableType.Resource_Brick) && int.Parse (brickNumLost) % 4 == 0 && (playerResources [StealableType.Resource_Brick] - int.Parse (brickNumLost)) >= 0) {
				player.CmdUpdateResource (StealableType.Resource_Brick, playerResources [StealableType.Resource_Brick] - int.Parse (brickNumLost));
				newRes += int.Parse (brickNumLost) / 4;
				player.CmdUpdateResource (resourceDict [resourceRequested], newRes);
				//player.playerResources[StealableType.Resource_Brick] =  player.playerResources[StealableType.Resource_Brick] - int.Parse(brickNumLost);
			}


			if (playerResources.ContainsKey (StealableType.Resource_Grain) && int.Parse (grainNumLost) % 4 == 0 && (playerResources [StealableType.Resource_Grain] - int.Parse (grainNumLost)) >= 0) {
				player.CmdUpdateResource (StealableType.Resource_Grain, playerResources [StealableType.Resource_Grain] - int.Parse (grainNumLost));
				newRes += int.Parse (grainNumLost) / 4;
				player.CmdUpdateResource (resourceDict [resourceRequested], newRes);
				//player.playerResources[StealableType.Resource_Grain] =  player.playerResources[StealableType.Resource_Grain] - int.Parse(grainNumLost);
			}

			if (playerResources.ContainsKey (StealableType.Resource_Ore) && int.Parse (oreNumLost) % 4 == 0 && (playerResources [StealableType.Resource_Ore] - int.Parse (oreNumLost)) >= 0) {
				player.CmdUpdateResource (StealableType.Resource_Ore, playerResources [StealableType.Resource_Ore] - int.Parse (oreNumLost));
				newRes += int.Parse (oreNumLost) / 4;
				player.CmdUpdateResource (resourceDict [resourceRequested], newRes);
				//player.playerResources[StealableType.Resource_Ore] =  player.playerResources[StealableType.Resource_Ore] - int.Parse(oreNumLost);
			}

			if (playerResources.ContainsKey (StealableType.Resource_Wool) && int.Parse (woolNumLost) % 4 == 0 && (playerResources [StealableType.Resource_Wool] - int.Parse (woolNumLost)) >= 0) {
				player.CmdUpdateResource (StealableType.Resource_Wool, playerResources [StealableType.Resource_Wool] - int.Parse (woolNumLost));
				newRes += int.Parse (woolNumLost) / 4;
				player.CmdUpdateResource (resourceDict [resourceRequested], newRes);
				//player.playerResources[StealableType.Resource_Wool] =  player.playerResources[StealableType.Resource_Wool] - int.Parse(woolNumLost);
			}

			if (playerResources.ContainsKey (StealableType.Resource_Lumber) && int.Parse (lumberNumLost) % 4 == 0 && (playerResources [StealableType.Resource_Lumber] - int.Parse (lumberNumLost)) >= 0) {
				player.CmdUpdateResource (StealableType.Resource_Lumber, playerResources [StealableType.Resource_Lumber] - int.Parse (lumberNumLost));
				newRes += int.Parse (lumberNumLost) / 4;
				player.CmdUpdateResource (resourceDict [resourceRequested], newRes);
				//player.playerResources[StealableType.Resource_Lumber] =  player.playerResources[StealableType.Resource_Lumber] - int.Parse(lumberNumLost);
			}

			if (playerResources.ContainsKey (StealableType.Resource_Fish) && int.Parse (fishNumLost) % 4 == 0 && (playerResources [StealableType.Resource_Fish] - int.Parse (fishNumLost)) >= 0) {
				player.CmdUpdateResource (StealableType.Resource_Fish, playerResources [StealableType.Resource_Fish] - int.Parse (fishNumLost));
				newRes += int.Parse (fishNumLost) / 4;
				player.CmdUpdateResource (resourceDict [resourceRequested], newRes);
				//player.playerResources[StealableType.Resource_Fish] =  player.playerResources[StealableType.Resource_Fish] - int.Parse(fishNumLost);
			}
		} else if (resourceDict.ContainsKey (resourceRequested) && player.hasTradingHouse) {
				int newRes = playerResources [resourceDict [resourceRequested]];

				if (playerResources.ContainsKey (StealableType.Resource_Brick) && int.Parse (brickNumLost) % 2 == 0 && (playerResources [StealableType.Resource_Brick] - int.Parse (brickNumLost)) >= 0) {
					player.CmdUpdateResource (StealableType.Resource_Brick, playerResources [StealableType.Resource_Brick] - int.Parse (brickNumLost));
					newRes += int.Parse (brickNumLost) / 2;
					player.CmdUpdateResource (resourceDict [resourceRequested], newRes);
				}

				if (playerResources.ContainsKey (StealableType.Resource_Grain) && int.Parse (grainNumLost) % 2 == 0 && (playerResources [StealableType.Resource_Grain] - int.Parse (grainNumLost)) >= 0) {
					player.CmdUpdateResource (StealableType.Resource_Grain, playerResources [StealableType.Resource_Grain] - int.Parse (grainNumLost));
					newRes += int.Parse (grainNumLost) / 2;
					player.CmdUpdateResource (resourceDict [resourceRequested], newRes);
				}

				if (playerResources.ContainsKey (StealableType.Resource_Ore) && int.Parse (oreNumLost) % 2 == 0 && (playerResources [StealableType.Resource_Ore] - int.Parse (oreNumLost)) >= 0) {
					player.CmdUpdateResource (StealableType.Resource_Ore, playerResources [StealableType.Resource_Ore] - int.Parse (oreNumLost));
					newRes += int.Parse (oreNumLost) / 2;
					player.CmdUpdateResource (resourceDict [resourceRequested], newRes);
				}

				if (playerResources.ContainsKey (StealableType.Resource_Wool) && int.Parse (woolNumLost) % 2 == 0 && (playerResources [StealableType.Resource_Wool] - int.Parse (woolNumLost)) >= 0) {
					player.CmdUpdateResource (StealableType.Resource_Wool, playerResources [StealableType.Resource_Wool] - int.Parse (woolNumLost));
					newRes += int.Parse (woolNumLost) / 2;
					player.CmdUpdateResource (resourceDict [resourceRequested], newRes);
				}

				if (playerResources.ContainsKey (StealableType.Resource_Lumber) && int.Parse (lumberNumLost) % 2 == 0 && (playerResources [StealableType.Resource_Lumber] - int.Parse (lumberNumLost)) >= 0) {
					player.CmdUpdateResource (StealableType.Resource_Lumber, playerResources [StealableType.Resource_Lumber] - int.Parse (lumberNumLost));
					newRes += int.Parse (lumberNumLost) / 2;
					player.CmdUpdateResource (resourceDict [resourceRequested], newRes);
				}

				if (playerResources.ContainsKey (StealableType.Resource_Fish) && int.Parse (fishNumLost) % 2 == 0 && (playerResources [StealableType.Resource_Fish] - int.Parse (fishNumLost)) >= 0) {
					player.CmdUpdateResource (StealableType.Resource_Fish, playerResources [StealableType.Resource_Fish] - int.Parse (fishNumLost));
					newRes += int.Parse (fishNumLost) / 2;
					player.CmdUpdateResource (resourceDict [resourceRequested], newRes);
				}
		}
	}

	void TaskOnClick(){
		Debug.Log ("You have clicked the button!");
		resourceRedistribution (resourceType [1].text, brickNum [1].text, grainNum [1].text, oreNum [1].text, woolNum [1].text, lumberNum [1].text, fishNum[1].text);
	}

	// Use this for initialization
	void Start () {
		brickNum = brick.GetComponentsInChildren<Text>();
		grainNum = grain.GetComponentsInChildren<Text>();
		oreNum = ore.GetComponentsInChildren<Text>();
		woolNum = wool.GetComponentsInChildren<Text>();
		lumberNum = lumber.GetComponentsInChildren<Text>();
		fishNum = fish.GetComponentsInChildren<Text> ();
		resourceType = resource.GetComponentsInChildren<Text>();

		Button btn = confirmButton.GetComponent<Button>();
		btn.onClick.AddListener (TaskOnClick);
	}

	// Update is called once per frame
	void Update () {
		
	}
}

