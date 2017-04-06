using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GUIInterface : MonoBehaviour {
	public GameObject guiCanvas;

	public void Start() {
		GameManager.GUI = this;
	}

    public IEnumerator ShowMessage(string msg, float delay = 2.5f) {
		GameObject popupPanel = guiCanvas.transform.FindChild ("PanelPopup").gameObject;
		GameObject popupPanelText = popupPanel.transform.FindChild ("Text").gameObject;

		popupPanelText.GetComponent<Text> ().text = msg;
		popupPanel.GetComponent<UIWindow> ().Show ();
		yield return new WaitForSeconds (delay);
		popupPanel.GetComponent<UIWindow>().Hide();
    }

	public void PostStatusMessage(string msg) {
		GameObject gameStatus = guiCanvas.transform.FindChild ("GameStatus").gameObject;
		GameObject lastStatusMsgTxt = gameStatus.transform.FindChild ("TxtLastStatusMessage").gameObject;

		lastStatusMsgTxt.GetComponent<Text> ().text = msg;
	}

	public void ShowHexActionWindow(UIIntersection uiIntersection) {
		if (hasModalWindowOpened ()) { return; }

		GameObject actionPanel = guiCanvas.transform.Find ("PanelHexActions").gameObject;
		actionPanel.GetComponent<RectTransform>().position = Input.mousePosition;
		actionPanel.SetActive (true);
	}

	public void ShowHexActionWindow(UIHex hexTile) {
		if (hasModalWindowOpened ()) { return; }

		GameObject actionPanel = guiCanvas.transform.Find ("PanelHexActions").gameObject;
		actionPanel.GetComponent<RectTransform>().position = Input.mousePosition;
		actionPanel.SetActive (true);
	}

	private bool hasModalWindowOpened() {
		GameObject actionPanel = guiCanvas.transform.Find ("PanelHexActions").gameObject;

		return actionPanel.activeSelf;
	}

	public GameObject GetTooltip(string tooltipName) {
		GameObject tooltipObj = guiCanvas.transform.FindChild(tooltipName).gameObject;
		tooltipObj.GetComponent<RectTransform> ().position = Input.mousePosition;

		return tooltipObj;
	}

	public void ShowTradeRequest(Trade currentTrade) {
		GameObject tradeRequestObj = guiCanvas.transform.FindChild ("TradeRequestPopup").gameObject;
		tradeRequestObj.GetComponent<UIWindow> ().Show ();
	}

	public GameObject GetRoadShipPopup() {
		return guiCanvas.transform.FindChild ("RoadShipConfirm").gameObject;
	}

	public GameObject ShowMoveRobberPiratePopup() {
		GameObject popup = guiCanvas.transform.FindChild ("RobberPirateWindow").gameObject;
		popup.GetComponent<UIWindow> ().Show();

		return popup;
	}

	// Update is called once per frame
	void Update () {
		GetRoadShipPopup ().GetComponent<RoadShipPopup> ();
	}
}
