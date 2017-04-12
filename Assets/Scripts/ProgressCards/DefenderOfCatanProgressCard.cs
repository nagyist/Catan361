using System;

[Serializable]
public class DefenderOfCatanProgressCard : AbstractProgressCard
{
	public DefenderOfCatanProgressCard (int id) : base(id)
	{
		CardType = ProgressCardType.Barbarian;
	}

	public override void ExecuteCardEffect() {
		return;
	}

	public override bool IsConsumable() {
		return false;
	}

	public override bool IsStealable() {
		return false;
	}

	public override string GetTitle ()
	{
		return "Defender of Catan";
	}

	public override string GetDescription ()
	{
		return "You get one victory point for saving the land of Catan of a Barbarian Invasion!";
	}
}
