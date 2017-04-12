using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Knight : IntersectionUnit {

	public bool active;
	public int level;
	public bool hasBeenPromotedThisTurn;

	// boolean to check if it can do any knight actions like moving positions or chasing away robber/pirate
	// knights can't be activated and do an action on the same turn
    public bool exhausted;

    public Knight() {
		this.active = false;
		this.level = 1;
		this.hasBeenPromotedThisTurn = false;
        this.exhausted = false;
    }


}
