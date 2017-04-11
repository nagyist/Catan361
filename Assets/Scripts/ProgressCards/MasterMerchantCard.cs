using System;

[Serializable]
public class MasterMerchantCard : AbstractProgressCard
{
	public MasterMerchantCard ()
	{
		CardType = ProgressCardType.Trade;
	}

	public override void ExecuteCardEffect() {

	}

	public override string GetTitle ()
	{
		return "Master Merchant";
	}

	public override string GetDescription ()
	{
		return "Select an opponent who has more victory points than you. You may examine his hand of resource cards and select any 2 cards, which you may add to your hand.";
	}
}
