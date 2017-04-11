using System;

[Serializable]
public class SpyCard : AbstractProgressCard
{
	public SpyCard ()
	{
		CardType = ProgressCardType.Politic;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Spy";
	}

	public override string GetDescription ()
	{
		return "Examine an opponent’s hand of progress cards. You may take 1 card of your choice and add it to your hand.";
	}
}

