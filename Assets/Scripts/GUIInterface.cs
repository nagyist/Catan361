using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GUIInterface : MonoBehaviour {
    private Canvas guiCanvas;

    public IEnumerator ShowMessage(string msg, float delay = 1.75f) {
		GameObject popupPanel = guiCanvas.transform.Find ("PanelPopup").gameObject;
		GameObject popupPanelText = popupPanel.transform.FindChild ("Text").gameObject;

		popupPanelText.GetComponent<Text> ().text = msg;
		popupPanel.gameObject.SetActive (true);
		yield return new WaitForSeconds (delay);
		popupPanel.gameObject.SetActive (false);
    }

	public void ShowHexActionWindow(Hex hexTile) {
		if (hasModalWindowOpened ()) { return; }

		GameObject actionPanel = guiCanvas.transform.Find ("PanelHexActions").gameObject;
		actionPanel.GetComponent<RectTransform>().position = Input.mousePosition;
		actionPanel.SetActive (true);
	}

	public void ToggleCurrentPlayerTurnManagement() {
		if (GameManager.Instance.currentTurn.IsCurrentPlayerAllowedToTakeTurn ()) {
			// take turn
			GameManager.Instance.currentPlayerTakeTurn();
		} else if (GameManager.Instance.currentTurn.IsCurrentPlayerTurn () && GameManager.Instance.currentTurn.IsTurnTaken ()) { 
			// end turn

		}
	}

	private bool hasModalWindowOpened() {
		GameObject actionPanel = guiCanvas.transform.Find ("PanelHexActions").gameObject;

		return actionPanel.activeSelf;
	}

	private void updateTurnMgmtButton() {
		GameObject turnMgmtButton = guiCanvas.transform.Find ("TurnMgmtButton").gameObject;
		if (GameManager.Instance.currentTurn.IsCurrentPlayerAllowedToTakeTurn()) {
			turnMgmtButton.transform.FindChild ("TakeTurnText").gameObject.SetActive (true);
			turnMgmtButton.transform.FindChild ("EndTurnText").gameObject.SetActive (false);
			turnMgmtButton.transform.FindChild ("WaitingTurnText").gameObject.SetActive (false);
		} else if (GameManager.Instance.currentTurn.IsCurrentPlayerTurn() && GameManager.Instance.currentTurn.IsTurnTaken ()) {
			turnMgmtButton.transform.FindChild ("TakeTurnText").gameObject.SetActive (false);
			turnMgmtButton.transform.FindChild ("EndTurnText").gameObject.SetActive (true);
			turnMgmtButton.transform.FindChild ("WaitingTurnText").gameObject.SetActive (false);
		} else {
			turnMgmtButton.transform.FindChild ("TakeTurnText").gameObject.SetActive (false);
			turnMgmtButton.transform.FindChild ("EndTurnText").gameObject.SetActive (false);
			turnMgmtButton.transform.FindChild ("WaitingTurnText").gameObject.SetActive (true);
			turnMgmtButton.GetComponent<Button> ().enabled = false;
		}
	}

	// Use this for initialization
	void Start () {
        guiCanvas = GameObject.FindObjectOfType<Canvas>();
	}

	// Update is called once per frame
	void Update () {
		updateTurnMgmtButton ();
	}
}
