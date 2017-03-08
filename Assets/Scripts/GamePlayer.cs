using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayer : NetworkBehaviour {
    public string myName = "";
    public Color myColor = Color.white;
	public Dictionary<StealableType, int> playerResources = new Dictionary<StealableType, int> ();

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

	void Start () {
		
	}
	
	void Update () {
		
	}
}
