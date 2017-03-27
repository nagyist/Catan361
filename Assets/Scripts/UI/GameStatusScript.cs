using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatusScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED) && GameManager.Instance.GetCurrentGameState().CurrentTurn != null) {
			// update turn no
			GameObject turnNum = transform.FindChild("TurnInfo").FindChild("NumTurn").gameObject;
			turnNum.GetComponentInChildren<Text> ().text = "" + GameManager.Instance.GetCurrentGameState ().CurrentTurn.RoundCount;

			// update current turn status
			GameObject turnStatusGameObj = transform.FindChild("TurnInfo").FindChild("TxtTurnType").gameObject;
			string turnStatus = "";
			int playerIdx = GameManager.Instance.GetCurrentGameState ().CurrentTurn.CurrentPlayerIndex;
			if (playerIdx < 0) {
				turnStatus = "Initial round";
			} else {
				if(GameManager.Instance.GetCurrentGameState().CurrentTurn.IsInSetupPhase()) {
					turnStatus = "Setup Phase";
				}
				turnStatus += GameManager.Instance.GetCurrentGameState().CurrentTurn.OrderedPlayers[playerIdx] + " turn";
			}

			turnStatusGameObj.GetComponent<Text>().text = turnStatus;
		}
	}
}
