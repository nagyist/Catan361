using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIntersection : MonoBehaviour {

	public Intersection referencedIntersection;

    private SpriteRenderer intersectionRenderer;

	void OnMouseEnter() {
		GetComponent<SpriteRenderer> ().color = Color.blue;

	}

	void OnMouseExit() {
		GetComponent<SpriteRenderer> ().color = Color.red;
	}

	// Use this for initialization
	void Start () {
        intersectionRenderer = GetComponent<SpriteRenderer>();
        intersectionRenderer.sortingLayerName = "Intersection";
	}

	// Update is called once per frame
	void Update () {

	}
}
