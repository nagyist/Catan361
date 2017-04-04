using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

		if(GameManager.Instance.GetCurrentGameState().CurrentBoard[hexScript.HexGridCubePosition].SelectedNum != 0 &&
			GameManager.Instance.GetCurrentGameState().CurrentBoard[hexScript.HexGridCubePosition].SelectedNum2 == 0)
		{
			text.text = string.Format("" + GameManager.Instance.GetCurrentGameState().CurrentBoard[hexScript.HexGridCubePosition].SelectedNum, pos.x, pos.y);
		}

		if(GameManager.Instance.GetCurrentGameState().CurrentBoard[hexScript.HexGridCubePosition].FishingNum != 0)
		{
			text.text = string.Format("" + GameManager.Instance.GetCurrentGameState().CurrentBoard[hexScript.HexGridCubePosition].FishingNum, pos.x, pos.y);
		}

		if(GameManager.Instance.GetCurrentGameState().CurrentBoard[hexScript.HexGridCubePosition].SelectedNum2 != 0)
		{
			text.text = string.Format("" + GameManager.Instance.GetCurrentGameState().CurrentBoard[hexScript.HexGridCubePosition].SelectedNum2 + "|" +
				GameManager.Instance.GetCurrentGameState().CurrentBoard[hexScript.HexGridCubePosition].SelectedNum3 + Environment.NewLine +  
				GameManager.Instance.GetCurrentGameState().CurrentBoard[hexScript.HexGridCubePosition].SelectedNum4 + "|" +
				GameManager.Instance.GetCurrentGameState().CurrentBoard[hexScript.HexGridCubePosition].SelectedNum5, pos.x, pos.y);
		}


	}
}
