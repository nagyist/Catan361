using System;

[Serializable]
public class MedicineCard : AbstractProgressCard
{
	public MedicineCard (int id) : base(id)
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().medicineProgressCardDiscount = true;
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().StartCoroutine (GameManager.GUI.ShowMessage ("Next settlement upgrade to a city will cost 2 ore and 1 grain."));
		GameManager.GUI.PostStatusMessage ("Medicine card used. Next city upgrade will cost 2 ore and 1 grain.");

		this.RemoveFromPlayerHand ();
	}

	public override string GetTitle ()
	{
		return "Medicine";
	}

	public override string GetDescription ()
	{
		return "For 2 ore and 1 grain, you may upgrade one of your settlements into a city.";
	}
}

