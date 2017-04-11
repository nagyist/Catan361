using System;

[Serializable]
public class EngineerCard : AbstractProgressCard
{
	public EngineerCard (int id) : base(id)
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().engineerProgressCardDiscount = true;
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().StartCoroutine(GameManager.GUI.ShowMessage ("Engineer card used. Next city wall will be free."));
		GameManager.GUI.PostStatusMessage ("Next constructed city wall will be free.");

		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdRemoveProgressCard (
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName,
			SerializationUtils.ObjectToByteArray (this)
		);

		this.RemoveFromPlayerHand ();
	}

	public override string GetTitle ()
	{
		return "Engineer";
	}

	public override string GetDescription ()
	{
		return "You may build 1 city wall for free.";
	}
}

