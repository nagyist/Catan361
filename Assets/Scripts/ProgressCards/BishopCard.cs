using System;

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
		return "Move the robber, following the normal rules. Draw 1 random resource/commodity card from each player who has a settlement or city next to the robber’s new hex.";
	}
}
