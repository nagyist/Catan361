using System;

[Serializable]
public class WarlordCard : AbstractProgressCard
{
	public WarlordCard ()
	{
		CardType = ProgressCardType.Politic;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Warlord";
	}

	public override string GetDescription ()
	{
		return "You may activate all of your knights for free.";
	}
}
