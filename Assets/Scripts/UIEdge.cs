using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEdge : MonoBehaviour {

	public Vec3 HexPos1 { get; set; }
	public Vec3 HexPos2 { get; set; }

	void OnMouseEnter() {
		GetComponent<SpriteRenderer> ().color = Color.red;

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

		Edge currentEdge = GameManager.Instance.GetCurrentGameState ().CurrentEdges.getEdge (HexPos1, HexPos2);
		if (currentEdge.IsOwned) {
			Debug.Log ("Edge is already owned");
			return;
		}

		GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer> ();
		if (GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsInSetupPhase () && localPlayer.placedRoad) {
			Debug.Log ("You already placed a road");
			return;
		}

		if (!GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsInSetupPhase ()) {
			Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int> () {
				{StealableType.Resource_Brick, 1},
				{StealableType.Resource_Lumber, 1}
			};

			if (!localPlayer.HasEnoughResources (requiredRes)) {
				Debug.Log ("Not enough resources");
				return;
			}

			localPlayer.ConsumeResources (requiredRes);
		}

		Debug.Log ("Create road");
		localPlayer.placedRoad = true;
		localPlayer.CmdBuildRoad(SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2 }));
	}

	void OnMouseExit() {
		GetComponent<SpriteRenderer> ().color = Color.black;
	}

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer>().sortingLayerName = "edge";
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.Instance.GameStateReadyAtStage (GameState.GameStatus.GRID_CREATED)) {			
			Edge e = GameManager.Instance.GetCurrentGameState ().CurrentEdges.getEdge (HexPos1, HexPos2);
			if (e == null) { return; }
			if (e.IsOwned) {
				GetComponent<SpriteRenderer> ().color = GameManager.ConnectedPlayersByName [e.Owner].GetComponent<GamePlayer> ().GetPlayerColor ();
			}
		}
	}
}
