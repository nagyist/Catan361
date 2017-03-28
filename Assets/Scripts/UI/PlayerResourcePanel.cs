﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// this class is used to control the text displayed in the left pnale showing all the players' resources
public class PlayerResourcePanel : MonoBehaviour {

	public int PlayerIndex = 0;

	private Dictionary<StealableType, GameObject> resourcesGameObjs = new Dictionary<StealableType, GameObject>();

	// Use this for initialization
	void Start () {
		resourcesGameObjs.Add (StealableType.Resource_Wool, transform.FindChild ("Wool").gameObject);
		resourcesGameObjs.Add (StealableType.Resource_Lumber, transform.FindChild ("Lumber").gameObject);
		resourcesGameObjs.Add (StealableType.Resource_Ore, transform.FindChild ("Ore").gameObject);
		resourcesGameObjs.Add (StealableType.Resource_Brick, transform.FindChild ("Brick").gameObject);
		resourcesGameObjs.Add (StealableType.Resource_Grain, transform.FindChild ("Grain").gameObject);
		resourcesGameObjs.Add (StealableType.Resource_Fish, transform.FindChild ("Fish").gameObject);
		resourcesGameObjs.Add (StealableType.Resource_Gold, transform.FindChild ("Gold").gameObject);

		transform.FindChild ("Header").gameObject.GetComponentInChildren<Text> ().text = this.getPlayer ().myName;
	}

	private void displayPlayerResource(StealableType type) {
		int resValue = 0;
		if (getPlayer ().playerResources.ContainsKey (type)) {
			resValue = getPlayer ().playerResources [type];
		}

		resourcesGameObjs [type].GetComponentInChildren<Text> ().text = "" + resValue;
	}

	private GamePlayer getPlayer() {
		return GameManager.ConnectedPlayers [PlayerIndex].GetComponent<GamePlayer>();
	}

	// Update is called once per frame
	void Update () {
        // check if the game is ready  (if the grid is created)
		if (GameManager.Instance.GameStateReadyAtStage (GameState.GameStatus.GRID_CREATED)) {
			foreach (StealableType type in getPlayer().playerResources.Keys) {
				displayPlayerResource (type);
			}
		}

	}
}
