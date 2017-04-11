using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BarbarianInvasion
{
	public enum OutcomeType
	{
		BarbarianAttacked,
		KnightDefended,
		Undecided
	}

	public OutcomeType CurrentOutcome = OutcomeType.Undecided;
	public int BarbarianStrength = 0;
	public int KnightStrength = 0;
	public Dictionary<string, int> KnightStrengthByPlayer = new Dictionary<string, int> ();
	public string PillagedPlayer = "";
	public int PillagedCitiesCount = 0;
	public List<string> PillagedCitiesKey = new List<string> ();

	public List<string> DefendersOfCatan = new List<string>();

	public BarbarianInvasion() {
		this.BarbarianStrength = computeBarbarianStrength ();
		this.KnightStrength = computeKnightStrength ();
	}

	private int computeBarbarianStrength() {
		// Count all the cities + metropoli (on islands and land)
		int totalCityAndMetropoli = 0;
		IntersectionCollection intersections = GameManager.Instance.GetCurrentGameState().CurrentIntersections;
		foreach (Intersection currentIntersection in intersections.Intersections.Values) {
			IntersectionUnit intersectionUnit = currentIntersection.unit;
			if (intersectionUnit != null && intersectionUnit.GetType () == typeof(Village)) {
				Village villageUnit = (Village)intersectionUnit;
				if (villageUnit.myKind == Village.VillageKind.City ||
				   villageUnit.myKind == Village.VillageKind.PoliticsMetropole ||
				   villageUnit.myKind == Village.VillageKind.ScienceMetropole ||
				   villageUnit.myKind == Village.VillageKind.TradeMetropole) {
					totalCityAndMetropoli++;
				}
			}
		}

		return totalCityAndMetropoli;
	}

	private int computeKnightStrength() {
		int totalActiveKnight = 0;
		IntersectionCollection intersections = GameManager.Instance.GetCurrentGameState().CurrentIntersections;
		foreach (Intersection currentIntersection in intersections.Intersections.Values) {
			IntersectionUnit intersectionUnit = currentIntersection.unit;
			if (intersectionUnit != null && intersectionUnit.GetType () == typeof(Knight)) {
				Knight knightUnit = (Knight)intersectionUnit;
				if (!knightUnit.active) {
					continue;
				}

				// update player knight strength
				if (KnightStrengthByPlayer.ContainsKey (currentIntersection.Owner)) {
					int oldStr = KnightStrengthByPlayer [currentIntersection.Owner];
					KnightStrengthByPlayer [currentIntersection.Owner] = oldStr + knightUnit.level;
				} else {
					KnightStrengthByPlayer.Add (currentIntersection.Owner, knightUnit.level);
				}

				// update total
				totalActiveKnight += knightUnit.level;
			}
		}

		foreach(GameObject gameObj in GameManager.ConnectedPlayers) {
			GamePlayer player = gameObj.GetComponent<GamePlayer> ();
			if (!KnightStrengthByPlayer.ContainsKey (player.myName)) {
				KnightStrengthByPlayer.Add (player.myName, 0);
			}
		}

		return totalActiveKnight;
	}

	private List<string> getOrderedPlayerNamesByKnightStrength() {
		List<KeyValuePair<string, int>> orderedPlayerKV = KnightStrengthByPlayer.ToList();
		orderedPlayerKV.Sort ((x, y) => x.Value.CompareTo(y.Value));
		return orderedPlayerKV.Select (x => x.Key).ToList();
	}

	private void pillagePlayer() {
		PillagedCitiesCount = 0;
		PillagedCitiesKey.Clear ();
		IntersectionCollection intersections = GameManager.Instance.GetCurrentGameState().CurrentIntersections;
		foreach (string currentIntersectionKey in intersections.Intersections.Keys) {
			Intersection currentIntersection = intersections.Intersections [currentIntersectionKey];
			if (currentIntersection.Owner != PillagedPlayer) {
				continue;
			}

			IntersectionUnit intersectionUnit = currentIntersection.unit;
			if (intersectionUnit != null && intersectionUnit.GetType () == typeof(Village)) {
				Village villageUnit = (Village)intersectionUnit;
				if (villageUnit.myKind == Village.VillageKind.City) {
					PillagedCitiesKey.Add (currentIntersectionKey);
					PillagedCitiesCount++;
				}
			}
		}

		if (PillagedCitiesCount > 0) {
			foreach (string intersectionKey in PillagedCitiesKey) {
				Intersection pillagedIntersection = GameManager.Instance.GetCurrentGameState ().CurrentIntersections.Intersections [intersectionKey];
				Village villageUnit = (Village)pillagedIntersection.unit;
				villageUnit.myKind = Village.VillageKind.Settlement;
				pillagedIntersection.unit = villageUnit;

				GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdUpdateIntersection (intersectionKey, SerializationUtils.ObjectToByteArray (pillagedIntersection));
			}
		}
	}

	private void defendCatan() {
		int maxKnightStr = KnightStrengthByPlayer.ToList ().Select (x => x.Value).Max ();
		DefendersOfCatan = KnightStrengthByPlayer.ToList ().Where (x => x.Value == maxKnightStr).Select(x => x.Key).ToList ();
		if (DefendersOfCatan.Count == 1) {
			// give victory point
		} else {
			foreach (string defender in DefendersOfCatan) {
				AbstractProgressCard rndCard = GameManager.Instance.GetCurrentGameState ().CurrentProgressCardDeck.DrawRandomCard ();
				GameManager.ConnectedPlayersByName [defender].GetComponent<GamePlayer> ().AddProgressCard (rndCard);
			}
		}
	}

	public void ExecutePrimaryOutcome() {
		BarbarianStrength = computeBarbarianStrength ();
		KnightStrength = computeKnightStrength ();

		if (KnightStrength > BarbarianStrength) {
			CurrentOutcome = OutcomeType.KnightDefended;

			defendCatan ();
		} else {
			CurrentOutcome = OutcomeType.BarbarianAttacked;
			List<string> ordredPlayerKnightStr = getOrderedPlayerNamesByKnightStrength ();
			PillagedPlayer = ordredPlayerKnightStr [0];
			pillagePlayer ();
		}

		GameManager.Instance.GetCurrentGameState ().CurrentBarbarianEvent.Reset ();
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdUpdateBarbarianEvent ();

		// desactivate all knights
		IntersectionCollection intersections = GameManager.Instance.GetCurrentGameState().CurrentIntersections;
		foreach (string intersectionKey in intersections.Intersections.Keys) {
			IntersectionUnit intersectionUnit = intersections.Intersections [intersectionKey].unit;
			if (intersectionUnit != null && intersectionUnit.GetType () == typeof(Knight)) {
				Knight intersectionKnight = (Knight)intersectionUnit;
				if (intersectionKnight.active) {
					intersectionKnight.active = false;
					intersections.Intersections [intersectionKey].unit = intersectionKnight;
					GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdUpdateIntersection (intersectionKey, SerializationUtils.ObjectToByteArray (intersections.Intersections [intersectionKey]));
				}
			}
		}
	}
}

