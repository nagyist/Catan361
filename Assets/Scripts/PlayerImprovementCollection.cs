using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class PlayerImprovementCollection
{
	public Dictionary<string, PlayerImprovement> CurrentPlayerImprovements = new Dictionary<string, PlayerImprovement>();

	public PlayerImprovementCollection ()
	{
	}

	public PlayerImprovement GetImprovementForPlayer(string name) {
		if (!CurrentPlayerImprovements.ContainsKey (name)) {
			CurrentPlayerImprovements.Add (name, new PlayerImprovement ());
		}

		return CurrentPlayerImprovements [name];
	}

	public void ChangePlayerImprovementForPlayer(string name, PlayerImprovement improvement) {
		GetImprovementForPlayer (name);

		CurrentPlayerImprovements [name] = improvement;
	}
}
