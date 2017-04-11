using System;

[Serializable]
public class InventorCard : AbstractProgressCard
{
	public InventorCard (int id) : base(id)
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().inventorProgressCardInUse = true;
		UIInventorProgressCard ui = GameManager.LocalPlayer.AddComponent<UIInventorProgressCard> ();

		this.RemoveFromPlayerHand ();
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

