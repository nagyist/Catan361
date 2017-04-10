using System;

public class InventorCard : AbstractProgressCard
{
	public InventorCard ()
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Inventor";
	}

	public override string GetDescription ()
	{
		return "You may swap 2 number tokens of your choice on the game board. You may not choose any 2, 12, 6, or 8 tokens.";
	}
}

