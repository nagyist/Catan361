using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GUIInterface : MonoBehaviour {
	private GameObject guiCanvas;

    public IEnumerator ShowMessage(string msg, float delay = 1.75f) {
		GameObject popupPanel = guiCanvas.transform.Find ("PanelPopup").gameObject;
		GameObject popupPanelText = popupPanel.transform.FindChild ("Text").gameObject;

		popupPanelText.GetComponent<Text> ().text = msg;
		popupPanel.gameObject.SetActive (true);
		yield return new WaitForSeconds (delay);
		popupPanel.gameObject.SetActive (false);
    }

	public void ShowHexActionWindow(UIHex hexTile) {
		if (hasModalWindowOpened ()) { return; }

		GameObject actionPanel = guiCanvas.transform.Find ("PanelHexActions").gameObject;
		actionPanel.GetComponent<RectTransform>().position = Input.mousePosition;
		actionPanel.SetActive (true);
	}

	public void ToggleCurrentPlayerTurnManagement() {
		if (GameManager.Instance.currentTurn.IsLocalPlayerAllowedToTakeTurn ()) {
			// take turn
				// roll dice
				// trade
				// build road / settlement
			GameManager.Instance.currentPlayerTakeTurn();
		} else if (GameManager.Instance.currentTurn.IsLocalPlayerTurn()) { 
			// end turn
			GameManager.Instance.currentPlayerEndTurn();
		}
	}

	private bool hasModalWindowOpened() {
		GameObject actionPanel = guiCanvas.transform.Find ("PanelHexActions").gameObject;

		return actionPanel.activeSelf;
	}

	private void updateTurnMgmtButton() {
		GameObject turnMgmtButton = guiCanvas.transform.FindChild ("TurnMgmtButton").gameObject;
		if (GameManager.Instance.currentTurn.IsLocalPlayerAllowedToTakeTurn()) {
			turnMgmtButton.transform.FindChild ("TakeTurnText").gameObject.SetActive (true);
			turnMgmtButton.transform.FindChild ("EndTurnText").gameObject.SetActive (false);
			turnMgmtButton.transform.FindChild ("WaitingTurnText").gameObject.SetActive (false);
			turnMgmtButton.GetComponent<Button> ().enabled = true;
		} else if (GameManager.Instance.currentTurn.IsLocalPlayerTurn()) {
			turnMgmtButton.transform.FindChild ("TakeTurnText").gameObject.SetActive (false);
			turnMgmtButton.transform.FindChild ("EndTurnText").gameObject.SetActive (true);
			turnMgmtButton.transform.FindChild ("WaitingTurnText").gameObject.SetActive (false);
			turnMgmtButton.GetComponent<Button> ().enabled = true;
		} else {
			turnMgmtButton.transform.FindChild ("TakeTurnText").gameObject.SetActive (false);
			turnMgmtButton.transform.FindChild ("EndTurnText").gameObject.SetActive (false);
			turnMgmtButton.transform.FindChild ("WaitingTurnText").gameObject.SetActive (true);
			turnMgmtButton.GetComponent<Button> ().enabled = false;
		}
	}
	/*
	private void rollDiceButton () {
		GameObject rollDiceButtonOne = guiCanvas.transform.Find ("RollDiceButton").gameObject;
		if (GameManager.Instance.currentTurn.IsLocalPlayerTurn()) {
			int diceNum = Random.Range (1, 7);
			StartCoroutine(gui.ShowMessage("Player X rolled " + diceNum));
		}

	}
	*/

	// Use this for initialization
	void Start () {
		guiCanvas = GameObject.FindGameObjectWithTag ("GameCanvas");
	}

	// Update is called once per frame
	void Update () {
		//updateTurnMgmtButton ();
	}
}
