using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveRobberPirate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void ClickAcceptMove() 
	{
		// TODO : how to block the program flow an await for an answer
		ToggleGroup toggles = gameObject.GetComponentInChildren<ToggleGroup>();
		IEnumerator<Toggle> togglesEnum = toggles.ActiveToggles().GetEnumerator();
		togglesEnum.MoveNext();
		Toggle toggle = togglesEnum.Current;
		string resourceSelected = toggle.transform.parent.name;
		string entityType = "";

		if (resourceSelected == "RobberBtn") {
			entityType = "robber";
		} else if (resourceSelected == "PirateBtn") {
			entityType = "pirate";
		}

		GameEventManager.Instance.HandleMoveRobberPirateDecision (entityType);

		GameManager.GUI.HideMoveRobberPiratePopup ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
