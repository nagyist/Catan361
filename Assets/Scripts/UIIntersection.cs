using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIIntersection : MonoBehaviour {

	public Vec3 HexPos1;
	public Vec3 HexPos2;
	public Vec3 HexPos3;

	void OnMouseEnter() {
		if (GameManager.Instance.GameStateReadyAtStage (GameState.GameStatus.GRID_CREATED)) {
			Intersection i = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.getIntersection (new List<Vec3> (new Vec3[] { HexPos1, HexPos2, HexPos3 }));

			GameObject mouseOver = GameObject.FindGameObjectWithTag ("GameCanvas").transform.FindChild ("MouseOverInfos").gameObject;
			mouseOver.GetComponent<Text> ().text = 
				"Level = " + i.SettlementLevel + "\n";
			mouseOver.GetComponent<Transform> ().position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			mouseOver.SetActive (true);
		} else {
			GetComponent<SpriteRenderer> ().color = Color.blue; 
		}
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
	
		GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer> ();
		if (GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsInSetupPhase () && localPlayer.placedSettlement) {
			Debug.Log ("You already placed a settlement during your turn");
			return;
		}

		Intersection intersection = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.getIntersection (new List<Vec3> (new Vec3[] { HexPos1, HexPos2, HexPos3 }));
		if (intersection.SettlementLevel > 0 && intersection.SettlementOwner != GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName) {
			Debug.Log ("Does not own the settlement");
			return;
		} else if (intersection.SettlementLevel == 1 && intersection.SettlementOwner == GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName) {
			// upgrade the settlement to city
			Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int> () {
				{StealableType.Resource_Ore, 3},
				{StealableType.Resource_Grain, 2}
			};

			if (!localPlayer.HasEnoughResources (requiredRes)) {
				Debug.Log ("Does not have enough resource to upgrade to city");
				return;
			}

			localPlayer.ConsumeResources (requiredRes);
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdUpgradeSettlement (
				SerializationUtils.ObjectToByteArray (new Vec3[] { HexPos1, HexPos2, HexPos3 })
			);
			return;
		}

		if (!GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsInSetupPhase ()) {
			Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int> () {
				{StealableType.Resource_Brick, 1},
				{StealableType.Resource_Lumber, 1},
				{StealableType.Resource_Wool, 1},
				{StealableType.Resource_Grain, 1}
			};

			if (!localPlayer.HasEnoughResources (requiredRes)) {
				Debug.Log ("Not enough resources");
				return;
			}

			localPlayer.ConsumeResources (requiredRes);
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
			if (i.SettlementLevel > 0) {
				GetComponent<SpriteRenderer> ().color = GameManager.ConnectedPlayersByName [i.SettlementOwner].GetComponent<GamePlayer> ().GetPlayerColor ();
			}
		}
	}
}
