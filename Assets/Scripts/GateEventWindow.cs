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
		int rollNum = UnityEngine.Random.Range (1, 4);
		AbstractProgressCard drawnProgressCard = GameManager.Instance.GetCurrentGameState ().CurrentProgressCardDeck.DrawCardOfType (CurrentEvent.CardType);
		if (drawnProgressCard != null/*rollNum == 1*/) {
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
