using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayer : NetworkBehaviour {
    public string myName = "";
    public Color myColor = Color.white;
	public Dictionary<StealableType, int> playerResources = new Dictionary<StealableType, int> ();

	public bool placedSettlement = false;
	public bool placedRoad = false;

	public override void OnStartClient() {
		// register itself as a connected player
		GameManager.PlayerConnected (gameObject);
	}

	public override void OnStartLocalPlayer() {
		// register as local player
		GameManager.SetLocalPlayer (gameObject);
	}

	public override bool Equals(object obj)
	{
		var item = obj as GamePlayer;

		if (item == null)
		{
			return false;
		}

		return item.myName == this.myName;
	}

	[Command]
	public void CmdTakeTurn() {
		// make sure that it is the current player turn
		if (GameManager.Instance.GetCurrentGameState().CurrentTurn.PlayerTakeTurn(this.myName)) {
			Debug.Log ("Player took turn!");
		}

		GameManager.Instance.GetCurrentGameState ().SyncGameTurns ();
	}

	[Command]
	public void CmdEndTurn() {
		// make sure that it is the current player turn
		if (GameManager.Instance.GetCurrentGameState().CurrentTurn.EndTurn (this.myName)) {
			Debug.Log ("Player Ended term");
		}

		GameManager.Instance.GetCurrentGameState ().SyncGameTurns ();
	}

	[Command]
	public void CmdBuildSettlement(byte[] vec3sSerialized) {
		Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject (vec3sSerialized) as Vec3[];
		Intersection i = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.getIntersection (new List<Vec3> (vec3Pos));

		int settlementIdx = i.SettlementCount;
		i.SettlementCount++;
		i.SettlementOwner = this.myName;
		i.SettlementLevels.Add (settlementIdx, 1);

		GameManager.Instance.GetCurrentGameState ().CurrentIntersections.setIntersection (vec3Pos, i);
		GameManager.Instance.GetCurrentGameState ().SyncGameBoard ();
	}

	public void CmdBuildRoad(byte[] vec3Serialized) {
		Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject (vec3Serialized) as Vec3[];
		Edge currentEdge = GameManager.Instance.GetCurrentGameState ().CurrentEdges.getEdge (vec3Pos [0], vec3Pos [1]);

		currentEdge.Owner = this.myName;
		currentEdge.IsOwned = true;

		GameManager.Instance.GetCurrentGameState ().CurrentEdges.setEdge (vec3Pos[0], vec3Pos[1], currentEdge);
		GameManager.Instance.GetCurrentGameState ().SyncGameBoard ();
	}
	void Start () {
		
	}
	
	void Update () {
		
	}
}
