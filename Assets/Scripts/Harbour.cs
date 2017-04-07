using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harbour : MonoBehaviour {

	public int exchangeRate;
	public StealableType returnedResource;
	public int returnedAmount;
	public Edge harbourEdge;

	void OnMouseEnter(){
		GetComponentInChildren<SpriteRenderer>().color = new Color32 (0, 255, 0, 255);
	}

	void OnMouseExit(){
		GetComponentInChildren<SpriteRenderer>().color = new Color32 (255, 255, 255, 255);
	}

	void OnMouseDown() {
		/*
		if(harbourEdge.)
		{
			launch popup window for trade
		}
		*/
	}

	// Update is called once per frame
	void Update () {
		
	}
}
