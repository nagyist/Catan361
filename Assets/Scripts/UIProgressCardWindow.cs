using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressCardWindow : MonoBehaviour {

	private Dictionary<AbstractProgressCard, GameObject> displayedHand;
	private GameObject ProgressCardEntry;
	private AbstractProgressCard SelectedCard;
	private int i = 0;

	// Use this for initialization
	void Start () {
		displayedHand = new Dictionary<AbstractProgressCard, GameObject> ();
		ProgressCardEntry = transform.FindChild ("Content").FindChild ("Scroll View").FindChild ("Cards Grid").FindChild ("ProgressCardEntry").gameObject;
	}

	GamePlayer getPlayer() {
		return GameManager.LocalPlayer.GetComponent<GamePlayer>();
	}

	GameObject getCardPreview() {
		return transform.FindChild ("Content").FindChild ("CardPreview").FindChild("ProgressCardFront").gameObject;
	}

	public void CloseWindow() {
		GetComponent<UIWindow> ().Hide ();
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
	
	// Update is called once per frame
	void Update () {
		if (!GetComponent<UIWindow> ().IsOpen) {
			return;
		}

		if (!GameManager.Instance.GameStateReadyAtStage (GameState.GameStatus.GRID_CREATED)) {
			return;
		}

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
