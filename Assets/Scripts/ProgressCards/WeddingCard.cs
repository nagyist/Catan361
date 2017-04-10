using System;

public class WeddingCard : AbstractProgressCard
{
	public WeddingCard ()
	{
		CardType = ProgressCardType.Politic;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Wedding";
	}

	public override string GetDescription ()
	{
		return "Each player who has more victory points than you must give you 2 cards of his choice (resource and/or commodity).";
	}
}

