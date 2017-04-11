using System;

[Serializable]
public class IntrigueCard : AbstractProgressCard
{
	public IntrigueCard ()
	{
		CardType = ProgressCardType.Politic;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Intrigue";
	}

	public override string GetDescription ()
	{
		return "You may displace one of your opponent’s knights, without using a knight of your own. The knight must be on an intersection connected to one of your roads or lines of ships.";
	}
}

