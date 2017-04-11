using System;

[Serializable]
public class PrinterCard : AbstractProgressCard
{
	public PrinterCard (int id) : base(id)
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {
		GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer> ();
		localPlayer.CmdAddVictoryPoint (1);

		localPlayer.StartCoroutine (GameManager.GUI.ShowMessage("Printer card used. You got 1 victory point!"));
		GameManager.Instance.GetCurrentGameState().RpcClientPostStatusMessage (localPlayer.myName + " used printer card. He got 1 victory point!");

		this.RemoveFromPlayerHand ();
	}

	public override string GetTitle ()
	{
		return "Printer";
	}

	public override string GetDescription ()
	{
		return "1 Victory Point! Reveal this card immediately when you draw it. This card cannot be stolen by a spy.";
	}
}
