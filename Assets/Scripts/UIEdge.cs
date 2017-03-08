using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEdge : MonoBehaviour {

	public Edge referencedEdge;

    SpriteRenderer edgeRenderer;

	void OnMouseEnter() {
		GetComponent<SpriteRenderer> ().color = Color.red;

	}

	void OnMouseExit() {
		GetComponent<SpriteRenderer> ().color = Color.black;
	}

    private void OnMouseDown()
    {
        GetComponent<SpriteRenderer>().color = Color.cyan;
    }

    // Use this for initialization
    void Start () {
        edgeRenderer = GetComponent<SpriteRenderer>();
        edgeRenderer.sortingLayerName = "Edge";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
