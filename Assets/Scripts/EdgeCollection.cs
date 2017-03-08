using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

[Serializable]
public class EdgeCollection {
	public Dictionary<string, Edge> Edges { get; private set; }

	public EdgeCollection() {
		Edges = new Dictionary<string, Edge> ();
	}

	public void addEdge(Vec3 hexPos1, Vec3 hexPos2) {
		string key = computeKey (hexPos1, hexPos2);
		if (!Edges.ContainsKey (key)) {
			Edge edgeToAdd = new Edge (hexPos1, hexPos2);

			Edges.Add (key, edgeToAdd);
		}
	}

	public void setEdge(string key, Edge edge) {
		if (!Edges.ContainsKey (key)) {
			Edges.Add (key, edge);
		} else {
			Edges [key] = edge;
		}
	}

	public Edge getEdge(Vec3 hexPos1, Vec3 hexPos2) {
		string key = computeKey (hexPos1, hexPos2);
		return Edges [key];
	}

	private string computeKey(Vec3 hex1, Vec3 hex2) {
		// apply simple heuristic : "flatten" xyz coords of both coords and take the lowest one
		byte[] hashFirst = posToByte(hex1);
		byte[] hashSecond = posToByte (hex2);

		string flattenFirst = hex1.x + "" + hex1.y + "" + hex1.z;
		string flattenSecond = hex2.x + "" + hex2.y + "" + hex2.z;

		UInt64 firstVal = BitConverter.ToUInt64 (hashFirst, 0);
		UInt64 secondVal = BitConverter.ToUInt64 (hashSecond, 0);

		string key = (firstVal < secondVal ? flattenFirst + flattenSecond : flattenSecond + flattenFirst);
		return key;
	}

	private byte[] posToByte(Vec3 pos) {
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