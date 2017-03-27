using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberUnit : MonoBehaviour {

    public Vector2 HexGridPosition;
    public Vec3 HexGridCubePosition;

    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {

        if (!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED))
        {
            return;
        }


    }

    public RobberUnit(Vector2 HexGridPosition, Vec3 HexGridCubePosition)
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
