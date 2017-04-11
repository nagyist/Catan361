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
    public bool placedSettlement = false;
    public bool placedRoad = false;
	public bool placedKnight = false;
	public int numBasicKnights = 0;
	public int numStrongKnights = 0;
	public int numMightyKnights = 0;
    public int numCityWalls = 0;

	// progress card
	public bool craneProgressCardDiscount = false;
	public bool engineerProgressCardDiscount = false;

	//can promote strong knights to mighty knights
	public bool hasFortress = false;

	//2:1 on all maritime trade
	public bool hasTradingHouse = false;

	//take a resource / commodity of your choice from the bank when you miss out on resources from 
	//dice roll
	public bool hasAqueduct = false;

    public UIIntersection selectedUIIntersection = null;
	public UIEdge selectedUIEdge = null;

	public UIIntersection uiIntersectionToMove = null;
	public UIEdge uiEdgeToMove = null;

	// queue to hold all knights the player must move 
    public Queue<Knight> knightsToMove = new Queue<Knight>();


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

	public void ResetBuildSelection() {
		if (this.selectedUIIntersection != null) {
			this.selectedUIIntersection.IsSelected = false;
			this.selectedUIIntersection = null;
		}

		if (this.selectedUIEdge != null) {
			this.selectedUIEdge.IsSelected = false;
			this.selectedUIEdge = null;
		}
	}

	public void SetBuildSelection(UIIntersection intersection) {
		ResetBuildSelection ();

		this.selectedUIIntersection = intersection;
		intersection.IsSelected = true;
	}

	public void SetBuildSelection(UIEdge edge) {
		ResetBuildSelection ();

		this.selectedUIEdge = edge;
		edge.IsSelected = true;
	}

	public void SetMoveSelection()
	{
		if (selectedUIEdge != null)
			this.uiEdgeToMove = selectedUIEdge;
		else if (selectedUIIntersection != null)
			this.uiIntersectionToMove = selectedUIIntersection;
		return;
	}

	public void ResetMoveSelection()
	{
		this.uiEdgeToMove = null;
		this.uiIntersectionToMove = null;
		return;
	}


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

		this.placedKnight = false;
        ResetBuildSelection();
		ResetMoveSelection();

        // go through all the intersections
        foreach (string key in GameManager.Instance.GetCurrentGameState().CurrentIntersections.Intersections.Keys)
        {
            Intersection i = GameManager.Instance.GetCurrentGameState().CurrentIntersections.Intersections[key];
            // check to see if it isn't empty
            if (i.Owner == myName && i.unit.GetType() == typeof(Knight))
            {
                Knight k = (Knight)i.unit;
                k.hasBeenPromotedThisTurn = false;
                k.hasMovedThisTurn = false;
            }
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

    
	// command function for building settlement
    [Command]
	public void CmdBuildSettlement(byte[] vec3sSerialized) 
	{
		Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject (vec3sSerialized) as Vec3[];
		Intersection intersection = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.getIntersection (new List<Vec3> (vec3Pos));
        int roundCount = GameManager.Instance.GetCurrentGameState().CurrentTurn.RoundCount;

        // build a new village
        Village village = new Village();
        if (roundCount == 1)
            village.myKind = Village.VillageKind.City;
        intersection.Owner = myName;
        intersection.unit = village;
        this.placedSettlement = true;

        // set and publish the intersection
        GameManager.Instance.GetCurrentGameState ().CurrentIntersections.setIntersection (vec3Pos, intersection);
		GameManager.Instance.GetCurrentGameState ().RpcPublishIntersection (vec3sSerialized, SerializationUtils.ObjectToByteArray (intersection));
	}

	// command for building a city wall
    [Command]
    public void CmdBuildCityWall(byte[] vec3sSerialized)
    {
        Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject(vec3sSerialized) as Vec3[];
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(vec3Pos));
        int roundCount = GameManager.Instance.GetCurrentGameState().CurrentTurn.RoundCount;

        // add city wall to village
        Village village = (Village)intersection.unit;
        village.cityWall = true;
        this.numCityWalls++;

        // set and publish the intersection
        GameManager.Instance.GetCurrentGameState().CurrentIntersections.setIntersection(vec3Pos, intersection);
        GameManager.Instance.GetCurrentGameState().RpcPublishIntersection(vec3sSerialized, SerializationUtils.ObjectToByteArray(intersection));
    }

    // command function for upgrading a settlement 
    [Command]
    public void CmdUpgradeSettlement(byte[] vec3sSerialized)
    {
        Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject(vec3sSerialized) as Vec3[];
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(vec3Pos));

        // upgrade the settlement to a city
        Village village = (Village)(intersection.unit);
        village.myKind = Village.VillageKind.City;

        // set and publish the intersection
        GameManager.Instance.GetCurrentGameState().CurrentIntersections.setIntersection(vec3Pos, intersection);
        GameManager.Instance.GetCurrentGameState().RpcPublishIntersection(vec3sSerialized, SerializationUtils.ObjectToByteArray(intersection));
    }

	// command for hiring a knight
    [Command]
    public void CmdHireKnight(byte[] vec3sSerialized)
    {
        Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject(vec3sSerialized) as Vec3[];
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(vec3Pos));

        // hire a knight
        Knight knight = new Knight();
        numBasicKnights++;
        this.placedKnight = true;
        intersection.unit = knight;
        intersection.Owner = myName;

        // set and publish the intersection
        GameManager.Instance.GetCurrentGameState().CurrentIntersections.setIntersection(vec3Pos, intersection);
        GameManager.Instance.GetCurrentGameState().RpcPublishIntersection(vec3sSerialized, SerializationUtils.ObjectToByteArray(intersection));

    }

    // command function for activating a knight
    [Command]
    public void CmdActivateKnight(byte[] vec3sSerialized)
    {
        Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject(vec3sSerialized) as Vec3[];
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(vec3Pos));
        
		// activate the knight
        Knight knight = (Knight)intersection.unit;
        knight.active = true;
    
        // set and publish the intersection
        GameManager.Instance.GetCurrentGameState().CurrentIntersections.setIntersection(vec3Pos, intersection);
        GameManager.Instance.GetCurrentGameState().RpcPublishIntersection(vec3sSerialized, SerializationUtils.ObjectToByteArray(intersection));

    }

    // command function for activating a knight
    [Command]
    public void CmdUpgradeKnight(byte[] vec3sSerialized)
    {
        // unserialize the argument
        Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject(vec3sSerialized) as Vec3[];
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(vec3Pos));

		// upgrade the knight
        Knight knight = (Knight)intersection.unit;
        knight.level++;
        knight.hasBeenPromotedThisTurn = true;

        // set and publish the intersection
        GameManager.Instance.GetCurrentGameState().CurrentIntersections.setIntersection(vec3Pos, intersection);
        GameManager.Instance.GetCurrentGameState().RpcPublishIntersection(vec3sSerialized, SerializationUtils.ObjectToByteArray(intersection));

    }

    // command function for building a road
    [Command]
	public void CmdBuildRoad(byte[] vec3Serialized) 
	{
		Vec3[] vec3Pos = SerializationUtils.ByteArrayToObject (vec3Serialized) as Vec3[];
		Edge currentEdge = GameManager.Instance.GetCurrentGameState ().CurrentEdges.getEdge (vec3Pos [0], vec3Pos [1]);

        // create the road on the edge
        this.placedRoad = true;
        currentEdge.Owner = this.myName;

        // set and publish the intersection
        GameManager.Instance.GetCurrentGameState ().CurrentEdges.setEdge (vec3Pos[0], vec3Pos[1], currentEdge);
		GameManager.Instance.GetCurrentGameState ().RpcPublishEdge (vec3Serialized, SerializationUtils.ObjectToByteArray (currentEdge));
	}

	// command used to reposition a replaced knight
	[Command]
	public void CmdReplaceKnight(byte[] vec3pos)
	{
		// get position and intersection
		Vec3[] newPos = SerializationUtils.ByteArrayToObject(vec3pos) as Vec3[];
        Intersection newIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(newPos));

		// dequeue a knight and add it to the new intersection
        Knight newKnight = knightsToMove.Dequeue();
        newIntersection.Owner = this.myName;
        newIntersection.unit = newKnight;

        // set and publish the new intersection
        GameManager.Instance.GetCurrentGameState().CurrentIntersections.setIntersection(newPos, newIntersection);
        GameManager.Instance.GetCurrentGameState().RpcPublishIntersection
            (SerializationUtils.ObjectToByteArray(newPos), SerializationUtils.ObjectToByteArray(newIntersection));

        return;
    }

    // command used to move a knight unit
    [Command]
    public void CmdMoveUnitWithReplacement(byte[] oldvec3, byte[] newvec3, byte[] name, byte[] knight)
    {
        // get the positions and intersections from the arguments
        Vec3[] oldPos = SerializationUtils.ByteArrayToObject(oldvec3) as Vec3[];
        Vec3[] newPos = SerializationUtils.ByteArrayToObject(oldvec3) as Vec3[];
		String oldOwnerName = SerializationUtils.ByteArrayToObject(name) as String;
		Knight replacedKnight = SerializationUtils.ByteArrayToObject(knight) as Knight;

        Intersection oldIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(oldPos));
        Intersection newIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(newPos));

        if (oldIntersection.unit != null)
        {
            if (oldIntersection.unit.GetType() == typeof(Knight))
            {
                // store the knight 
                Knight k = (Knight)oldIntersection.unit;

                // update the old intersection
                oldIntersection.Owner = "";
                oldIntersection.unit = null;

                // set and publish the old intersection
                GameManager.Instance.GetCurrentGameState().CurrentIntersections.setIntersection(oldPos, oldIntersection);
                GameManager.Instance.GetCurrentGameState().RpcPublishIntersection(oldvec3, SerializationUtils.ObjectToByteArray(oldIntersection));

                // update the new intersection
                newIntersection.Owner = this.myName;
                newIntersection.unit = k;

                // set and publish the new intersection
                GameManager.Instance.GetCurrentGameState().CurrentIntersections.setIntersection(newPos, newIntersection);
                GameManager.Instance.GetCurrentGameState().RpcUpdatePlayerKnights(name, knight);
                GameManager.Instance.GetCurrentGameState().RpcPublishIntersection(newvec3, SerializationUtils.ObjectToByteArray(newIntersection));
            }
        }

    }

	// command used to move a knight unit
	[Command]
	public void CmdMoveUnit(byte[] oldvec3, byte[] newvec3)
	{
		// get the positions and intersections from the arguments
		Vec3[] oldPos = SerializationUtils.ByteArrayToObject(oldvec3) as Vec3[];
		Vec3[] newPos = SerializationUtils.ByteArrayToObject(oldvec3) as Vec3[];
		Intersection oldIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(oldPos));
		Intersection newIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(newPos));

		if (oldIntersection.unit != null)
		{
			if (oldIntersection.unit.GetType() == typeof(Knight))
            {
                // store the knight 
                Knight k = (Knight)oldIntersection.unit;

                // update the old intersection
                oldIntersection.Owner = "";
                oldIntersection.unit = null;

                // set and publish the old intersection
                GameManager.Instance.GetCurrentGameState().CurrentIntersections.setIntersection(oldPos, oldIntersection);
                GameManager.Instance.GetCurrentGameState().RpcPublishIntersection(oldvec3, SerializationUtils.ObjectToByteArray(oldIntersection));

                // update the new intersection
                newIntersection.Owner = this.myName;
                newIntersection.unit = k;

                // set and publish the new intersection
                GameManager.Instance.GetCurrentGameState().CurrentIntersections.setIntersection(newPos, newIntersection);
                GameManager.Instance.GetCurrentGameState().RpcPublishIntersection(newvec3, SerializationUtils.ObjectToByteArray(newIntersection));
            }
		}
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
	public void CmdAddResourcesResources(byte[] requiredResSerialized) {
		Dictionary<StealableType, int> requiredRes = (Dictionary<StealableType, int>) SerializationUtils.ByteArrayToObject (requiredResSerialized);
		// update server game state
		bool result = GameManager.Instance.GetCurrentGameState ().CurrentResources.PlayerAddResources (myName, requiredRes);
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
	public void CmdSendTradeRequest(byte[] tradeSerialized) {
		GameManager.Instance.GetCurrentGameState ().RpcClientTradeRequest (tradeSerialized);
	}

	[Command]
	public void CmdAnswerTradeRequest(byte[] tradeSerialized, bool answer) {
		GameManager.Instance.GetCurrentGameState ().RpcClientAnswerTradeRequest (tradeSerialized, answer);
	}

	[Command]
	public void CmdUpdateTradeOffer(byte[] tradeSerialized) {
		GameManager.Instance.GetCurrentGameState ().RpcClientUpdateTradeOffer (tradeSerialized);
	}

	[Command]
	public void CmdProceedToTrade(byte[] tradeSerialized) {
		Trade currentTrade = (Trade)SerializationUtils.ByteArrayToObject (tradeSerialized);
		TradeManager.Instance.ProceedToTrade (currentTrade);
	}

	[Command]
	public void CmdAddVictoryPoint(int amount) {
		GameManager.Instance.GetCurrentGameState ().CurrentVictoryPoints.AddVictoryPointsForPlayer (myName, amount);
		VictoryPointsCollection victoryPts = GameManager.Instance.GetCurrentGameState ().CurrentVictoryPoints;
		GameManager.Instance.GetCurrentGameState ().RpcClientPostVictoryPointUpdate (SerializationUtils.ObjectToByteArray(victoryPts));
	}

	[Command]
	public void CmdRemoveVictoryPoint(int amount) {
		GameManager.Instance.GetCurrentGameState ().CurrentVictoryPoints.RemoveVictoryPointsForPlayer (myName, amount);
		VictoryPointsCollection victoryPts = GameManager.Instance.GetCurrentGameState ().CurrentVictoryPoints;
		GameManager.Instance.GetCurrentGameState ().RpcClientPostVictoryPointUpdate (SerializationUtils.ObjectToByteArray(victoryPts));
	}

	[Command]
	public void CmdUpdateBarbarianEvent(byte[] barbarianEventSerialized) {
		BarbarianEvent evt = (BarbarianEvent)SerializationUtils.ByteArrayToObject (barbarianEventSerialized);
		GameManager.Instance.GetCurrentGameState ().RpcClientPostBarbarianUpdate (SerializationUtils.ObjectToByteArray (evt));
	}

	[Command]
	public void CmdTriggerBarbarianInvasion() {
		GameEventManager.Instance.TriggerNewBarbarianInvasion ();
	}

	[Command]
	public void CmdUpdateIntersection(string key, byte[] newIntersectionSerialized) {
		GameManager.Instance.GetCurrentGameState ().RpcClientUpdateIntersection (key, newIntersectionSerialized);
	}

	[Command]
	public void CmdAddProgressCard(string name, byte[] progressCardSerialized) {
		AbstractProgressCard card = (AbstractProgressCard)SerializationUtils.ByteArrayToObject (progressCardSerialized);

		ProgressCardCollection cardCollection = GameManager.Instance.GetCurrentGameState ().CurrentProgressCardHands;
		cardCollection.AddCardToPlayerHand (name, card);

		GameManager.Instance.GetCurrentGameState ().RpcClientPublishProgressCardHandUpdate (SerializationUtils.ObjectToByteArray (cardCollection));
		Debug.Log (name + " = " + GameManager.Instance.GetCurrentGameState ().CurrentProgressCardHands.GetCardsForPlayer(name).Count);
	}

	[Command]
	public void CmdRemoveProgressCard(string name, byte[] progressCardSerialized) {
		AbstractProgressCard card = (AbstractProgressCard)SerializationUtils.ByteArrayToObject (progressCardSerialized);
		GameManager.Instance.GetCurrentGameState ().CurrentProgressCardHands.RemoveCardFromPlayerHand (name, card);
		GameManager.Instance.GetCurrentGameState ().RpcClientPublishProgressCardHandUpdate (SerializationUtils.ObjectToByteArray (GameManager.Instance.GetCurrentGameState ().CurrentProgressCardHands));
	}

	[Command]
	public void CmdTriggerGateEvent(string name, byte[] gateEventOutcomeSerialized) {
		RollDiceScript.EventDiceOutcome gateEventOutcome = (RollDiceScript.EventDiceOutcome)SerializationUtils.ByteArrayToObject (gateEventOutcomeSerialized);
		GameEventManager.Instance.TriggerNewGateEvent (gateEventOutcome);
	}

	[Command]
	public void CmdChangeImprovement(string name, byte[] newImprovementSerialized) {
		PlayerImprovement newImprov = (PlayerImprovement)SerializationUtils.ByteArrayToObject (newImprovementSerialized);
		GameManager.Instance.GetCurrentGameState ().CurrentPlayerImprovements.ChangePlayerImprovementForPlayer (name, newImprov);
		GameManager.Instance.GetCurrentGameState ().RpcClientPublishPlayerImprovement (SerializationUtils.ObjectToByteArray (GameManager.Instance.GetCurrentGameState().CurrentPlayerImprovements));
	}

	public void CmdChangeImprovement(PlayerImprovement newImprov) {
		CmdChangeImprovement (myName, SerializationUtils.ObjectToByteArray (newImprov));
	}

	// NOT A COMMAND per say
	public void CmdConsumeResources(Dictionary<StealableType, int> requiredRes) {
		CmdConsumeResources(SerializationUtils.ObjectToByteArray(requiredRes));
	}

	public void AddProgressCard(AbstractProgressCard pCard) {
		StartCoroutine(GameManager.GUI.ShowNewProgressCardNotification (pCard));
		CmdAddProgressCard (myName, SerializationUtils.ObjectToByteArray (pCard));
	}

	void Start () {
		
	}
	
	void Update () {
		 
	}
}
