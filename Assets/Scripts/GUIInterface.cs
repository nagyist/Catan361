using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GUIInterface : MonoBehaviour {
    private Canvas guiCanvas;

    public IEnumerator ShowMessage(string msg, float delay = 1.75f) {
		GameObject popupPanel = guiCanvas.transform.Find ("PanelPopup").gameObject;
		GameObject popupPanelText = popupPanel.transform.FindChild ("Text").gameObject;

		popupPanelText.GetComponent<Text> ().text = msg;
		popupPanel.gameObject.SetActive (true);
		yield return new WaitForSeconds (delay);
		popupPanel.gameObject.SetActive (false);
    }

	// Use this for initialization
	void Start () {
        guiCanvas = GameObject.FindObjectOfType<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
