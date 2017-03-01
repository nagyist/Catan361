using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	bool active {get; set;};
	bool hasMovedThisTurn {get; set;};
	int level {get; set;};
	bool hasBeenPromotedThisTurn {get; set;};

	public Knight() {
		this.active = false;
		this.hasMovedThisTurn = false;
		this.level = 1;
		this.hasBeenPromotedThisTurn = false;
	}

	
}
