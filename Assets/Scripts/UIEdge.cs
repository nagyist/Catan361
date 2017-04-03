using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEdge : MonoBehaviour {

	public Vec3 HexPos1 { get; set; }
	public Vec3 HexPos2 { get; set; }
	public bool IsSelected = false;

	void OnMouseEnter() {
		GetComponent<SpriteRenderer> ().color = Color.red;
	}

	void OnMouseDown() {
		GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
		localPlayer.SetBuildSelection (this);
		return;
	}

	public void ConstructRoad() {
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

		if (!isConnectedToOwnedUnit ()) {
			StartCoroutine (GameManager.GUI.ShowMessage ("You must place a road adjacent to any intersection!"));
			return;
		}

		if (!GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsInSetupPhase ()) {
			Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int> () {
				{ StealableType.Resource_Brick, 1 },
				{ StealableType.Resource_Lumber, 1 }
			};

			if (!localPlayer.HasEnoughResources (requiredRes)) {
				Debug.Log ("Not enough resources");
				return;
			}

			localPlayer.ConsumeResources (requiredRes);
		} else {
			if (!localPlayer.placedSettlement) {
				StartCoroutine(GameManager.GUI.ShowMessage ("You must place a settlement first."));
				return;
			}
		}

		localPlayer.placedRoad = true;
		localPlayer.CmdBuildRoad(SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2 }));
	}

	private bool isConnectedToOwnedUnit() {
		GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer> ();
		List<List<Vec3>> adjIntersections1 = UIHex.getIntersectionsAdjacentPos (this.HexPos1);
		List<List<Vec3>> adjIntersections2 = UIHex.getIntersectionsAdjacentPos (this.HexPos2);

		// get the intersection of both list
		Dictionary<Intersection, List<Vec3>> potentialIntersectionsIntersection = new Dictionary<Intersection, List<Vec3>>(); // hacky way of doing an intersection lel
		foreach (List<Vec3> intersection1 in adjIntersections1) {
			Intersection i = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.getIntersection (intersection1);
			if (!potentialIntersectionsIntersection.ContainsKey (i)) {
				potentialIntersectionsIntersection.Add(i, intersection1);
			}
		}
		foreach (List<Vec3> intersection2 in adjIntersections2) {
			Intersection i = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.getIntersection (intersection2);
			if (!potentialIntersectionsIntersection.ContainsKey (i)) {
				potentialIntersectionsIntersection.Add(i, intersection2);
			}
		}

		// check if any of the intersections intersection is owned by the local player
		foreach(Intersection i in potentialIntersectionsIntersection.Keys) {
			if (i.Owner == localPlayer.myName) {
				return true;
			}
		}

		return false;
	}

	void OnMouseExit() {
		GetComponent<SpriteRenderer> ().color = new Color (0.0f, 0.0f, 0.0f, 0.3f);
	}

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer>().sortingLayerName = "edge";
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameManager.Instance.GameStateReadyAtStage (GameState.GameStatus.GRID_CREATED)) {			
			return;
		}

		Edge e = GameManager.Instance.GetCurrentGameState ().CurrentEdges.getEdge (HexPos1, HexPos2);
		if (e == null) { return; }

		if (e.IsOwned) {
			GetComponent<SpriteRenderer> ().color = GameManager.ConnectedPlayersByName [e.Owner].GetComponent<GamePlayer> ().GetPlayerColor ();
			return;
		}

		if (e.isHarbour == true) {
			GetComponent<SpriteRenderer> ().color = Color.yellow;
			return;
		}

		if (this.IsSelected) {
			GetComponent<SpriteRenderer> ().color = Color.green;
			return;
		} else {
			GetComponent<SpriteRenderer> ().color = new Color(0, 0, 0, 55);
			return;
		}

	}
}
