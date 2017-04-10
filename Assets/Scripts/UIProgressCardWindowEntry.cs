using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressCardWindowEntry : MonoBehaviour {

	public AbstractProgressCard CurrentCard;

	// Use this for initialization
	void Start () {
		
	}

	public void SetCard(AbstractProgressCard card) {
		CurrentCard = card;
		transform.FindChild ("CardTitle").GetComponent<Text> ().text = card.GetTitle ();
		if (card.CardType == AbstractProgressCard.ProgressCardType.Science) {
			transform.FindChild ("CardTypeScience").gameObject.SetActive (true);
		} else if (card.CardType == AbstractProgressCard.ProgressCardType.Politic) {
			transform.FindChild ("CardTypePolitics").gameObject.SetActive (true);
		} else if (card.CardType == AbstractProgressCard.ProgressCardType.Trade) {
			transform.FindChild ("CardTypeTrade").gameObject.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
