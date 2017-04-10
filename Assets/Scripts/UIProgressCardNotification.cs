using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressCardNotification : MonoBehaviour {

	public AbstractProgressCard CurrentCard;

	void Start () {
		CurrentCard = null;
	}

	// Update is called once per frame
	void Update () {
		if (!GetComponent<UIWindow> ().IsOpen) {
			return;
		}

		if (CurrentCard == null) {
			return;
		}

		GetComponentInChildren<UIProgressCardFront> ().CurrentCard = CurrentCard;
		GetComponentInChildren<UIProgressCardFront> ().Turned = true;
		GetComponentInChildren<UIProgressCardFront> ().CardSelected = true;
	}
}
