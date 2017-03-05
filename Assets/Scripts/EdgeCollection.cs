using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeCollection {
	public Dictionary<string, Edge> Edges { get; }

	public EdgeCollection() {
		Edges = new Dictionary<string, Edge> ();
	}

	public void addEdge(Edge edgeToAdd) {
		// apply simple heuristic : "flatten" xyz coords of both coords and take the lowest one
		byte[] hashFirst = posToByte(edgeToAdd.adjTile1);
		byte[] hashSecond = posToByte (edgeToAdd.adjTile2);

		string flattenFirst = edgeToAdd.adjTile1.x + "" + edgeToAdd.adjTile1.y + "" + edgeToAdd.adjTile1.z;
		string flattenSecond = edgeToAdd.adjTile2.x + "" + edgeToAdd.adjTile2.y + "" + edgeToAdd.adjTile2.z;

		UInt64 firstVal = BitConverter.ToUInt64 (hashFirst, 0);
		UInt64 secondVal = BitConverter.ToUInt64 (hashSecond, 0);

		string key = (firstVal < secondVal ? flattenFirst + flattenSecond : flattenSecond + flattenFirst);
		if (!Edges.ContainsKey (key)) {
			Edges.Add (key, edgeToAdd);
		} else {
			Debug.Log ("test");
		}
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