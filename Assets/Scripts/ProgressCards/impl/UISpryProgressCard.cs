using System;
using UnityEngine;

public class UISpryProgressCard : MonoBehaviour
{
	public AbstractProgressCard CurrentCard;

	void Start() {
		StartCoroutine (GameManager.GUI.ShowMessage("You may steal a progress card from any opponent's hand."));
		GameManager.GUI.PostStatusMessage ("Steal any progress card from an opponent's hand.");
	}

	public void ClickStealCard(AbstractProgressCard cardToSteal, string stealFrom) {
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdAddProgressCard (GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName, SerializationUtils.ObjectToByteArray (cardToSteal));
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdRemoveProgressCard (stealFrom, SerializationUtils.ObjectToByteArray (cardToSteal));

		StartCoroutine (GameManager.GUI.ShowMessage("You stole a progress card from " + stealFrom));

		GameManager.LocalPlayer.GetComponent<GamePlayer> ().spyProgressCardUsed = false;
		CurrentCard.RemoveFromPlayerHand ();
		Destroy (gameObject);
	}

	void Update() {
	
	}
}
