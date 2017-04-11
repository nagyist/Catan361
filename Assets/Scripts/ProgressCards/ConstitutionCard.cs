using System;

[Serializable]
public class ConstitutionCard : AbstractProgressCard
{
	public ConstitutionCard (int id) : base(id)
	{
		CardType = ProgressCardType.Politic;
	}

	public override void ExecuteCardEffect() {
		GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer> ();
		localPlayer.CmdAddVictoryPoint (1);

		localPlayer.StartCoroutine (GameManager.GUI.ShowMessage("Constitution card used. You got 1 victory point!"));
		GameManager.Instance.GetCurrentGameState().RpcClientPostStatusMessage (localPlayer.myName + " used constitution card. He got 1 victory point!");

		this.RemoveFromPlayerHand ();
	}

	public override string GetTitle ()
	{
		return "Constitution";
	}

	public override string GetDescription ()
	{
		return "1 Victory Point ! Reveal this card immediately when you draw it. This card cannot be stolen by a spy.";
	}
}
