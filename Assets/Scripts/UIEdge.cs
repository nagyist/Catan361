using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEdge : MonoBehaviour {

	public Edge referencedEdge;

	void OnMouseEnter() {
		GetComponent<SpriteRenderer> ().color = Color.red;
	}

	void OnMouseExit() {
		GetComponent<SpriteRenderer> ().color = Color.black;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
