using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour {
	public Vector2 hexGridPosition;
	public Vector3 hexGridCubePosition;
	public int selectedNum;

	private SpriteRenderer renderer;
	private Color oldColor;

	public enum AdjHex {
		LEFT, 
		LEFT_TOP, 
		RIGHT_TOP,
		RIGHT,
		RIGHT_BOTTOM,
		LEFT_BOTTOM
	}

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

	public Vector3 getAdjacentHexPos(AdjHex adjHex) {
		switch (adjHex) {
		case AdjHex.LEFT:
			return new Vector3 (hexGridCubePosition.x - 1, hexGridCubePosition.y + 1, hexGridCubePosition.z);
		case AdjHex.LEFT_TOP:
			return new Vector3 (hexGridCubePosition.x, hexGridCubePosition.y + 1, hexGridCubePosition.z - 1);
		case AdjHex.RIGHT_TOP:
			return new Vector3 (hexGridCubePosition.x + 1, hexGridCubePosition.y, hexGridCubePosition.z - 1);
		case AdjHex.RIGHT:
			return new Vector3 (hexGridCubePosition.x + 1, hexGridCubePosition.y - 1, hexGridCubePosition.z);
		case AdjHex.RIGHT_BOTTOM:
			return new Vector3 (hexGridCubePosition.x, hexGridCubePosition.y - 1, hexGridCubePosition.z + 1);
		case AdjHex.LEFT_BOTTOM:
			return new Vector3 (hexGridCubePosition.x - 1, hexGridCubePosition.y, hexGridCubePosition.z + 1);
		default:
			throw new UnityException ("Inexistant adjacent edge");	
		}
	}

	public List<Vector3> getAdjacentHexesPos() {
		List<Vector3> adjHexes = new List<Vector3> ();
		adjHexes.Add (this.getAdjacentHexPos (AdjHex.LEFT));
		adjHexes.Add (this.getAdjacentHexPos (AdjHex.LEFT_TOP));
		adjHexes.Add (this.getAdjacentHexPos (AdjHex.RIGHT_TOP));
		adjHexes.Add (this.getAdjacentHexPos (AdjHex.RIGHT));
		adjHexes.Add (this.getAdjacentHexPos (AdjHex.RIGHT_BOTTOM));
		adjHexes.Add (this.getAdjacentHexPos (AdjHex.LEFT_BOTTOM));
		return adjHexes;
	}
}
