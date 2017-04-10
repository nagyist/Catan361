using System;

public class CraneCard : AbstractProgressCard
{
	public CraneCard ()
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Crane";
	}

	public override string GetDescription ()
	{
		return "One city improvement (abbey, town hall, etc.) that you build this turn costs one less commodity than usual.";
	}
}

