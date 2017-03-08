using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GUIInterface : MonoBehaviour {
	private GameObject guiCanvas;

    public IEnumerator ShowMessage(string msg, float delay = 1.75f) {
		GameObject popupPanel = guiCanvas.transform.Find ("PanelPopup").gameObject;
		GameObject popupPanelText = popupPanel.transform.FindChild ("Text").gameObject;

		popupPanelText.GetComponent<Text> ().text = msg;
		popupPanel.gameObject.SetActive (true);
		yield return new WaitForSeconds (delay);
		popupPanel.gameObject.SetActive (false);
    }

	public void ShowHexActionWindow(UIHex hexTile) {
		if (hasModalWindowOpened ()) { return; }

		GameObject actionPanel = guiCanvas.transform.Find ("PanelHexActions").gameObject;
		actionPanel.GetComponent<RectTransform>().position = Input.mousePosition;
		actionPanel.SetActive (true);
	}

	public void ToggleCurrentPlayerTurnManagement() {
		
	}

	private bool hasModalWindowOpened() {
		GameObject actionPanel = guiCanvas.transform.Find ("PanelHexActions").gameObject;

		return actionPanel.activeSelf;
	}


	// Use this for initialization
	void Awake () {
		guiCanvas = GameObject.FindGameObjectWithTag ("GameCanvas");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
