using System;

[Serializable]
public class DeserterCard : AbstractProgressCard
{
	public DeserterCard ()
	{
		CardType = ProgressCardType.Politic;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Deserter";
	}

	public override string GetDescription ()
	{
		return "Choose an opponent. He must remove 1 of his knights (his choice) from the board. You may place 1 of your own knights on the board (its strength must be equal to the knight removed)";
	}
}