using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDiceScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void RollDiceClick() {
		if(!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED)) {
			return;
		}

		GameManager.Instance.RollDice (UnityEngine.Random.Range(1, 13));
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
