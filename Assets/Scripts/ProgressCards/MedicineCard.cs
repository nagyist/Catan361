using System;

public class MedicineCard : AbstractProgressCard
{
	public MedicineCard ()
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Medicine";
	}

	public override string GetDescription ()
	{
		return "For 2 ore and 1 grain, you may upgrade one of your settlements into a city.";
	}
}

