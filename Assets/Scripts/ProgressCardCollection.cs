using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class ProgressCardCollection
{
	private Dictionary<string, List<AbstractProgressCard>> PlayerHands = new Dictionary<string, List<AbstractProgressCard>>();

	public ProgressCardCollection ()
	{
	}

	public List<AbstractProgressCard> GetCardsForPlayer(string name) {
		if (!PlayerHands.ContainsKey (name)) {
			PlayerHands.Add (name, new List<AbstractProgressCard> ());
		}

		return PlayerHands [name];
	}

	public void AddCardToPlayerHand(string name, AbstractProgressCard card) {
		List<AbstractProgressCard> cardHand = GetCardsForPlayer (name);
		cardHand.Add (card);

		PlayerHands [name] = cardHand;
	}

	public void RemoveCardFromPlayerHand(string name, AbstractProgressCard card) {
		List<AbstractProgressCard> cardHand = GetCardsForPlayer (name);
		cardHand.Remove (card);

		PlayerHands [name] = cardHand;
	}
}
