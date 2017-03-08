using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour {
	public Vector2 hexGridPosition;
	public Vector3 hexGridCubePosition;
	public int selectedNum;
	public int resourceNumber;
	public StealableType hexResource;

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

	public enum HexIntersection {
		LEFT_TOP,
		TOP,
		RIGHT_TOP,
		RIGHT_BOTTOM,
		BOTTOM,
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

	void OnMouseDown() {
		GameManager.Instance.gui.ShowHexActionWindow (this);
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
		
	public List<Vector3> getIntersectionAdjacentHexPos(HexIntersection intersection) {
		List<Vector3> adjHexes = new List<Vector3> ();
		switch (intersection) {
		case HexIntersection.LEFT_TOP:
			adjHexes.AddRange (new Vector3[] {
				this.hexGridCubePosition,
				this.getAdjacentHexPos (AdjHex.LEFT),
				this.getAdjacentHexPos (AdjHex.LEFT_TOP)
			});
			break;
		case HexIntersection.TOP:
			adjHexes.AddRange (new Vector3[] {
				this.hexGridCubePosition,
				this.getAdjacentHexPos (AdjHex.LEFT_TOP),
				this.getAdjacentHexPos (AdjHex.RIGHT_TOP)
			});
			break;
		case HexIntersection.RIGHT_TOP:
			adjHexes.AddRange (new Vector3[] {
				this.hexGridCubePosition,
				this.getAdjacentHexPos (AdjHex.RIGHT_TOP),
				this.getAdjacentHexPos (AdjHex.RIGHT)
			});
			break;
		case HexIntersection.RIGHT_BOTTOM:
			adjHexes.AddRange (new Vector3[] {
				this.hexGridCubePosition,
				this.getAdjacentHexPos (AdjHex.RIGHT),
				this.getAdjacentHexPos (AdjHex.RIGHT_BOTTOM)
			});
			break;
		case HexIntersection.BOTTOM:
			adjHexes.AddRange (new Vector3[] {
				this.hexGridCubePosition,
				this.getAdjacentHexPos (AdjHex.RIGHT_BOTTOM),
				this.getAdjacentHexPos (AdjHex.LEFT_BOTTOM)
			});
			break;
		case HexIntersection.LEFT_BOTTOM:
			adjHexes.AddRange (new Vector3[] {
				this.hexGridCubePosition,
				this.getAdjacentHexPos (AdjHex.LEFT_BOTTOM),
				this.getAdjacentHexPos (AdjHex.LEFT)
			});
			break;
		default:
			throw new UnityException ("Inexistant intersection");	
		}

		return adjHexes;
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

	public List<List<Vector3>> getIntersectionsAdjacentPos() {
		List<List<Vector3>> intersectionPosList = new List<List<Vector3>> ();
		intersectionPosList.Add (this.getIntersectionAdjacentHexPos (HexIntersection.LEFT_TOP));
		intersectionPosList.Add (this.getIntersectionAdjacentHexPos (HexIntersection.TOP));
		intersectionPosList.Add (this.getIntersectionAdjacentHexPos (HexIntersection.RIGHT_TOP));
		intersectionPosList.Add (this.getIntersectionAdjacentHexPos (HexIntersection.RIGHT_BOTTOM));
		intersectionPosList.Add (this.getIntersectionAdjacentHexPos (HexIntersection.BOTTOM));
		intersectionPosList.Add (this.getIntersectionAdjacentHexPos (HexIntersection.LEFT_BOTTOM));
		return intersectionPosList;
	}
}
