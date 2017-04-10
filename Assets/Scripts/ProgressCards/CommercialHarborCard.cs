using System;

public class CommercialHarborCard : AbstractProgressCard
{
	public CommercialHarborCard ()
	{
		CardType = ProgressCardType.Trade;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Commercial Harbor";
	}

	public override string GetDescription ()
	{
		return "You may offer each opponent a resource card from your hand. In exchange, each player must give you a commodity card of his choice. If he has none, your resource card is returned.";
	}
}
