using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIntersection : MonoBehaviour {

	public Intersection referencedIntersection;

	void OnMouseEnter() {
		GetComponent<SpriteRenderer> ().color = Color.blue;

	}

	void OnMouseExit() {
		GetComponent<SpriteRenderer> ().color = Color.red;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
