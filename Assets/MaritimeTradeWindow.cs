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
	public GameObject harbour;
	public Button confirmButton;
	public HexGrid hexGrid;

	public Text [] brickNum;
	public Text [] grainNum;
	public Text [] oreNum;
	public Text [] woolNum;
	public Text [] lumberNum;
	public Text [] harbourNum;
	public Text [] resourceOffer;

	public GameObject findHarbour(string harbourNumber)
	{
		return hexGrid.harbourCollection [harbourNumber];
	}

	public StealableType findHarbourResource (GameObject harbourSelected)
	{
		return harbourSelected.GetComponent<Harbour>().returnedResource;
	}

	public void resourceRedistribution (StealableType resourceFromHarbour, string brickNumLost, string grainNumLost, string oreNumLost, string woolNumLost, string lumberNumLost)
	{
		GamePlayer player = GameManager.LocalPlayer.GetComponent<GamePlayer> ();

		if (player.playerResources.ContainsKey (resourceFromHarbour)) 
		{
			player.playerResources [resourceFromHarbour] = player.playerResources [resourceFromHarbour]++;
		}

		if (player.playerResources.ContainsKey (StealableType.Resource_Brick))
		{
			player.playerResources[StealableType.Resource_Brick] =  player.playerResources[StealableType.Resource_Brick] - int.Parse(brickNumLost);
		}

		if (player.playerResources.ContainsKey (StealableType.Resource_Grain))
		{
			player.playerResources[StealableType.Resource_Grain] =  player.playerResources[StealableType.Resource_Grain] - int.Parse(grainNumLost);
		}

		if (player.playerResources.ContainsKey (StealableType.Resource_Ore))
		{
			player.playerResources[StealableType.Resource_Ore] =  player.playerResources[StealableType.Resource_Ore] - int.Parse(oreNumLost);
		}

		if (player.playerResources.ContainsKey (StealableType.Resource_Wool))
		{
			player.playerResources[StealableType.Resource_Wool] =  player.playerResources[StealableType.Resource_Wool] - int.Parse(woolNumLost);
		}

		if (player.playerResources.ContainsKey (StealableType.Resource_Lumber))
		{
			player.playerResources[StealableType.Resource_Lumber] =  player.playerResources[StealableType.Resource_Lumber] - int.Parse(lumberNumLost);
		}
	}

	// Use this for initialization
	void Start () {
		
		brickNum = brick.GetComponentsInChildren<Text>();
		grainNum = grain.GetComponentsInChildren<Text>();
		oreNum = ore.GetComponentsInChildren<Text>();
		woolNum = wool.GetComponentsInChildren<Text>();
		lumberNum = lumber.GetComponentsInChildren<Text>();
		harbourNum = harbour.GetComponentsInChildren<Text>();

		//Text [] resourceOffer = {brickNum[1], grainNum[1], oreNum[1], woolNum[1], lumberNum[1]};
		//confirmButton.onClick
			GameObject thisHarbour = findHarbour (harbourNum[1].text);
			StealableType thisHarbourResource = findHarbourResource (thisHarbour);
			resourceRedistribution (thisHarbourResource, brickNum [1].text, grainNum [1].text, oreNum [1].text, woolNum [1].text, lumberNum [1].text);
		
	}

	// Update is called once per frame
	void Update () {
		
	}
}

