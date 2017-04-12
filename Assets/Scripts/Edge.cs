using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Edge {

	public bool IsOwned = false;
	public bool isHarbour { get; set; }
	public string Owner = "";

	public Vec3 adjTile1 { get; }
	public Vec3 adjTile2 { get; }

	public Edge() {}

	public Edge(Vec3 tile1, Vec3 tile2) {
		adjTile1 = tile1;
		adjTile2 = tile2;
	}

	public override bool Equals(System.Object obj) {
		if (obj == null || GetType () != obj.GetType ())
			return false;

		Edge e = obj as Edge;
		return computeKey (this.adjTile1, this.adjTile2) == computeKey (e.adjTile1, e.adjTile2);
	}
	
	private string computeKey(Vec3 hex1, Vec3 hex2) {
		// apply simple heuristic : "flatten" xyz coords of both coords and take the lowest one
		byte[] hashFirst = PositionUtilities.PosToByte(hex1);
		byte[] hashSecond = PositionUtilities.PosToByte(hex2);

		string flattenFirst = hex1.x + "" + hex1.y + "" + hex1.z;
		string flattenSecond = hex2.x + "" + hex2.y + "" + hex2.z;

		UInt64 firstVal = BitConverter.ToUInt64 (hashFirst, 0);
		UInt64 secondVal = BitConverter.ToUInt64 (hashSecond, 0);

		string key = (firstVal < secondVal ? flattenFirst + flattenSecond : flattenSecond + flattenFirst);
		return key;
	}

	public override int GetHashCode() {
		byte[] hashFirst = PositionUtilities.PosToByte(adjTile1);
		byte[] hashSecond = PositionUtilities.PosToByte(adjTile2);

		Int32 firstVal = BitConverter.ToInt32 (hashFirst, 0);
		Int32 secondVal = BitConverter.ToInt32 (hashSecond, 0);

		return firstVal + secondVal;
	}

	public bool IsShip() {
		HexTile adjHex1 = GameManager.Instance.GetCurrentGameState ().CurrentBoard [this.adjTile1];
		HexTile adjHex2 = GameManager.Instance.GetCurrentGameState ().CurrentBoard [this.adjTile2];

		return adjHex1.IsWater && adjHex2.IsWater;
	}
}
