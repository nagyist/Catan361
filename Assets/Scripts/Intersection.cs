using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Intersection {

	public Vec3 adjTile1;
	public Vec3 adjTile2;
	public Vec3 adjTile3;

    public IntersectionUnit unit;
	public String Owner = "";

	//public bool canAccessHarbour { get; set; }

	public Intersection(Vec3 i1, Vec3 i2, Vec3 i3) {
		adjTile1 = i1;
		adjTile2 = i2;
		adjTile3 = i3;
	}

	public bool IsSurroundedByWater() {
		bool allWater = true;
		Dictionary<Vec3, HexTile> board = GameManager.Instance.GetCurrentGameState ().CurrentBoard;

		if (board.ContainsKey(this.adjTile1)) {
			allWater = allWater && GameManager.Instance.GetCurrentGameState ().CurrentBoard [adjTile1].IsWater;
		}

		if (board.ContainsKey(this.adjTile2)) {
			allWater = allWater && GameManager.Instance.GetCurrentGameState ().CurrentBoard [adjTile2].IsWater;
		}

			if (board.ContainsKey(this.adjTile3)) {
			allWater = allWater && GameManager.Instance.GetCurrentGameState ().CurrentBoard [adjTile3].IsWater;
		}

		return allWater;
	}

	public override bool Equals(System.Object obj) {
		if (obj == null || GetType () != obj.GetType ())
			return false;

		Intersection i = (Intersection) obj;
		return computeKey (this.adjTile1, this.adjTile2, this.adjTile3) == computeKey (i.adjTile1, i.adjTile2, i.adjTile3);
	}

	public override int GetHashCode() {
		byte[] hashFirst = PositionUtilities.PosToByte(this.adjTile1);
		byte[] hashSecond = PositionUtilities.PosToByte(this.adjTile2);
		byte[] hashThird = PositionUtilities.PosToByte(this.adjTile3);

		Int32 firstHashVal = BitConverter.ToInt32 (hashFirst, 0);
		Int32 secondHashVal = BitConverter.ToInt32 (hashSecond, 0);
		Int32 thirdHashVal = BitConverter.ToInt32 (hashThird, 0);

		return firstHashVal + secondHashVal + thirdHashVal;
	}

	private string computeKey(Vec3 hex1, Vec3 hex2, Vec3 hex3) {
		// apply simple heuristic : "flatten" xyz coords of both coords and take the lowest one
		byte[] hashFirst = PositionUtilities.PosToByte(hex1);
		byte[] hashSecond = PositionUtilities.PosToByte(hex2);
		byte[] hashThird = PositionUtilities.PosToByte(hex3);

		string flattenFirst = hex1.x + "" + hex1.y + "" + hex1.z;
		string flattenSecond = hex2.x + "" + hex2.y + "" + hex2.z;
		string flattenThird = hex3.x + "" + hex3.y + "" + hex3.z;

		UInt64 firstHashVal = BitConverter.ToUInt64 (hashFirst, 0);
		UInt64 secondHashVal = BitConverter.ToUInt64 (hashSecond, 0);
		UInt64 thirdHashVal = BitConverter.ToUInt64 (hashThird, 0);

		Dictionary<UInt64, string> hashToFlatten = new Dictionary<UInt64, string> ();
		hashToFlatten.Add (firstHashVal, flattenFirst);
		hashToFlatten.Add (secondHashVal, flattenSecond);
		hashToFlatten.Add (thirdHashVal, flattenThird);

		UInt64[] sortArr = new ulong[] { firstHashVal, secondHashVal, thirdHashVal };
		Array.Sort (sortArr);

		string key = "";
		foreach (UInt64 k in sortArr) {
			key += hashToFlatten [k];
		}

		return key;
	}
}
