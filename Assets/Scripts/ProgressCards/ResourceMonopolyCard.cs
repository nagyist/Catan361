using System;

[Serializable]
public class ResourceMonopolyCard : AbstractProgressCard
{
	public ResourceMonopolyCard ()
	{
		CardType = ProgressCardType.Trade;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Resource Monopoly";
	}

	public override string GetDescription ()
	{
		return "Name a resource (brick, grain, ore, lumber, or wool). Each opponent must give you 2 cards of that type (if he has any).";
	}
}

