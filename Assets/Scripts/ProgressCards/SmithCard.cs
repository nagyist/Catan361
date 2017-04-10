using System;

public class SmithCard : AbstractProgressCard
{
	public SmithCard ()
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Smith";
	}

	public override string GetDescription ()
	{
		return "You may promote up to 2 of your knights for free (the normal rules for promoting knights still apply). Mighty knights may not be promoted.";
	}
}

