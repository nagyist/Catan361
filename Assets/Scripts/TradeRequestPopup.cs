using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeRequestPopup : MonoBehaviour {

	public Trade tradeObj = null;
	public bool active = false;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (active == false) {
			return;
		}

		transform.FindChild ("Content").FindChild("ContentGrid").gameObject.GetComponentInChildren<Text> ().text = 
			tradeObj.Player1 + " would like to trade with you.\nDo you wish to trade ?";
	}
}
