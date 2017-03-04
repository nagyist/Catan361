using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GUIInterface : MonoBehaviour {
    private Canvas guiCanvas;

    public IEnumerator ShowMessage(string msg, float delay = 1.75f) {
        Text guiText = guiCanvas.GetComponent<Text>();
        guiText.text = msg;
        guiText.enabled = true;
        yield return new WaitForSeconds(delay);
        guiText.enabled = false;
    }

	// Use this for initialization
	void Start () {
        guiCanvas = GameObject.FindObjectOfType<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
