using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class MiningCard : AbstractProgressCard
{
	public MiningCard (int id) : base(id)
	{
		CardType = ProgressCardType.Science;
	}

	public override void ExecuteCardEffect() {
		int totalIntersectionAdjToMountains = 0;
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

					if (adjHex1.Resource == StealableType.Resource_Ore || adjHex2.Resource == StealableType.Resource_Ore || adjHex3.Resource == StealableType.Resource_Ore) {
						totalIntersectionAdjToMountains++;
					}
				}
			}
		}
		Dictionary<StealableType, int> newRes = new Dictionary<StealableType, int> () {
			{StealableType.Resource_Ore, totalIntersectionAdjToMountains * 2}
		};

		GameManager.Instance.GetCurrentGameState ().RpcClientShowMessage ("Mining card used. You got " + (totalIntersectionAdjToMountains * 2) + " ore resources.", 2.0f);

		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdAddResourcesResources (SerializationUtils.ObjectToByteArray(newRes));
		GameManager.Instance.GetCurrentGameState ().RpcClientPostStatusMessage (localPlayer.myName + " used mining card, got " + (totalIntersectionAdjToMountains * 2) + " ore resources.");

		this.RemoveFromPlayerHand ();
	}

	public override string GetTitle ()
	{
		return "Mining";
	}

	public override string GetDescription ()
	{
		return "You may take 2 ore cards from the bank for each mountains hex adjacent to at least one of your cities or settlements.";
	}
}