using System;

[Serializable]
public class SmithCard : AbstractProgressCard
{
	public SmithCard (int id) : base(id)
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {
		GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer> ();

		localPlayer.smithProgressCardDiscount = true;
		localPlayer.smithProgressCardUsed = 0;

		localPlayer.StartCoroutine (GameManager.GUI.ShowMessage(localPlayer.myName + " used road smith card."));
		GameManager.Instance.GetCurrentGameState ().RpcClientPostStatusMessage (localPlayer.myName + " used smith card. He gets to promite 2 knights for free.");

		this.RemoveFromPlayerHand ();
	}

	public override string GetTitle ()
	{
		return "Smith";
	}

	public override string GetDescription ()
	{
		return "You may promote up to 2 of your knights for free (the normal rules for promoting knights still apply). Mighty knights may not be promoted.";
	}
}

