using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayNumber : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var parent = transform.parent;

		var parentRenderer = parent.GetComponent<Renderer>();
		var renderer = GetComponent<Renderer>();
		renderer.sortingLayerID = parentRenderer.sortingLayerID;
		renderer.sortingOrder = parentRenderer.sortingOrder;

		var spriteTransform = parent.transform;
		var text = GetComponent<TextMesh>();
		var pos = spriteTransform.position;

		UIHex hexScript = parent.GetComponent<UIHex> ();

		if (!GameManager.Instance.GameStateReady() || 
			GameManager.Instance.GetCurrentGameState().CurrentStatus < GameState.GameStatus.GRID_CREATED) {
			return;
		}

		text.text = string.Format("" + GameManager.Instance.GetCurrentGameState().CurrentBoard[hexScript.HexGridCubePosition].SelectedNum, pos.x, pos.y);
	}
}
