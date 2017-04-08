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
			StartCoroutine(GameManager.GUI.ShowMessage ("You don't have enough " + transform + " to trade that amount."));
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

	void Update () {
		if (!GetComponent<UIWindow> ().IsVisible) {
			return;
		}

		// deactivate other player's offer GUI elements
		GameObject otherPlayerOffer = transform.FindChild ("Content").FindChild ("Pages").FindChild ("Page").FindChild (getOtherPlayerIdInTrade()).gameObject;
		foreach (InputField f in otherPlayerOffer.GetComponentsInChildren<InputField>()) {
			f.DeactivateInputField ();
			f.enabled = false;
		}

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

			f.text = "" + currentOffering;
		}
	}

}
