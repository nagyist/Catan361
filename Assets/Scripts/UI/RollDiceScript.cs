using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDiceScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void RollDiceClick() {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED)) {
			return;
		}

		GetComponent<Button> ().enabled = GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsLocalPlayerTurn ();
	}
}
