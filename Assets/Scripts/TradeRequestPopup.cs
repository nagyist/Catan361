using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeRequestPopup : MonoBehaviour {

	public Trade tradeObj = null;
	public bool active = false;

	void Start () {
		
	}

	public void ClickAcceptTradeRequest() {
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdAnswerTradeRequest (SerializationUtils.ObjectToByteArray (tradeObj), true);
		GetComponent<UIWindow> ().Hide ();
	}

	public void ClickDeclineTradeRequest() {
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdAnswerTradeRequest (SerializationUtils.ObjectToByteArray (tradeObj), false);
		GetComponent<UIWindow> ().Hide ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!GetComponent<UIWindow> ().IsVisible) {
			return;
		}

		transform.FindChild ("Content").FindChild("ContentGrid").gameObject.GetComponentInChildren<Text> ().text = 
			tradeObj.Player1 + " would like to trade with you.\nDo you wish to trade ?";
	}
}
