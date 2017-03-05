using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour {
	public Vector2 hexGridPosition;
	public Vector3 hexGridCubePosition;

	public int selectedNum;

	private SpriteRenderer renderer;
	private Color oldColor;

	void OnMouseEnter () {
		oldColor = renderer.color;
		renderer.color = new Color (oldColor.r, oldColor.g, oldColor.b, oldColor.a - 0.5f);
		displayPosition ();
	}

	void OnMouseExit () {
		renderer.color = oldColor;
	}

	void displayPosition() {
		Hex hexTile = GetComponent<Hex> ();

		Vector2 oddR = hexTile.hexGridPosition;
		Vector3 cubeCoords = hexTile.hexGridCubePosition;

		Debug.Log ("Position = " +
			"(" + oddR.x + ", " + oddR.y + ") - " +
			"(" + cubeCoords.x + ", " + cubeCoords.y + ", " + cubeCoords.z + ")");
	}

	// Use this for initialization
	void Start () {
		renderer = GetComponent<SpriteRenderer> ();
		oldColor = renderer.color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
