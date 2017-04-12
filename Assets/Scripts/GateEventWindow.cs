using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateEventWindow : MonoBehaviour {

	public GateEvent CurrentEvent;

	void Start () {
		CurrentEvent = null;
	}

	public void ClickRollDice() {
		// TODO : fix this (levelOfThatColor / trade / science / politics) / 6
		AbstractProgressCard drawnProgressCard = GameManager.Instance.GetCurrentGameState ().CurrentProgressCardDeck.DrawCardOfType (CurrentEvent.CardType);
		List<int> allowedValues = new List<int> ();
		PlayerImprovement currentImprovement = GameManager.Instance.GetCurrentGameState ().CurrentPlayerImprovements.GetImprovementForPlayer (GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName);
		if (CurrentEvent.CardType == AbstractProgressCard.ProgressCardType.Politic) {
			for (int i = 0; i < (int)currentImprovement.CurrentPoliticsImprovement; i++) {
				allowedValues.Add (i);
			}
		} else if (CurrentEvent.CardType == AbstractProgressCard.ProgressCardType.Science) {
			for (int i = 0; i < (int)currentImprovement.CurrentScienceImprovement; i++) {
				allowedValues.Add (i);
			}
		} else if (CurrentEvent.CardType == AbstractProgressCard.ProgressCardType.Trade) {
			for (int i = 0; i < (int)currentImprovement.CurrentTradeImprovement; i++) {
				allowedValues.Add (i);
			}
		}

		int rollNum = UnityEngine.Random.Range (0, 7);
		if (allowedValues.Contains(rollNum)) {
			// get the progress card
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().AddProgressCard (drawnProgressCard);
		} else {
			StartCoroutine(GameManager.GUI.ShowMessage ("Unfortunately you did not get any progress card."));
		}

		GetComponent<UIWindow> ().Hide ();
	}

	// Update is called once per frame
	void Update () {
		if (!GetComponent<UIWindow> ().IsOpen) {
			return;
		}

		if (CurrentEvent == null) {
			return;
		}

		if (CurrentEvent.GateOutcome == RollDiceScript.EventDiceOutcome.City_Gates_Blue) {
			transform.FindChild ("Content").FindChild ("Grid").FindChild ("Txt1").GetComponent<Text> ().text = "The event dice landed on a blue gate.";
		} else if (CurrentEvent.GateOutcome == RollDiceScript.EventDiceOutcome.City_Gates_Red) {
			transform.FindChild ("Content").FindChild ("Grid").FindChild ("Txt1").GetComponent<Text> ().text = "The event dice landed on a red gate.";
		} else if (CurrentEvent.GateOutcome == RollDiceScript.EventDiceOutcome.City_Gates_Green) {
			transform.FindChild ("Content").FindChild ("Grid").FindChild ("Txt1").GetComponent<Text> ().text = "The event dice landed on a green gate.";
		}

		GameObject playerCardPreview = transform.FindChild ("Content").FindChild ("ProgressCardPreview").FindChild ("ProgressCardFront").gameObject;
		playerCardPreview.GetComponent<UIProgressCardFront> ().CurrentCard = GameManager.Instance.GetCurrentGameState ().CurrentProgressCardDeck.DrawCardOfType (CurrentEvent.CardType);
		playerCardPreview.GetComponent<UIProgressCardFront> ().Turned = false;
		playerCardPreview.GetComponent<UIProgressCardFront> ().CardSelected = true;
	}
}
