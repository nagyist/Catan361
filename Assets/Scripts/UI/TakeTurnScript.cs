using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeTurnScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void ClickTakeTurn() {
		if(!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED)) {
			return;
		}

		if (GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsLocalPlayerTurn ()) { // end turn
			GameManager.Instance.CurrentPlayerEndTurn();
		} else if (GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsLocalPlayerAllowedToTakeTurn ()) { // take turn
			GameManager.Instance.CurrentPlayerTakeTurn();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED)) {
			return;
		}

		if (GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsLocalPlayerTurn ()) {
			GetComponentInChildren<Text> ().text = "END TURN";
			GetComponent<Button> ().enabled = true;
		} else if (GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsLocalPlayerAllowedToTakeTurn ()) {
			GetComponentInChildren<Text> ().text = "TAKE TURN";
			GetComponent<Button> ().enabled = true;
		} else {
			GetComponentInChildren<Text> ().text = "WAITING";
			GetComponent<Button> ().enabled = false;
		}


	}
}
