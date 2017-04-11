using System;

[Serializable]
public class DiplomatCard : AbstractProgressCard
{
	public DiplomatCard ()
	{
		CardType = ProgressCardType.Politic;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Diplomat";
	}

	public override string GetDescription ()
	{
		return "You may remove any open road (a road with nothing attached at one end). If you remove one of your own roads, you may place it in another location.";
	}
}

