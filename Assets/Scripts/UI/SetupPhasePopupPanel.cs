using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupPhasePopupPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED)) {
			return;
		}

		if (GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsLocalPlayerTurn ()) {
			if (GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsInSetupPhase ()) {
				GetComponentInChildren<Text> ().text = GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName + " - your turn (setup phase, place road & settlement)";
			} else {
				GetComponentInChildren<Text> ().text = GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName + " - your turn";
			}

			gameObject.GetComponentInChildren<Image> ().enabled = true;
			gameObject.GetComponentInChildren<Text> ().enabled = true;
		} else {
			gameObject.GetComponentInChildren<Image> ().enabled = false;
			gameObject.GetComponentInChildren<Text> ().enabled = false;
		}
	}
}
