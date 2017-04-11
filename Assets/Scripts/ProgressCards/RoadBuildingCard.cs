using System;

[Serializable]
public class RoadBuildingCard : AbstractProgressCard
{
	public RoadBuildingCard (int id) : base(id)
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {
		GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer> ();

		localPlayer.roadBuildingProgressCardDiscount = true;
		localPlayer.roadBuildingProgressCardUsed = 0;

		localPlayer.StartCoroutine (GameManager.GUI.ShowMessage(localPlayer.myName + " used road building card."));
		GameManager.Instance.GetCurrentGameState ().RpcClientPostStatusMessage (localPlayer.myName + " used road building card. He gets to build 2 road/ship construction for free.");
	
		this.RemoveFromPlayerHand ();
	}

	public override string GetTitle ()
	{
		return "Road Building";
	}

	public override string GetDescription ()
	{
		return "When you play this card, you may place 2 roads for free (if playing with Catan: Seafarers, you may place 2 ships or 1 ship and 1 road instead).";
	}
}

