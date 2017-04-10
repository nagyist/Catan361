using System;

public class EngineerCard : AbstractProgressCard
{
	public EngineerCard ()
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Engineer";
	}

	public override string GetDescription ()
	{
		return "You may build one city wall for free.";
	}
}

