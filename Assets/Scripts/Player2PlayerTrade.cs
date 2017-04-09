using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player2PlayerTrade : MonoBehaviour {
	
	public Trade currentTrade;

	private static Dictionary<string, StealableType> transformNameToStealableType = new Dictionary<string, StealableType> {
		{"Wool", StealableType.Resource_Wool},
		{"Lumber", StealableType.Resource_Lumber},
		{"Ore", StealableType.Resource_Ore},
		{"Brick", StealableType.Resource_Brick},
		{"Grain", StealableType.Resource_Grain},
		{"Fish", StealableType.Resource_Fish}
	};


	void Start () {
		
	}

	private GamePlayer getLocalPlayer() {
		return GameManager.LocalPlayer.GetComponent<GamePlayer> ();
	}

	private string getLocalPlayerIdInTrade() {
		return getLocalPlayer ().myName == currentTrade.Player1 ? "Player1" : "Player2";
	}

	private string getOtherPlayerIdInTrade() {
		return getLocalPlayerIdInTrade () == "Player1" ? "Player2" : "Player1";
	}

	public void EditQty(InputField field) {
		string transformName = field.transform.name;
		StealableType editQtyType = transformNameToStealableType [transformName];
		int qtyVal = Int32.Parse(field.text);

		ResourceCollection.PlayerResourcesCollection playerRes = GameManager.Instance.GetCurrentGameState ().CurrentResources.GetPlayerResources (getLocalPlayer ().myName);
		if (!playerRes.ContainsKey (editQtyType) || playerRes[editQtyType] < qtyVal) {
			StartCoroutine(GameManager.GUI.ShowMessage ("You don't have enough " + transformName + " to trade that amount."));
			field.text = "0";
			return;
		}

		if (getLocalPlayerIdInTrade () == "Player1") {
			currentTrade.Player1Offer [editQtyType] = qtyVal;
		} else if (getLocalPlayerIdInTrade () == "Player2") {
			currentTrade.Player2Offer [editQtyType] = qtyVal;
		}

		getLocalPlayer ().CmdUpdateTradeOffer (SerializationUtils.ObjectToByteArray (currentTrade));
	}

	public void AcceptOffer() {
		if (getLocalPlayerIdInTrade () == "Player1") {
			currentTrade.Player1Accepted = Trade.TradePlayerOfferStatus.PlayerAccepted;
		} else if (getLocalPlayerIdInTrade () == "Player2") {
			currentTrade.Player2Accepted = Trade.TradePlayerOfferStatus.PlayerAccepted;
		}

		getLocalPlayer ().CmdUpdateTradeOffer (SerializationUtils.ObjectToByteArray (currentTrade));
	}

	public void DeclineOffer() {
		if (getLocalPlayerIdInTrade () == "Player1") {
			currentTrade.Player1Accepted = Trade.TradePlayerOfferStatus.PlayerRejected;
		} else if (getLocalPlayerIdInTrade () == "Player2") {
			currentTrade.Player2Accepted = Trade.TradePlayerOfferStatus.PlayerRejected;
		}

		getLocalPlayer ().CmdUpdateTradeOffer (SerializationUtils.ObjectToByteArray (currentTrade));
	}

	void Update () {
		if (!GetComponent<UIWindow> ().IsVisible) {
			return;
		}

		// deactivate other player's offer GUI elements
		GameObject localPlayerOffer = transform.FindChild ("Content").FindChild ("Pages").FindChild ("Page").FindChild (getLocalPlayerIdInTrade()).gameObject;
		GameObject otherPlayerOffer = transform.FindChild ("Content").FindChild ("Pages").FindChild ("Page").FindChild (getOtherPlayerIdInTrade()).gameObject;
		foreach (InputField f in otherPlayerOffer.GetComponentsInChildren<InputField>()) {
			f.DeactivateInputField ();
			f.enabled = false;
		}

		Button localAcceptBtn = localPlayerOffer.transform.FindChild ("AcceptBtn").gameObject.GetComponent<Button> ();
		Button localDeclineBtn = localPlayerOffer.transform.FindChild ("DeclineBtn").gameObject.GetComponent<Button> ();

		Button otherAcceptBtn = otherPlayerOffer.transform.FindChild ("AcceptBtn").gameObject.GetComponent<Button>();
		Button otherDeclineBtn = otherPlayerOffer.transform.FindChild ("DeclineBtn").gameObject.GetComponent<Button> ();

		otherAcceptBtn.enabled = false;
		otherDeclineBtn.enabled = false;

		// update trade button
		Trade.TradePlayerOfferStatus localPlayerStatus = Trade.TradePlayerOfferStatus.Undecided;
		Trade.TradePlayerOfferStatus otherPlayerStatus = Trade.TradePlayerOfferStatus.Undecided;
		if (getOtherPlayerIdInTrade () == "Player1") {
			otherPlayerStatus = currentTrade.Player1Accepted;
			localPlayerStatus = currentTrade.Player2Accepted;
		} else if (getOtherPlayerIdInTrade () == "Player2") {
			otherPlayerStatus = currentTrade.Player2Accepted;
			localPlayerStatus = currentTrade.Player1Accepted;
		}

		if (localPlayerStatus == Trade.TradePlayerOfferStatus.PlayerAccepted) {
			localAcceptBtn.GetComponentInChildren<Image>().color = new Color32 (0, 255, 0, 255);
			localDeclineBtn.GetComponentInChildren<Image>().color = new Color32 (0, 0, 0, 255);
		} else if (localPlayerStatus == Trade.TradePlayerOfferStatus.PlayerRejected) {
			localAcceptBtn.GetComponentInChildren<Image>().color = new Color32 (0, 0, 0, 255);
			localDeclineBtn.GetComponentInChildren<Image>().color = new Color32 (255, 0, 0, 255);
		} else {
			localAcceptBtn.GetComponentInChildren<Image>().color = new Color32 (0, 0, 0, 255);
			localDeclineBtn.GetComponentInChildren<Image>().color = new Color32 (0, 0, 0, 255);
		}

		if (otherPlayerStatus == Trade.TradePlayerOfferStatus.PlayerAccepted) {
			otherAcceptBtn.GetComponentInChildren<Image>().color = new Color32 (0, 255, 0, 255);
			otherDeclineBtn.GetComponentInChildren<Image>().color = new Color32 (0, 0, 0, 255);
		} else if (otherPlayerStatus == Trade.TradePlayerOfferStatus.PlayerRejected) {
			otherAcceptBtn.GetComponentInChildren<Image>().color = new Color32 (0, 0, 0, 255);
			otherDeclineBtn.GetComponentInChildren<Image>().color = new Color32 (255, 0, 0, 255);
		} else {
			otherAcceptBtn.GetComponentInChildren<Image>().color = new Color32 (0, 0, 0, 255);
			otherDeclineBtn.GetComponentInChildren<Image>().color = new Color32 (0, 0, 0, 255);
		}

		// update label
		localPlayerOffer.transform.FindChild("Offer").FindChild("OfferTxt").gameObject.GetComponent<Text>().text = "My Offer";
		otherPlayerOffer.transform.FindChild ("Offer").FindChild ("OfferTxt").gameObject.GetComponent<Text> ().text = "Other player offer";

		foreach (Button b in otherPlayerOffer.GetComponentsInChildren<Button>()) {
			b.enabled = false;
		}

		// update player offer respectively
		foreach (InputField f in otherPlayerOffer.GetComponentsInChildren<InputField>()) {
			StealableType type = transformNameToStealableType [f.transform.name];
			int currentOffering = 0;
			if (getOtherPlayerIdInTrade () == "Player1") {
				currentOffering = currentTrade.GetPlayer1OfferingFor (type);
			} else {
				currentOffering = currentTrade.GetPlayer2OfferingFor (type);
			}

			f.textComponent.text = "" + currentOffering;
			Debug.Log (f.name + " = " + currentOffering);
		}

	}

}
