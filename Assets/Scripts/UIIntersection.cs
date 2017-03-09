using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIIntersection : MonoBehaviour {

	public Vec3 HexPos1;
	public Vec3 HexPos2;
	public Vec3 HexPos3;

	void OnMouseEnter() {
		if (GameManager.Instance.GameStateReadyAtStage (GameState.GameStatus.GRID_CREATED)) {
			Intersection i = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.getIntersection (new List<Vec3> (new Vec3[] { HexPos1, HexPos2, HexPos3 }));
			Debug.Log ("Settlement count = " + i.SettlementCount);
		}
		GetComponent<SpriteRenderer> ().color = Color.blue;
	}

	void OnMouseDown() {
		if (!GameManager.Instance.GameStateReadyAtStage (GameState.GameStatus.GRID_CREATED)) {
			Debug.Log ("Grid not created");
			return;
		}

		if (!GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsLocalPlayerTurn ()) {
			Debug.Log ("Is not local player turn");
			return;
		}

		if (!GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsInSetupPhase ()) {
			Debug.Log ("Is not in setup phase");
			return;
		}

		Intersection intersection = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.getIntersection (new List<Vec3> (new Vec3[] { HexPos1, HexPos2, HexPos3 }));
		if (intersection.SettlementCount > 0 && intersection.SettlementOwner != GameManager.LocalPlayer.GetComponent<GamePlayer>().myName) {
			Debug.Log ("Does not own the settlement");
			return;
		}
			
		GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer> ();
		if (localPlayer.placedSettlement) {
			Debug.Log ("You already placed a settlement during your turn");
			return;
		}

		localPlayer.placedSettlement = true;

		Debug.Log ("Created the settlement");
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdBuildSettlement (
			SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 })
		);
	}

	void OnMouseExit() {
		GetComponent<SpriteRenderer> ().color = Color.red;
	}

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer>().sortingLayerName = "intersection";
	}

	// Update is called once per frame
	void Update () {
		if (GameManager.Instance.GameStateReadyAtStage (GameState.GameStatus.GRID_CREATED)) {
			Intersection i = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.getIntersection (new List<Vec3> (new Vec3[] { HexPos1, HexPos2, HexPos3 }));
			if (i.SettlementCount > 0) {
				GetComponent<SpriteRenderer> ().color = Color.green;
			}
		}
	}
}
