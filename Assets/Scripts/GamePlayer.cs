using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GamePlayer : NetworkBehaviour {
    public string myName = "";
    public Color myColor = Color.white;

	public override void OnStartClient() {
		// register itself as a connected player
		GameManager.Instance.PlayerConnected (gameObject);
	}

	public override void OnStartLocalPlayer() {
		// register as local player
		GameManager.Instance.SetLocalPlayer (gameObject);
	}

	void Start () {
		
	}
	
	void Update () {
		
	}
}
