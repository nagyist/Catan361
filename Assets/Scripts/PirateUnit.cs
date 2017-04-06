using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateUnit : Singleton<PirateUnit> {

	public Vector2 HexGridPosition;
	public Vec3 HexGridCubePosition;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if(! GameManager.Instance.GameStateReady() ||
			GameManager.Instance.GetCurrentGameState().CurrentStatus < GameState.GameStatus.GRID_CREATED) {
			return;
		}

		if (GameManager.Instance.GetCurrentGameState ().CurrentPiratePosition.IsPlaced) {
			HexGrid currentGrid = GameManager.Instance.GetCurrentGameState().GetComponent<HexGrid> ();

			MoveTo (currentGrid.cubeHexes [GameManager.Instance.GetCurrentGameState ().CurrentPiratePosition.PlacementPos].GetComponent<UIHex>());
		}
	}

	public void MoveTo(UIHex moveTo) {
		HexGridPosition = moveTo.HexGridPosition;
		HexGridCubePosition = moveTo.HexGridCubePosition;

		transform.position = moveTo.GetComponentInChildren<TextMesh>().transform.position;
	}

	public PirateUnit(Vector2 HexGridPosition, Vec3 HexGridCubePosition)
	{
		this.HexGridPosition = HexGridPosition;
		this.HexGridCubePosition = HexGridCubePosition;
	}

	public void ActivateRobber()
	{
		if (!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED))
		{
			return;
		}

		if (GameManager.Instance.GetCurrentGameState().CurrentTurn.IsLocalPlayerTurn())
		{
		}

	}
}
