using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Edge {

	public bool IsOwned = false;
	public string Owner;

	public Vec3 adjTile1 { get; }
	public Vec3 adjTile2 { get; }

	public Edge() {}

	public Edge(Vec3 tile1, Vec3 tile2) {
		adjTile1 = tile1;
		adjTile2 = tile2;
	}
}
