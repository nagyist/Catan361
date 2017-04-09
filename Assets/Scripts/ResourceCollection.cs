using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public class ResourceCollection
{
	private Dictionary<string, PlayerResourcesCollection> PlayerResources = new Dictionary<string, PlayerResourcesCollection> ();

	[Serializable]
	public class PlayerResourcesCollection : Dictionary<StealableType, int> {
        public PlayerResourcesCollection()
        {

        }
        public PlayerResourcesCollection(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }

	public void InitPlayerResources(string playerName) {
		if (PlayerResources.ContainsKey (playerName)) {
			return;
		}

		PlayerResourcesCollection resCollection = new PlayerResourcesCollection ();
		resCollection.Add (StealableType.Resource_Brick, 100);
		resCollection.Add (StealableType.Resource_Grain, 100);
		resCollection.Add (StealableType.Resource_Lumber, 100);
		resCollection.Add (StealableType.Resource_Ore, 100);
		resCollection.Add (StealableType.Resource_Wool, 100);
		resCollection.Add (StealableType.Resource_Fish, 100);

		PlayerResources.Add (playerName, resCollection);
	}

	public PlayerResourcesCollection GetPlayerResources(string playerName) {
		if (!PlayerResources.ContainsKey (playerName)) {
			InitPlayerResources (playerName);
		}

		return PlayerResources [playerName];
	}

	public void UpdateResource(string playerName, StealableType type, int newAmount) {
		PlayerResourcesCollection resCollection = GetPlayerResources (playerName);
		if (!resCollection.ContainsKey (type)) {
			resCollection.Add (type, newAmount);
		} else {
			resCollection [type] = newAmount;
		}
	}

	public bool PlayerHasEnoughResources(string playerName, Dictionary<StealableType, int> requiredRes) {
		PlayerResourcesCollection playerResources = GetPlayerResources (playerName);
		// iterate through required resources
		foreach(StealableType key in requiredRes.Keys) {
			// return false if player lacks any of the resource types 
			if(!playerResources.ContainsKey(key)) {
				return false;
			}
			// return false if the player doesn't have enough of the current resource
			int playerAmount = playerResources[key];
			if(playerAmount < requiredRes[key]) {
				return false;
			}
		}

		return true;
	}

	// consumes player resources defiend by requiredRes
	public bool PlayerConsumeResources(string playerName, Dictionary<StealableType, int> requiredRes) {
		PlayerResourcesCollection playerResources = GetPlayerResources (playerName);
		// returns false if the player does the required resouces
		if(!PlayerHasEnoughResources(playerName, requiredRes)) { return false; }

		// iterate though the required resources and decrement the player's amounts
		foreach(StealableType key in requiredRes.Keys) {
			int playerAmount = playerResources[key];
			int newAmount = playerAmount - requiredRes[key];
			UpdateResource (playerName, key, newAmount);
		}

		return true;
	}

	public bool PlayerAddResources(string playerName, Dictionary<StealableType, int> addingAmounts) {
		PlayerResourcesCollection playerResources = GetPlayerResources (playerName);
		foreach(StealableType key in addingAmounts.Keys) {
			int playerAmount = playerResources [key];
			int newAmount = playerAmount + addingAmounts [key];
			UpdateResource (playerName, key, newAmount);
		}

		return true;
	}
}
