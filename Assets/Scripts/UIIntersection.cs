using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// note: more development needed on OnMouseDown function
//          in order to represent buying a settlement
// for test purposes assume current player's color is cyan


public class UIIntersection : MonoBehaviour {

	public Intersection referencedIntersection;

    private SpriteRenderer intersectionRenderer;

	void OnMouseEnter() {
        intersectionRenderer.color = Color.yellow;
	}

    
	void OnMouseExit() {
        // check to see if intersection is owned
        if (intersectionRenderer.color != Color.cyan)
		    intersectionRenderer.color = Color.red;
	}
    

	// Use this for initialization
	void Start () {
        intersectionRenderer = GetComponent<SpriteRenderer>();
        intersectionRenderer.sortingLayerName = "Intersection";
        intersectionRenderer.color = Color.red;
	}

    // not complete
    // we can change the OnMouseDown function to represent buying a settlement
    // we can just change the color of the intersection to that of the player
    // waiting until concept of player can be fully utlized
    private void OnMouseDown()
    {
        intersectionRenderer.color = Color.cyan;
    }

    // Update is called once per frame
    

	// Update is called once per frame
	void Update () {

	}
}
