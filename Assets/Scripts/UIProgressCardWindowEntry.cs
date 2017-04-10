using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressCardWindowEntry : MonoBehaviour {

	public AbstractProgressCard CurrentCard;

	// Use this for initialization
	void Start () {
		CurrentCard = null;
	}

	public void SetCard(AbstractProgressCard card) {
		CurrentCard = card;
		transform.FindChild ("CardTitle").GetComponent<Text> ().text = card.GetTitle ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
