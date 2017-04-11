using System;

[Serializable]
public class TradeMonopolyCard : AbstractProgressCard
{
	public TradeMonopolyCard ()
	{
		CardType = ProgressCardType.Trade;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Trade Monopoly";
	}

	public override string GetDescription ()
	{
		return "Name a commodity (cloth, coin, or paper). Each opponent must give you 1 card of that type (if he has any).";
	}
}
