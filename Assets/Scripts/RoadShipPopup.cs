using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq.Expressions;

public class RoadShipPopup : MonoBehaviour {

	// SoadBtn
	// ShipBtn
	enum OptionSelected {
		BUILD_ROAD,
		BUILD_SHIP,
		UNKNOWN
	}

	public void ClickAccept() {
		// TODO : how to block the program flow an await for an answer
		ToggleGroup toggles = gameObject.GetComponentInChildren<ToggleGroup>();
		IEnumerator<Toggle> togglesEnum = toggles.ActiveToggles().GetEnumerator();
		togglesEnum.MoveNext();
		Toggle toggle = togglesEnum.Current;
		string resourceSelected = toggle.transform.parent.name;
		OptionSelected selectedOption = OptionSelected.UNKNOWN;

		if (resourceSelected == "RoadBtn") {
			selectedOption = OptionSelected.BUILD_ROAD;
		} else if (resourceSelected == "ShipBtn") {
			selectedOption = OptionSelected.BUILD_SHIP;
		}

		//this.returnLambda(selectedOption);
	}

	public void ClickCancel() {
		GetComponent<UIWindow> ().Hide ();
	}
}
