using System;
using UnityEngine;

public class UIDeserterProgressCard : MonoBehaviour
{
	public AbstractProgressCard CurrentCard;

	void Start() {
		StartCoroutine (GameManager.GUI.ShowMessage ("Choose any of your opponent's knight."));
	}

	public void ClickChoseKnightIntersection(Intersection ChosenKnightIntersection) {
		if (!GameManager.LocalPlayer.GetComponent<GamePlayer> ().deserterProgressCardUsed) {
			return;
		}

		IntersectionUnit intersectionUnit = ChosenKnightIntersection.unit;
		if (intersectionUnit == null || intersectionUnit.GetType () != typeof(Knight)) {
			StartCoroutine (GameManager.GUI.ShowMessage ("You must select a valid knight."));
			ChosenKnightIntersection = null;
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().deserterProgressCardUsed = false;
			return;
		}

		Knight selectedKnight = (Knight)intersectionUnit;
		if (ChosenKnightIntersection.Owner == GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName) {
			StartCoroutine (GameManager.GUI.ShowMessage ("The selected knight cannot be yours!"));
			ChosenKnightIntersection = null;
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().deserterProgressCardUsed = false;
			return;
		}

		string previousOwner = ChosenKnightIntersection.Owner;
		selectedKnight.Owner = GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName;

		ChosenKnightIntersection.unit = selectedKnight;
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdUpdateIntersection (ChosenKnightIntersection.getKey (), SerializationUtils.ObjectToByteArray (ChosenKnightIntersection));
		CurrentCard.RemoveFromPlayerHand ();

		GameManager.Instance.GetCurrentGameState ().RpcClientShowMessage (GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName + " converted " + previousOwner + "'s knight with Deserter card.", 2.0f);

		GameManager.LocalPlayer.GetComponent<GamePlayer> ().deserterProgressCardUsed = false;
		Destroy (gameObject);
	}
}
