using System;

public class RoadBuildingCard : AbstractProgressCard
{
	public RoadBuildingCard ()
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Road Building";
	}

	public override string GetDescription ()
	{
		return "When you play this card, you may place 2 roads for free (if playing with Catan: Seafarers, you may place 2 ships or 1 ship and 1 road instead).";
	}
}

