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

		if (GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsLocalPlayerTurn ()) {
			// roll the dice
			int rand = UnityEngine.Random.Range(1, 13);
			GameObject panelRollDice = GameObject.FindGameObjectWithTag ("GameCanvas").transform.FindChild ("PanelRollDice").gameObject;
			panelRollDice.GetComponentInChildren<Text>().text = "You rolled " + rand;
			GameObject.FindGameObjectWithTag ("GameCanvas").transform.FindChild ("PanelRollDice").gameObject.SetActive (true);

			GameManager.Instance.RollDice(rand);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED)) {
			return;
		}

		if (GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsInSetupPhase ()) {
			GetComponent<Button> ().enabled = false;
			GetComponentInChildren<Text> ().text = "In setup phase -- round: " + GameManager.Instance.GetCurrentGameState ().CurrentTurn.RoundCount;
		} else {
			if (GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsLocalPlayerTurn ()) {
				GetComponent<Button> ().enabled = true;
				GetComponentInChildren<Text> ().text = "Roll dice";
			} else {
				GetComponent<Button> ().enabled = false;
				GetComponentInChildren<Text> ().text = "Wait for your turn";
			}
		}
	}
}
