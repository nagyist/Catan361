using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayNumber : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var parent = transform.parent;

		var parentRenderer = parent.GetComponent<Renderer>();
		var renderer = GetComponent<Renderer>();
		renderer.sortingLayerID = parentRenderer.sortingLayerID;
		renderer.sortingOrder = parentRenderer.sortingOrder;

		var spriteTransform = parent.transform;
		var text = GetComponent<TextMesh>();
		var pos = spriteTransform.position;

		Hex hexScript = parent.GetComponent<Hex> ();
		text.text = string.Format("" + hexScript.selectedNum, pos.x, pos.y);
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
