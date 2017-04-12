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
		tradeRequestObj.GetComponent<TradeRequestPopup> ().tradeObj = currentTrade;
		tradeRequestObj.GetComponent<TradeRequestPopup> ().active = true;
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

	public GameObject HideMoveRobberPiratePopup() {
		GameObject popup = guiCanvas.transform.FindChild ("RobberPirateWindow").gameObject;
		popup.GetComponent<UIWindow> ().Hide();

		return popup;
	}
		
	public GameObject ShowPlayer2PlayerTradeWindow(Trade currentTrade) {
		GameObject tradeWindow = guiCanvas.transform.FindChild ("Player2PlayerTrade").gameObject;
		tradeWindow.GetComponent<Player2PlayerTrade> ().currentTrade = currentTrade;

		tradeWindow.GetComponent<UIWindow> ().Show ();
		return tradeWindow;
	}	

	public GameObject ShowHarbourTradePopup() {
		GameObject popup = guiCanvas.transform.FindChild ("HarbourTradeMenu").gameObject;
		popup.GetComponent<UIWindow> ().Show();

		return popup;
	}

	public GameObject HideHarbourTradePopup() {
		GameObject popup = guiCanvas.transform.FindChild ("HarbourTradeMenu").gameObject;
		popup.GetComponent<UIWindow> ().Hide();

		return popup;
	}

	public GameObject ShowGoldPopup() {
		GameObject popup = guiCanvas.transform.FindChild ("GoldPopup").gameObject;
		popup.GetComponent<UIWindow> ().Show();

		return popup;
	}

	public GameObject HideGoldPopup() {
		GameObject popup = guiCanvas.transform.FindChild ("GoldPopup").gameObject;
		popup.GetComponent<UIWindow> ().Hide();

		return popup;
	}

	public GameObject ShowAqueductPopup() {
		GameObject popup = guiCanvas.transform.FindChild ("AqueductPopup").gameObject;
		popup.GetComponent<UIWindow> ().Show();

		return popup;
	}

	public GameObject HideAqueductPopup() {
		GameObject popup = guiCanvas.transform.FindChild ("AqueductPopup").gameObject;
		popup.GetComponent<UIWindow> ().Hide();

		return popup;
	}

	public GameObject ShowFishPopup() {
		GameObject popup = guiCanvas.transform.FindChild ("FishActionMenu").gameObject;
		popup.GetComponent<UIWindow> ().Show();

		return popup;
	}

	public GameObject HideFishPopup() {
		GameObject popup = guiCanvas.transform.FindChild ("FishActionMenu").gameObject;
		popup.GetComponent<UIWindow> ().Hide();

		return popup;
	}

	public GameObject ShowBarbarianInvasionSummary(BarbarianInvasion invasion) {
		GameObject popup = guiCanvas.transform.FindChild ("BarbarianInvasionSummary").gameObject;
		popup.GetComponent<BarbarianInvasionSummary> ().CurrentInvasion = invasion;
		popup.GetComponent<UIWindow> ().Show ();

		return popup;
	}

	public GameObject ShowGateEvent(GateEvent currEvent) {
		GameObject popup = guiCanvas.transform.FindChild("GateEventDiceWindow").gameObject;
		popup.GetComponent<GateEventWindow>().CurrentEvent = currEvent;
		popup.GetComponent<UIWindow> ().Show ();

		return popup;
	}

	public IEnumerator ShowNewProgressCardNotification(AbstractProgressCard newCard) {
		GameObject notif = guiCanvas.transform.FindChild ("ProgressCardNotification").gameObject;
		notif.GetComponent<UIProgressCardNotification> ().CurrentCard = newCard;
		notif.GetComponent<UIWindow> ().Show ();
		yield return new WaitForSeconds (2.5f);
		notif.GetComponent<UIWindow>().Hide();
	}

	public GameObject ShowProgressCardWindowOfUser(string playerName) {
		GameObject window = guiCanvas.transform.FindChild ("ProgressCardsWindow").gameObject;
		window.GetComponent<UIProgressCardWindow> ().Clear ();
		window.GetComponent<UIProgressCardWindow> ().OtherPlayer = true;
		window.GetComponent<UIProgressCardWindow> ().GamePlayerName = playerName;

		window.GetComponent<UIWindow> ().Show ();

		return window;
	}

	public GameObject ShowResourceMonopolyPopup(AbstractProgressCard currentCard) {
		GameObject window = guiCanvas.transform.FindChild ("ResourceMonopolyPopup").gameObject;
		window.GetComponent<ResourceMonopolyPopup> ().CurrentCard = currentCard;
		window.GetComponent<UIWindow> ().Show ();

		return window;
	}

	// Update is called once per frame
	void Update () {
		GetRoadShipPopup ().GetComponent<RoadShipPopup> ();
	}
}
