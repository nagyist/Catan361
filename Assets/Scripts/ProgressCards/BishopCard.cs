using System;

[Serializable]
public class BishopCard : AbstractProgressCard
{
	public BishopCard ()
	{
		CardType = ProgressCardType.Politic;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Bishop";
	}

	public override string GetDescription ()
	{
		return "Move the robber. You may draw 1 random card (resource or commodity) from the hand of each player who has a settlement or city adjacent to the robber’s new hex.";
	}
}
