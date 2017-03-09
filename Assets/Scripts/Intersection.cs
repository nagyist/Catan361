using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Intersection {

	public Vec3 adjTile1;
	public Vec3 adjTile2;
	public Vec3 adjTile3;

	public String SettlementOwner = "";
	public int SettlementCount = 0;
	public Dictionary<int, int> SettlementLevels = new Dictionary<int, int>();

	public Intersection(Vec3 i1, Vec3 i2, Vec3 i3) {
		adjTile1 = i1;
		adjTile2 = i2;
		adjTile3 = i3;
	}
}
