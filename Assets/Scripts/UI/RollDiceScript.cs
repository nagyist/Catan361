using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDiceScript : MonoBehaviour {

	public enum EventDiceOutcome {
		Barbarian, City_Gates_Red, City_Gates_Green, City_Gates_Blue
	}

	private static Dictionary<int, EventDiceOutcome> eventDiceOutcomeDistr = new Dictionary<int, EventDiceOutcome> {
		{1, EventDiceOutcome.Barbarian},
		{2, EventDiceOutcome.Barbarian},
		{3, EventDiceOutcome.Barbarian},
		{4, EventDiceOutcome.City_Gates_Red},
		{5, EventDiceOutcome.City_Gates_Green},
		{6, EventDiceOutcome.City_Gates_Blue}
	};

	// Use this for initialization
	void Start () {
		
	}

	public void RollDiceClick() {
		if(!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED)) {
			return;
		}

		GameManager.Instance.RollDice (UnityEngine.Random.Range(1, 13));
		GameManager.Instance.RollEventDice (eventDiceOutcomeDistr[UnityEngine.Random.Range (1, 7)]);
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED)) {
			return;
		}

		if (GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsInSetupPhase ()) {
			GetComponent<Button> ().enabled = false;
			GetComponentInChildren<Text> ().text = "SETUP";
		} else {
			if (GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsLocalPlayerTurn ()) {
				GetComponent<Button> ().enabled = true;
				GetComponentInChildren<Text> ().text = "ROLL DICE";
			} else {
				GetComponent<Button> ().enabled = false;
				GetComponentInChildren<Text> ().text = "WAIT";
			}
		}
	}
}
