using System;

[Serializable]
public class MiningCard : AbstractProgressCard
{
	public MiningCard ()
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Mining";
	}

	public override string GetDescription ()
	{
		return "You may take 2 ore cards from the bank for each mountains hex adjacent to at least one of your cities or settlements.";
	}
}