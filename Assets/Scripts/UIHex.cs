using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHex : MonoBehaviour {
	public Vector2 HexGridPosition;
	public Vec3 HexGridCubePosition;

	public Dictionary<StealableType, Color> resourceColor = new Dictionary<StealableType, Color>()
	{
		{StealableType.Resource_Wool, new Color32 (255, 160, 122, 255)},
		{StealableType.Resource_Lumber, new Color32 (128, 128, 0, 255)},
		{StealableType.Resource_Ore, new Color32 (112, 128, 144, 255)},
		{StealableType.Resource_Brick, new Color32 (178, 34, 34, 255)},
		{StealableType.Resource_Grain, new Color32 (255, 215, 0, 255)}
	};


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
		displayPosition ();
	}

	void OnMouseExit () {
	}

	void OnMouseDown() {
		//GameManager.Instance.gui.ShowHexActionWindow (this);
	}

	void displayPosition() {
		Vector2 oddR = HexGridPosition;
		Vec3 cubeCoords = HexGridCubePosition;
		Debug.Log ("Position = " +
			"(" + oddR.x + ", " + oddR.y + ") - " +
			"(" + cubeCoords.x + ", " + cubeCoords.y + ", " + cubeCoords.z + ")");
	}

	// Use this for initialization
	void Start () {
	}

	void Update () {
		// poke game state
		if(! GameManager.Instance.GameStateReady() ||
		   GameManager.Instance.GetCurrentGameState().CurrentStatus < GameState.GameStatus.GRID_CREATED) {
			return;
		}

		HexTile refTile = GameManager.Instance.GetCurrentGameState().CurrentBoard[HexGridCubePosition];
		if (refTile.IsWater) {
			GetComponent<SpriteRenderer>().color = Color.blue;
		} else {
			GetComponent<SpriteRenderer>().color = resourceColor[refTile.Resource];
		}

	}

	public Vec3 getAdjacentHexPos(AdjHex adjHex) {
		return getAdjacentHexPos (this.HexGridCubePosition, adjHex);
	}

	public static Vec3 getAdjacentHexPos(Vec3 hexPos, AdjHex adjHex) {
		switch (adjHex) {
		case AdjHex.LEFT:
			return new Vec3 (hexPos.x - 1, hexPos.y + 1, hexPos.z);
		case AdjHex.LEFT_TOP:
			return new Vec3 (hexPos.x, hexPos.y + 1, hexPos.z - 1);
		case AdjHex.RIGHT_TOP:
			return new Vec3 (hexPos.x + 1, hexPos.y, hexPos.z - 1);
		case AdjHex.RIGHT:
			return new Vec3 (hexPos.x + 1, hexPos.y - 1, hexPos.z);
		case AdjHex.RIGHT_BOTTOM:
			return new Vec3 (hexPos.x, hexPos.y - 1, hexPos.z + 1);
		case AdjHex.LEFT_BOTTOM:
			return new Vec3 (hexPos.x - 1, hexPos.y, hexPos.z + 1);
		default:
			throw new UnityException ("Inexistant adjacent edge");	
		}
	}

	public List<Vec3> getIntersectionAdjacentHexPos(HexIntersection intersection) {
		return getIntersectionAdjacentHexPos (this.HexGridCubePosition, intersection);	
	}
		
	public static List<Vec3> getIntersectionAdjacentHexPos(Vec3 hexPos, HexIntersection intersection) {
		List<Vec3> adjHexes = new List<Vec3> ();
		switch (intersection) {
		case HexIntersection.LEFT_TOP:
			adjHexes.AddRange (new Vec3[] {
				hexPos,
				getAdjacentHexPos (hexPos, AdjHex.LEFT),
				getAdjacentHexPos (hexPos, AdjHex.LEFT_TOP)
			});
			break;
		case HexIntersection.TOP:
			adjHexes.AddRange (new Vec3[] {
				hexPos,
				getAdjacentHexPos (hexPos, AdjHex.LEFT_TOP),
				getAdjacentHexPos (hexPos, AdjHex.RIGHT_TOP)
			});
			break;
		case HexIntersection.RIGHT_TOP:
			adjHexes.AddRange (new Vec3[] {
				hexPos,
				getAdjacentHexPos (hexPos, AdjHex.RIGHT_TOP),
				getAdjacentHexPos (hexPos, AdjHex.RIGHT)
			});
			break;
		case HexIntersection.RIGHT_BOTTOM:
			adjHexes.AddRange (new Vec3[] {
				hexPos,
				getAdjacentHexPos (hexPos, AdjHex.RIGHT),
				getAdjacentHexPos (hexPos, AdjHex.RIGHT_BOTTOM)
			});
			break;
		case HexIntersection.BOTTOM:
			adjHexes.AddRange (new Vec3[] {
				hexPos,
				getAdjacentHexPos (hexPos, AdjHex.RIGHT_BOTTOM),
				getAdjacentHexPos (hexPos, AdjHex.LEFT_BOTTOM)
			});
			break;
		case HexIntersection.LEFT_BOTTOM:
			adjHexes.AddRange (new Vec3[] {
				hexPos,
				getAdjacentHexPos (hexPos, AdjHex.LEFT_BOTTOM),
				getAdjacentHexPos (hexPos, AdjHex.LEFT)
			});
			break;
		default:
			throw new UnityException ("Inexistant intersection");	
		}

		return adjHexes;
	}

	public List<Vec3> getAdjacentHexesPos() {
		return getAdjacentHexesPos (this.HexGridCubePosition);
	}

	public static List<Vec3> getAdjacentHexesPos(Vec3 hexPos) {
		List<Vec3> adjHexes = new List<Vec3> ();
		adjHexes.Add (getAdjacentHexPos (hexPos, AdjHex.LEFT));
		adjHexes.Add (getAdjacentHexPos (hexPos, AdjHex.LEFT_TOP));
		adjHexes.Add (getAdjacentHexPos (hexPos, AdjHex.RIGHT_TOP));
		adjHexes.Add (getAdjacentHexPos (hexPos, AdjHex.RIGHT));
		adjHexes.Add (getAdjacentHexPos (hexPos, AdjHex.RIGHT_BOTTOM));
		adjHexes.Add (getAdjacentHexPos (hexPos, AdjHex.LEFT_BOTTOM));
		return adjHexes;
	}

	public  List<List<Vec3>> getIntersectionsAdjacentPos() {
		return getIntersectionsAdjacentPos (this.HexGridCubePosition);
	}

	public static List<List<Vec3>> getIntersectionsAdjacentPos(Vec3 hexPos) {
		List<List<Vec3>> intersectionPosList = new List<List<Vec3>> ();
		intersectionPosList.Add (getIntersectionAdjacentHexPos (hexPos, HexIntersection.LEFT_TOP));
		intersectionPosList.Add (getIntersectionAdjacentHexPos (hexPos, HexIntersection.TOP));
		intersectionPosList.Add (getIntersectionAdjacentHexPos (hexPos, HexIntersection.RIGHT_TOP));
		intersectionPosList.Add (getIntersectionAdjacentHexPos (hexPos, HexIntersection.RIGHT_BOTTOM));
		intersectionPosList.Add (getIntersectionAdjacentHexPos (hexPos, HexIntersection.BOTTOM));
		intersectionPosList.Add (getIntersectionAdjacentHexPos (hexPos, HexIntersection.LEFT_BOTTOM));
		return intersectionPosList;
	}
}
