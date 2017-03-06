using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class IntersectionCollection
{
	private Dictionary<string, Intersection> Intersections { get; }

	public IntersectionCollection ()
	{
		Intersections = new Dictionary<String, Intersection> ();
	}

	public void addIntersection(Vector3 adjTile1, Vector3 adjTile2, Vector3 adjTile3) {
		string key = computeKey (adjTile1, adjTile2, adjTile3);
		if (!Intersections.ContainsKey (key)) {
			Intersection newIntersection = new Intersection ();
			newIntersection.adjTile1 = adjTile1;
			newIntersection.adjTile2 = adjTile2;
			newIntersection.adjTile3 = adjTile3;

			Intersections.Add (key, newIntersection);
		}
	}

	public Intersection getIntersection(List<Vector3> hexes) {
		return getIntersection (hexes [0], hexes [1], hexes [2]);
	}

	public Intersection getIntersection(Vector3 hex1, Vector3 hex2, Vector3 hex3) {
		string key = computeKey (hex1, hex2, hex3);
		return Intersections [key];
	}

	// TODO : refactor for better code
	private string computeKey(Vector3 hex1, Vector3 hex2, Vector3 hex3) {
		// apply simple heuristic : "flatten" xyz coords of both coords and take the lowest one
		byte[] hashFirst = posToByte(hex1);
		byte[] hashSecond = posToByte (hex2);
		byte[] hashThird = posToByte (hex3);

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

	private byte[] posToByte(Vector3 pos) {
		byte[] xBytes = BitConverter.GetBytes ((int)pos.x);
		byte[] yBytes = BitConverter.GetBytes ((int)pos.y);
		byte[] zBytes = BitConverter.GetBytes ((int)pos.z);

		byte[] result = new byte[64];

		for (int i = 0; i < xBytes.Length; i++) {
			result [i] = xBytes [i];
		}

		for (int j = 0; j < yBytes.Length; j++) {
			result [j + 3 - 1] = yBytes [j];
		}

		for (int k = 0; k < zBytes.Length; k++) {
			result [k + 6 - 1] = zBytes [k];
		}

		return result;
	}
}

