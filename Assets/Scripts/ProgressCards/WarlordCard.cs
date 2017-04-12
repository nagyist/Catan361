using System;

[Serializable]
public class WarlordCard : AbstractProgressCard
{
	public WarlordCard (int id) : base(id)
	{
		CardType = ProgressCardType.Politic;
	}

	public override void ExecuteCardEffect() {
		IntersectionCollection intersections = GameManager.Instance.GetCurrentGameState ().CurrentIntersections;
		foreach (Intersection i in intersections.Intersections.Values) {
			if (i.Owner == GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName) {
				if (i.unit != null && i.unit.GetType () == typeof(Knight)) {
					Knight k = (Knight)i.unit;
					if (!k.active) {
						GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdActivateKnight (SerializationUtils.ObjectToByteArray (new Vec3[] { i.adjTile1, i.adjTile2, i.adjTile3 }));
					}
				}
			}
		}

		GameManager.LocalPlayer.GetComponent<GamePlayer>().StartCoroutine(GameManager.GUI.ShowMessage("Warlord card used. All knight were activated."));
		this.RemoveFromPlayerHand ();
	}

	public override string GetTitle ()
	{
		return "Warlord";
	}

	public override string GetDescription ()
	{
		return "You may activate all of your knights for free.";
	}
}
