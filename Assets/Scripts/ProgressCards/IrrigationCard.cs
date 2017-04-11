using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class IrrigationCard : AbstractProgressCard
{
	public IrrigationCard (int id) : base(id)
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {
		int totalIntersectionAdjToFields = 0;
		GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer> ();
		List<HexTile> examinedPos = new List<HexTile> ();

		foreach (Intersection currentIntersection in GameManager.Instance.GetCurrentGameState().CurrentIntersections.Intersections.Values) {
			IntersectionUnit currentUnit = currentIntersection.unit;
			if (currentIntersection.Owner == localPlayer.myName && currentUnit != null && currentUnit.GetType() == typeof(Village)) {
				Village currentVillage = (Village)currentUnit;
				if (currentVillage.myKind == Village.VillageKind.Settlement) {
					HexTile adjHex1 = GameManager.Instance.GetCurrentGameState().CurrentBoard[currentIntersection.adjTile1];
					HexTile adjHex2 = GameManager.Instance.GetCurrentGameState().CurrentBoard[currentIntersection.adjTile2];
					HexTile adjHex3 = GameManager.Instance.GetCurrentGameState().CurrentBoard[currentIntersection.adjTile3];

					if (examinedPos.Contains (adjHex1) || examinedPos.Contains (adjHex2) || examinedPos.Contains (adjHex3)) {
						continue;
					}

					if (adjHex1.Resource == StealableType.Resource_Grain || adjHex2.Resource == StealableType.Resource_Grain || adjHex3.Resource == StealableType.Resource_Grain) {
						totalIntersectionAdjToFields++;
					}
				}
			}
		}
		Dictionary<StealableType, int> newRes = new Dictionary<StealableType, int> () {
			{StealableType.Resource_Grain, totalIntersectionAdjToFields * 2}
		};

		GameManager.Instance.GetCurrentGameState ().RpcClientShowMessage ("Irrigation card used. You got " + (totalIntersectionAdjToFields * 2) + " grain resources.", 2.0f);

		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdAddResourcesResources (SerializationUtils.ObjectToByteArray(newRes));
		GameManager.Instance.GetCurrentGameState ().RpcClientPostStatusMessage (localPlayer.myName + " used irrigation card, got " + (totalIntersectionAdjToFields * 2) + " grain resources.");

		this.RemoveFromPlayerHand ();
	}

	public override string GetTitle ()
	{
		return "Irrigation";
	}

	public override string GetDescription ()
	{
		return "You may take 2 grain cards from the bank for each fields hex which is adjacent to at least one of your cities or settlements.";
	}
}

