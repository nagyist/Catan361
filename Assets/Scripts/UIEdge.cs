using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEdge : MonoBehaviour {

	public Vec3 HexPos1 { get; set; }
	public Vec3 HexPos2 { get; set; }

	void OnMouseEnter() {
		GetComponent<SpriteRenderer> ().color = Color.red;

	}

	void OnMouseExit() {
		GetComponent<SpriteRenderer> ().color = Color.black;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
