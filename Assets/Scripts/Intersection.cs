using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Intersection {

	public Vec3 adjTile1;
	public Vec3 adjTile2;
	public Vec3 adjTile3;

    public bool isOccupied()
    {
        if (SettlementLevel > 0)
            return true;
        if (KnightLevel > 0)
            return true;
        return false;
    }

	public String SettlementOwner = "";
	public int SettlementLevel = 0;
    public int KnightLevel = 0;

	public Intersection(Vec3 i1, Vec3 i2, Vec3 i3) {
		adjTile1 = i1;
		adjTile2 = i2;
		adjTile3 = i3;
	}
}
