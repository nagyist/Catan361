using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// this class holds information about game players
public class GamePlayer : NetworkBehaviour {

    // initialize some variables
    public string myName;
    public Color myColor;

    // vars to check if the player has placed anything during the current turn
    public bool placedSettlement = false;
    public bool placedRoad = false;
	public bool placedKnight = false;

    // player can have maximum of 3 basic, strong, might knights and city walls each
	public int numBasicKnights = 0;
	public int numStrongKnights = 0;
	public int numMightyKnights = 0;
    public int numCityWalls = 0;

    // boolean used to represent check if the player has the following city improvements
	public bool hasFortress = false;    

    // vars to hold to player's selection depending on what the player clicks on 
    public UIIntersection selectedUIIntersection = null;
	public UIEdge selectedUIEdge = null;

    // dictionary containing playe colors
    private static Dictionary<string, Color> playerColors = new Dictionary<string, Color>() {
		{"Player1", new Color32(100, 149, 237, 255)},
		{"Player2", new Color32(255, 165, 0, 255)},
		{"Player3", new Color32(152, 251, 152, 255)}
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

    // function used to reset the player's selection
	private void resetBuildSelection() {
		if (this.selectedUIIntersection != null) {
			this.selectedUIIntersection.IsSelected = false;
			this.selectedUIIntersection = null;
		}

		if (this.selectedUIEdge != null) {
			this.selectedUIEdge.IsSelected = false;
			this.selectedUIEdge = null;
		}
	}


    // functions to add a player's game selection
	public void SetBuildSelection(UIIntersection intersection) {
		resetBuildSelection ();

		this.selectedUIIntersection = intersection;
		intersection.IsSelected = true;
	}
	public void SetBuildSelection(UIEdge edge) {
		resetBuildSelection ();

		this.selectedUIEdge = edge;
		edge.IsSelected = true;
	}

    // getter for player's resources
	public ResourceCollection.PlayerResourcesCollection GetPlayerResources() {
		return GameManager.Instance.GetCurrentGameState ().CurrentResources.GetPlayerResources (myName);
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

    /* [Command] tag means this is a command function
     * Command functions are called form the client and run on the server
     * Function name must start with Cmd
     */
     // command to show a message to everyone
    [Command]
	public void CmdShowMessage(string message, float delay) {
		GameManager.Instance.GetCurrentGameState ().RpcClientShowMessage (message, delay);
	}

    // command to take a turn
	[Command]
	public void CmdTakeTurn() {
		// make sure that it is the current player turn
		if (GameManager.Instance.GetCurrentGameState().CurrentTurn.PlayerTakeTurn(this.myName)) {
			CmdShowMessage (this.myName + " turn begins.", 2.0f);
			GameManager.Instance.GetCurrentGameState ().RpcClientPostStatusMessage (this.myName + " took his turn.");
		}
		this.placedKnight = false;
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

        Village village = new Village();

        // get the position where the player has clicked
		Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject (vec3sSerialized) as Vec3[];
        // get the intersection located at that at that intersection
		Intersection i = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.getIntersection (new List<Vec3> (vec3Pos));


        // if in round two, place city, otherwise place settlement
        int roundCount = GameManager.Instance.GetCurrentGameState().CurrentTurn.RoundCount;
        if (roundCount == 1)
        {
            // create the settlement at the intersection
            village.myKind = Village.VillageKind.City;
            i.Owner = this.myName;
            i.unit = village;
        } 
        else
        {
            // create the settlement at the intersection
            village.myKind = Village.VillageKind.Settlement;
            i.Owner = this.myName;
            i.unit = village;
        }
	

        // add the intersection to the game manager
		GameManager.Instance.GetCurrentGameState ().CurrentIntersections.setIntersection (vec3Pos, i);
        // publish the intersection through an rpc function call
		GameManager.Instance.GetCurrentGameState ().RpcPublishIntersection (vec3sSerialized, SerializationUtils.ObjectToByteArray (i));
	}

	// command function for hiring a knight  
	[Command]
	public void CmdHireKnight(byte[] vec3sSerialized) {

        // transform argument into intersection
        Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject(vec3sSerialized) as Vec3[];
        Intersection i = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(vec3Pos));

        // check for non-empty intersection
        if (i.unit != null)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("Intersection already contains a " + i.unit.GetType().ToString() +"."));
            return;
        }
        // check for max number of basic knights 
        else if (numBasicKnights >= 2)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("You already have the maximum number of basic knights"));
            return;
        }
        // check if local player has already placed knight 
        else if (placedKnight)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("You already placed a knight during your turn"));
            return;
        }


        // check resources
        Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int>() {
            {StealableType.Resource_Ore, 1},
            {StealableType.Resource_Wool, 1},
        };
        if (!HasEnoughResources(requiredRes))
        {
            StartCoroutine(GameManager.GUI.ShowMessage("Not enough resources."));
            return;
        }

        // hire a basic knight
        CmdConsumeResources(requiredRes);
        Knight knight = new Knight();
		i.Owner = this.myName;
		i.unit = knight;
        numBasicKnights++;
        placedKnight = true;
        resetBuildSelection();
        StartCoroutine(GameManager.GUI.ShowMessage("You have hired a knight."));

        // set and publish the intersection
        GameManager.Instance.GetCurrentGameState ().CurrentIntersections.setIntersection (vec3Pos, i);
		GameManager.Instance.GetCurrentGameState ().RpcPublishIntersection (vec3sSerialized, SerializationUtils.ObjectToByteArray (i));

        return;
    }

    // command function for activating a knight
    [Command]
    public void CmdActivateKnight(byte[] vec3sSerialized)
    {
     
        // get the position where the player has clicked
        Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject(vec3sSerialized) as Vec3[];
        // get the intersection located at that at that intersection
        Intersection i = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(vec3Pos));

        Knight knight = (Knight)(i.unit);
        knight.active = true;

        // add the intersection to the game manager
        GameManager.Instance.GetCurrentGameState().CurrentIntersections.setIntersection(vec3Pos, i);
        // publish the intersection through an rpc function call
        GameManager.Instance.GetCurrentGameState().RpcPublishIntersection(vec3sSerialized, SerializationUtils.ObjectToByteArray(i));
    
    }

    // command function for hiring a knight  
    [Command]
	public void CmdUpgradeKnight(byte[] vec3sSerialized) {

        // get the position where the player has clicked
		Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject (vec3sSerialized) as Vec3[];
        // get the intersection located at that at that intersection
		Intersection i = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.getIntersection (new List<Vec3> (vec3Pos));

		// increment the knight level
		Knight knight = (Knight)(i.unit);
		knight.level++;
	
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
        if (i.unit.GetType() == typeof(Village))
        {
            Village village = (Village)(i.unit);
            if (village.myKind == Village.VillageKind.Settlement)
            {
                village.myKind = Village.VillageKind.City;
            }
            else
            {
                Debug.Log("The village is not a settlement, it might have been upgrade already.");
            }
                
        }

        // add the new intersetcion to the game manager
		GameManager.Instance.GetCurrentGameState ().CurrentIntersections.setIntersection (vec3Pos, i);
        // publish the intersection through an rpc function call
		GameManager.Instance.GetCurrentGameState ().RpcPublishIntersection (vec3sSerialized, SerializationUtils.ObjectToByteArray (i));
	}

    // command function for upgrading a settlement 
    [Command]
    public void CmdBuildCityWall(byte[] vec3sSerialized)
    {
        // get the vector position where the player has clicked
        Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject(vec3sSerialized) as Vec3[];
        // get the intersecion at that postition
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(vec3Pos));

        // check to see if there's currently a unit on the intersection
        if (intersection.unit != null)
        {
            if (intersection.Owner != myName)
            {
                StartCoroutine(GameManager.GUI.ShowMessage("You do not own this intersection."));
                return;
            }
            // check for non-village unit
            else if (intersection.unit.GetType() != typeof(Village))
            {
                StartCoroutine(GameManager.GUI.ShowMessage("The intersection contains a knight."));
                return;
            }
            // check for max number of walls 
            else if (numCityWalls >= 3)
            {
                StartCoroutine(GameManager.GUI.ShowMessage("You already have 3 city walls."));
                return;
            }

            Village village = (Village)intersection.unit;
            // if the player's village is a settlement
            if (village.myKind == Village.VillageKind.Settlement)
            {
                StartCoroutine(GameManager.GUI.ShowMessage("Settlements cannot have a city wall."));
                return;
            }
            else if (village.cityWall)
            {
                StartCoroutine(GameManager.GUI.ShowMessage("City already has a city wall."));
                return;
            }
            else
            {
                // check for resources
                Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int>() {
                        {StealableType.Resource_Brick, 2},
                    };
                if (!HasEnoughResources(requiredRes))
                {
                    Debug.Log("Does not have enough resource to upgrade to city");
                    return;
                }

                // build city walls
                CmdConsumeResources(requiredRes);
                village.cityWall = true;
                numCityWalls++;
                resetBuildSelection();
                StartCoroutine(GameManager.GUI.ShowMessage("You have placed a city wall."));           

                // set and publish the intersection         
                GameManager.Instance.GetCurrentGameState().CurrentIntersections.setIntersection(vec3Pos, intersection);
                GameManager.Instance.GetCurrentGameState().RpcPublishIntersection(vec3sSerialized, SerializationUtils.ObjectToByteArray(intersection));

                return;
            }
            
        }
       
        StartCoroutine(GameManager.GUI.ShowMessage("You have selected an empty intersection."));
        return;
        
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

	[Command]
	public void CmdUpdateResource(StealableType type, int newAmount) {
		// update on server
		GameManager.Instance.GetCurrentGameState ().CurrentResources.UpdateResource (myName, type, newAmount);
		// post update to clients
		GameManager.Instance.GetCurrentGameState ().RpcClientPostResourceUpdate (myName, type, newAmount);
	}

    // returns true if the player's resources are more than the requiredRes arguments
	public bool HasEnoughResources(Dictionary<StealableType, int> requiredRes) {
		return GameManager.Instance.GetCurrentGameState ().CurrentResources.PlayerHasEnoughResources (myName, requiredRes);
	}

	[Command]
	public void CmdConsumeResources(byte[] requiredResSerialized) {
		Dictionary<StealableType, int> requiredRes = (Dictionary<StealableType, int>) SerializationUtils.ByteArrayToObject (requiredResSerialized);
		// update server game state
		bool result = GameManager.Instance.GetCurrentGameState ().CurrentResources.PlayerConsumeResources (myName, requiredRes);
		if (result) {
			foreach (StealableType key in requiredRes.Keys) {
				int newAmount = GetPlayerResources () [key];
				// push update to clients
				GameManager.Instance.GetCurrentGameState ().RpcClientPostResourceUpdate (myName, key, newAmount);
			}
		} 
	}

	[Command]
	public void CmdHandleMoveRobberPirateEntity(string entityType, byte[] moveToPosSerialized) {
		GameManager.Instance.GetCurrentGameState ().RpcClientMoveRobberPirateEntity (entityType, moveToPosSerialized);
	}

	[Command]
	public void CmdRequestPlayerTrade(string fromPlayer) {
		
	}

    
	public void CmdConsumeResources(Dictionary<StealableType, int> requiredRes) {
		CmdConsumeResources(SerializationUtils.ObjectToByteArray(requiredRes));
	}

	void Start () {
		
	}
	
	void Update () {
		 
	}
}
