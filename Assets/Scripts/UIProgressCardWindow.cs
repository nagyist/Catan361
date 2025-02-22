﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressCardWindow : MonoBehaviour {

	private Dictionary<AbstractProgressCard, GameObject> displayedHand;
	private GameObject ProgressCardEntry;
	private AbstractProgressCard SelectedCard;
	public string GamePlayerName;
	public bool OtherPlayer = false;

	public void Clear() {
		GamePlayerName = "";
		OtherPlayer = false;
		SelectedCard = null;
	}

	// Use this for initialization
	void Start () {
		displayedHand = new Dictionary<AbstractProgressCard, GameObject> ();
		ProgressCardEntry = transform.FindChild ("Content").FindChild ("Scroll View").FindChild ("Cards Grid").FindChild ("ProgressCardEntry").gameObject;
	}

	GamePlayer getPlayer() {
		if (OtherPlayer == false) {
			return GameManager.LocalPlayer.GetComponent<GamePlayer> ();
		} else {
			return GameManager.ConnectedPlayersByName [GamePlayerName].GetComponent<GamePlayer> ();
		}
	}

	GameObject getCardPreview() {
		return transform.FindChild ("Content").FindChild ("CardPreview").FindChild("ProgressCardFront").gameObject;
	}

	public void CloseWindow() {
		GetComponent<UIWindow> ().Hide ();
	}

	public void ClickUseProgressCard() {
		SelectedCard.ExecuteCardEffect ();
		getCardPreview ().GetComponent<UIProgressCardFront> ().CurrentCard = null;
		getCardPreview ().GetComponent<UIProgressCardFront> ().Turned = false;
		getCardPreview ().GetComponent<UIProgressCardFront> ().CardSelected = false;

		CloseWindow ();
	}

	public void ClickSelectedCard() {
		ToggleGroup toggles = gameObject.GetComponentInChildren<ToggleGroup>();
		IEnumerator<Toggle> togglesEnum = toggles.ActiveToggles().GetEnumerator();
		togglesEnum.MoveNext();
		Toggle toggle = togglesEnum.Current;
		SelectedCard = toggle.gameObject.GetComponent<UIProgressCardWindowEntry>().CurrentCard;
		Debug.Log (SelectedCard);
		getCardPreview ().GetComponent<UIProgressCardFront> ().CurrentCard = SelectedCard;
		getCardPreview ().GetComponent<UIProgressCardFront> ().Turned = false;
		getCardPreview ().GetComponent<UIProgressCardFront> ().CardSelected = true;
	}

	public void ClickStealCard() {
		if (GameManager.LocalPlayer.GetComponent<GamePlayer> ().spyProgressCardUsed) {
			GameManager.LocalPlayer.GetComponent<UISpryProgressCard> ().ClickStealCard (SelectedCard, getPlayer ().myName);
			//CloseWindow ();
		}
	}

	// Update is called once per frame
	void Update () {
		if (!GetComponent<UIWindow> ().IsVisible) {
			return;
		}

		if (!GameManager.Instance.GameStateReadyAtStage (GameState.GameStatus.GRID_CREATED)) {
			return;
		}

		if (getPlayer ().myName == GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName) {
			transform.FindChild ("Content").FindChild ("CardPreview").FindChild ("StealProgressCardBtn").gameObject.SetActive (false);
		} else {
			if (GameManager.LocalPlayer.GetComponent<GamePlayer> ().spyProgressCardUsed) {
				transform.FindChild ("Content").FindChild ("CardPreview").FindChild ("StealProgressCardBtn").gameObject.SetActive (true);
			} else {
				transform.FindChild ("Content").FindChild ("CardPreview").FindChild ("StealProgressCardBtn").gameObject.SetActive (false);
			}
		}

		if (SelectedCard == null) {
			transform.FindChild ("Content").FindChild ("CardPreview").FindChild ("StealProgressCardBtn").gameObject.GetComponent<Button> ().enabled = false;
			transform.FindChild ("Content").FindChild ("CardPreview").FindChild ("UseProgressCardBtn").gameObject.GetComponent<Button> ().enabled = false;
		} else {
			if (!SelectedCard.IsStealable ()) {
				transform.FindChild ("Content").FindChild ("CardPreview").FindChild ("StealProgressCardBtn").gameObject.GetComponent<Button> ().enabled = false;
			} else {
				transform.FindChild ("Content").FindChild ("CardPreview").FindChild ("StealProgressCardBtn").gameObject.GetComponent<Button> ().enabled = true;
			}

			if (!SelectedCard.IsConsumable()) {
				transform.FindChild ("Content").FindChild ("CardPreview").FindChild ("UseProgressCardBtn").gameObject.SetActive (false);
			} else {
				transform.FindChild ("Content").FindChild ("CardPreview").FindChild ("UseProgressCardBtn").gameObject.SetActive (true);
				transform.FindChild ("Content").FindChild ("CardPreview").FindChild ("UseProgressCardBtn").gameObject.GetComponent<Button> ().enabled = true;
			}

		}

		transform.FindChild ("Header").FindChild ("Text").GetComponent<Text> ().text = getPlayer ().myName + "'s PROGRESS CARD";

		List<AbstractProgressCard> currentHand = GameManager.Instance.GetCurrentGameState ().CurrentProgressCardHands.GetCardsForPlayer (getPlayer ().myName);
		List<AbstractProgressCard> removed = displayedHand.Keys.Where (x => !currentHand.Contains (x)).ToList ();
		List<AbstractProgressCard> added = currentHand.Where (x => !displayedHand.ContainsKey (x)).ToList ();

		foreach (AbstractProgressCard toRemove in removed) {
			Destroy (displayedHand [toRemove]);
			displayedHand.Remove (toRemove);
		}

		foreach (AbstractProgressCard toAdd in added) {
			GameObject newCard = Instantiate<GameObject> (ProgressCardEntry);
			newCard.GetComponent<UIProgressCardWindowEntry> ().SetCard (toAdd);
			newCard.transform.parent = ProgressCardEntry.transform.parent;
			newCard.transform.localScale = ProgressCardEntry.transform.localScale;
			newCard.SetActive (true);
			displayedHand.Add (toAdd, newCard);
		}
	}
}
