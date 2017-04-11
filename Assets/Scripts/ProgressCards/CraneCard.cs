using System;

[Serializable]
public class CraneCard : AbstractProgressCard
{
	public CraneCard (int id) : base(id)
	{		
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().craneProgressCardDiscount = true;
		GameManager.GUI.ShowMessage ("Crane Progress Card used. Next city improvement will be discounted.");
		GameManager.GUI.PostStatusMessage ("Next city improvement will be discounted by 1 commodity.");

		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdRemoveProgressCard (
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName,
			SerializationUtils.ObjectToByteArray (this)
		);	
	}

	public override string GetTitle ()
	{
		return "Crane";
	}

	public override string GetDescription ()
	{
		return "One city improvement (abbey, town hall, etc.) that you build this turn costs one less commodity than usual.";
	}
}

