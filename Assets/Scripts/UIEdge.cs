using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// note: more development needed on OnMouseDown function
//          in order to represent buying a settlement
// for test purposes assume current player's color is cyan
public class UIEdge : MonoBehaviour {

	public Edge referencedEdge;

    SpriteRenderer edgeRenderer;



	void OnMouseEnter() {
		edgeRenderer.color = Color.yellow;

	}

    
	void OnMouseExit() {
        if (edgeRenderer.color != Color.cyan)
            edgeRenderer.color = Color.black;
	}
    



    // not complete
    // we can change the OnMouseDown function to represent buying a road
    // we can just change the color of the edge to that of the player
    // waiting until concept of player can be fully utlized
    private void OnMouseDown()
    {
        edgeRenderer.color = Color.cyan;
    }

    // Use this for initialization
    void Start () {
        edgeRenderer = GetComponent<SpriteRenderer>();
        edgeRenderer.sortingLayerName = "Edge";
        edgeRenderer.color = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
