using System;

public class IrrigationCard : AbstractProgressCard
{
	public IrrigationCard ()
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Irrigation";
	}

	public override string GetDescription ()
	{
		return "You may take 2 grain cards from the bank for each fields hex which is adjacent to at least one of your cities or settlements.";
	}
}

