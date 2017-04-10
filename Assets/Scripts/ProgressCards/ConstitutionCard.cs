using System;

public class ConstitutionCard : AbstractProgressCard
{
	public ConstitutionCard ()
	{
		CardType = ProgressCardType.Politic;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Constitution";
	}

	public override string GetDescription ()
	{
		return "1 Victory Point ! Reveal this card immediately when you draw it. This card cannot be stolen by a spy.";
	}
}
