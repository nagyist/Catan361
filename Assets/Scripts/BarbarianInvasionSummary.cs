using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarbarianInvasionSummary : MonoBehaviour {

	public BarbarianInvasion CurrentInvasion;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!GetComponent<UIWindow> ().IsOpen) {
			return;
		}

		if (CurrentInvasion.CurrentOutcome == BarbarianInvasion.OutcomeType.BarbarianAttacked) {
			transform.FindChild ("BarbarianWin").gameObject.SetActive (true);
			transform.FindChild ("BarbarianWin").FindChild ("Content").FindChild ("Grid").FindChild ("NumPillagedCities").gameObject.GetComponent<Text> ().text = "" + CurrentInvasion.PillagedCitiesCount;
			transform.FindChild ("BarbarianWin").FindChild ("Content").FindChild ("Grid").FindChild ("PlayerPillagedCities").gameObject.GetComponent<Text> ().text = CurrentInvasion.PillagedPlayer;
		} else if (CurrentInvasion.CurrentOutcome == BarbarianInvasion.OutcomeType.KnightDefended) {
			transform.FindChild ("BarbarianWin").gameObject.SetActive (true);
		}
	}
}
