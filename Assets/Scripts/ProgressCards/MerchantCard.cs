using System;

public class MerchantCard : AbstractProgressCard
{
	public MerchantCard ()
	{
		CardType = ProgressCardType.Trade;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Merchant";
	}

	public override string GetDescription ()
	{
		return "Place the merchant on a land hex next to your settlement or city. While the merchant remains here, you may trade the resource produced by this terrain at a 2:1 ratio.";
	}
}
