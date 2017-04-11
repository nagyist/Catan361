using System;

[Serializable]
public class SaboteurCard : AbstractProgressCard
{
	public SaboteurCard ()
	{
		CardType = ProgressCardType.Politic;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Saboteur";
	}

	public override string GetDescription ()
	{
		return "Each player who has as many or more victory points than you must discard half his cards to the bank (resource and/or commodity).";
	}
}

