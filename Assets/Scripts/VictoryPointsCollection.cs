using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class VictoryPointsCollection
{
	private Dictionary<string, int> victoryPoints = new Dictionary<string, int>();

	public int GetVictoryPointsForPlayer(string playerName) {
		if (!victoryPoints.ContainsKey (playerName)) {
			victoryPoints.Add (playerName, 0);
		}

		return victoryPoints [playerName];
	}

	public void AddVictoryPointsForPlayer(string playerName, int amount) {
		GetVictoryPointsForPlayer (playerName); // shitty but meh

		int curAmount = victoryPoints [playerName];
		victoryPoints [playerName] = curAmount + amount;
	}

	public void RemoveVictoryPointsForPlayer(string playerName, int amount) {
		GetVictoryPointsForPlayer (playerName); // meh

		int curAmount = victoryPoints [playerName];
		victoryPoints [playerName] = curAmount - amount;

	}
}
