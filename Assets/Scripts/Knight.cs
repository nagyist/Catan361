using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Knight : IntersectionUnit {

	public bool active;
	public int level;
	public bool hasBeenPromotedThisTurn;

	public Knight() {
		this.active = false;
		this.level = 1;
		this.hasBeenPromotedThisTurn = false;
	}


}
