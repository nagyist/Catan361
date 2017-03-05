using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour {
	public Vector3 adjTile1 { get; }
	public Vector3 adjTile2 { get; }

	public Edge(Vector3 tile1, Vector3 tile2) {
		adjTile1 = tile1;
		adjTile2 = tile2;
	}
}
