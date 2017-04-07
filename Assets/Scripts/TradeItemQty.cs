using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TradeItemQty : MonoBehaviour {

	public int TradingQty;

	// Use this for initialization
	void Start () {
		
	}

	public void ClickChangeQty() {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.FindChild ("Button").gameObject.GetComponentInChildren<Text> ().text = "" + TradingQty;
		if (Input.GetKeyUp (KeyCode.Return)) {
			GetComponent<InputField> ().DeactivateInputField();
		}
	}
}
