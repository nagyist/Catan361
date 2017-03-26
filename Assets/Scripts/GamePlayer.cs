using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// this class holds information about game players
public class GamePlayer : NetworkBehaviour {

    // initialize some variables
    public string myName;
    public Color myColor;
    public bool placedSettlement = false;
    public bool placedRoad = false;

    // dictionary containing playe colors
    private static Dictionary<string, Color> playerColors = new Dictionary<string, Color>() {
		{"Player1", new Color32(100, 149, 237, 255)},
		{"Player2", new Color32(255, 165, 0, 255)},
		{"Player3", new Color32(152, 251, 152, 255)}
	};

    // dictionary holding players stealable resources
	public Dictionary<StealableType, int> playerResources = new Dictionary<StealableType, int> () {
		{StealableType.Resource_Brick, 100},
		{StealableType.Resource_Grain, 100},
		{StealableType.Resource_Lumber, 100},
		{StealableType.Resource_Ore, 100},
		{StealableType.Resource_Wool, 100}
		// Now must include Fish, possibly gold?
	};

    // add the the current player game object to the list of connected players
	public override void OnStartClient() {
		// register itself as a connected player
		GameManager.PlayerConnected (gameObject);
	}

    // set the current game object as the local player 
	public override void OnStartLocalPlayer() {
		// register as local player
		GameManager.SetLocalPlayer (gameObject);
	}

    // remake of the equals function
	public override bool Equals(object obj)
	{
        // if argument is null, return false
		var item = obj as GamePlayer;
		if (item == null)
		{
			return false;
		}

        // compare the names and return boolean
		return item.myName == this.myName;
	}

    // getter for the player's color
	public Color32 GetPlayerColor() {
        // return new value if player's name isn't listed in playerColors dictionary
		if (!playerColors.ContainsKey (this.myName)) {
			return new Color32 (153, 50, 204, 255);
		}
        // otherwise return player's color
        else {
			return playerColors [this.myName];
		}
	}

	[Command]
	public void CmdShowMessage(string message, float delay) {
		GameManager.Instance.GetCurrentGameState ().RpcClientShowMessage (message, delay);
	}

    /* [Command] tag means this is a command function
     * Command functions are called form the client and run on the server
     * Function name must start with Cmd
     */
	[Command]
	public void CmdTakeTurn() {
		// make sure that it is the current player turn
		if (GameManager.Instance.GetCurrentGameState().CurrentTurn.PlayerTakeTurn(this.myName)) {
			CmdShowMessage (this.myName + " turn begins.", 2.0f);
			GameManager.Instance.GetCurrentGameState ().RpcClientPostStatusMessage (this.myName + " took his turn.");
		}
        // synchronize game turns
		GameManager.Instance.GetCurrentGameState ().SyncGameTurns ();
	}

    // command function for ending a player turn
	[Command]
	public void CmdEndTurn() {
		// make sure that it is the current player turn
		if (GameManager.Instance.GetCurrentGameState().CurrentTurn.EndTurn (this.myName)) {
			GameManager.Instance.GetCurrentGameState ().RpcClientPostStatusMessage (this.myName + " ended his turn.");
		}
        // synchronize game turns
		GameManager.Instance.GetCurrentGameState ().SyncGameTurns ();
	}

    // command function for building a settlement 
	[Command]
	public void CmdBuildSettlement(byte[] vec3sSerialized) {
        // get the position where the player has clicked
		Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject (vec3sSerialized) as Vec3[];
        // get the intersection located at that at that intersection
		Intersection i = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.getIntersection (new List<Vec3> (vec3Pos));


        // if in round two, place city, otherwise place settlement
        int roundCount = GameManager.Instance.GetCurrentGameState().CurrentTurn.RoundCount;
        if (roundCount == 1)
        {
            // create the settlement at the intersection
            i.SettlementLevel += 2;
            i.Owner = this.myName;
        } 
        else
        {
            // create the settlement at the intersection
            i.SettlementLevel += 1;
            i.Owner = this.myName;
        }
	

        // add the intersection to the game manager
		GameManager.Instance.GetCurrentGameState ().CurrentIntersections.setIntersection (vec3Pos, i);
        // publish the intersection through an rpc function call
		GameManager.Instance.GetCurrentGameState ().RpcPublishIntersection (vec3sSerialized, SerializationUtils.ObjectToByteArray (i));
	}

    // command function for upgrading a settlement 
	[Command]
	public void CmdUpgradeSettlement(byte[] vec3sSerialized) {
        // get the vector position where the player has clicked
        Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject (vec3sSerialized) as Vec3[];
        // get the intersecion at that postition
		Intersection i = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.getIntersection (new List<Vec3> (vec3Pos));

        // increment the settlement count
		i.SettlementLevel++;

        // add the new intersetcion to the game manager
		GameManager.Instance.GetCurrentGameState ().CurrentIntersections.setIntersection (vec3Pos, i);
        // publish the intersection through an rpc function call
		GameManager.Instance.GetCurrentGameState ().RpcPublishIntersection (vec3sSerialized, SerializationUtils.ObjectToByteArray (i));
	}

    // command function for building a road
	[Command]
	public void CmdBuildRoad(byte[] vec3Serialized) {
        // get the vector position where the player has clicked
		Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject (vec3Serialized) as Vec3[];
        // get the edge at that position
		Edge currentEdge = GameManager.Instance.GetCurrentGameState ().CurrentEdges.getEdge (vec3Pos [0], vec3Pos [1]);

        // create the road on the edge
		currentEdge.Owner = this.myName;
		currentEdge.IsOwned = true;

        // add the adge to the game manager
		GameManager.Instance.GetCurrentGameState ().CurrentEdges.setEdge (vec3Pos[0], vec3Pos[1], currentEdge);
        // publish the edge though an rpc function call
		GameManager.Instance.GetCurrentGameState ().RpcPublishEdge (vec3Serialized, SerializationUtils.ObjectToByteArray (currentEdge));
	}

    // returns true if the player's resources are more than the requiredRes arguments
	public bool HasEnoughResources(Dictionary<StealableType, int> requiredRes) {

        // iterate through required resources
		foreach(StealableType key in requiredRes.Keys) {
            // return false if player lacks any of the resource types 
			if(!this.playerResources.ContainsKey(key)) {
				return false;
			}
            // return false if the player doesn't have enough of the current resource
			int playerAmount = this.playerResources[key];
			if(playerAmount < requiredRes[key]) {
				return false;
			}
		}

		return true;
	}

    // consumes player resources defiend by requiredRes
	public bool ConsumeResources(Dictionary<StealableType, int> requiredRes) {

        // returns false if the player does the required resouces
		if(!HasEnoughResources(requiredRes)) { return false; }

        // iterate though the required resources and decrement the player's amounts
		foreach(StealableType key in requiredRes.Keys) {
			int playerAmount = this.playerResources[key];
			int newAmount = playerAmount - requiredRes[key];
			this.playerResources[key] = newAmount;
		}

		return true;
	}

	void Start () {
		
	}
	
	void Update () {
		
	}
}
