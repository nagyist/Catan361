using System;

public class AlchemistCard : AbstractProgressCard
{
	public AlchemistCard ()
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {
		
	}

	public override string GetTitle ()
	{
		return "Alchemist";
	}

	public override string GetDescription ()
	{
		return "Play this card before you roll the dice. You decide what the results of both numbered dice will be. Then roll the event die normally. Resolve the event die first.";
	}
}
