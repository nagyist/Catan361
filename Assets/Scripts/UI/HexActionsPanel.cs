using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexActionsPanel : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}

	private void closeModalWindowIfNeeded() {
		if (Input.GetMouseButton(0) && 
			!RectTransformUtility.RectangleContainsScreenPoint (
				gameObject.GetComponent<RectTransform> (), 
				Input.mousePosition, 
				null)) {
			gameObject.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		closeModalWindowIfNeeded ();
	}
}
