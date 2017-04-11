using System;

[Serializable]
public class MerchantFleetCard : AbstractProgressCard
{
	public MerchantFleetCard ()
	{
		CardType = ProgressCardType.Trade;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Merchant Fleet";
	}

	public override string GetDescription ()
	{
		return "For the rest of your turn, you may trade one resource or commodity of your choice with the bank at a 2:1 rate. You may make as many trades as you wish.";
	}
}

