using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHex : MonoBehaviour {

	public Vector2 HexGridPosition;
	public Vec3 HexGridCubePosition;
	public Texture2D TilesTexture;
	private Sprite[] loadedSprites;
	private int oldIndex = 0;

    // Dictionary containing the resource type and it's specific index
	public Dictionary<StealableType, int[]> resourceSpriteIndex = new Dictionary<StealableType, int[]>()
	{
		{StealableType.Resource_Wool, new int[] { 0 }},
		{StealableType.Resource_Lumber, new int[] { 16, 17, 18, 19 }},
		{StealableType.Resource_Ore, new int[] { 8, 11 }},
		{StealableType.Resource_Brick, new int [] { 12, 13, 14, 15 }},
		{StealableType.Resource_Grain, new int[] { 32, 33, 34}},
		{StealableType.Resource_Fish, new int[] { 4 } },
		{StealableType.Resource_Gold, new int[] { 20, 21 }}
	};

    // Enum for the Adjacent Hex postitions
	public enum AdjHex {
		LEFT, 
		LEFT_TOP, 
		RIGHT_TOP,
		RIGHT,
		RIGHT_BOTTOM,
		LEFT_BOTTOM
	}
    // Enum for the Hex Intersection positions
	public enum HexIntersection {
		LEFT_TOP,
		TOP,
		RIGHT_TOP,
		RIGHT_BOTTOM,
		BOTTOM,
		LEFT_BOTTOM
	}

	private HexTile getHexTile() {
		return GameManager.Instance.GetCurrentGameState ().CurrentBoard [this.HexGridCubePosition];
	}

    // When the mouse hover's over a tile, log the hex position
	void OnMouseEnter () {
		displayPosition ();
		if (GameEventManager.Instance.IsEventMoveRobberPirateEntitySet && GameEventManager.Instance.EventMoveRobberPirateEntityType == "robber" && !getHexTile().IsWater) {
			GetComponent<SpriteRenderer> ().color = new Color32 (255, 0, 0, 255);
		} else if(GameEventManager.Instance.IsEventMoveRobberPirateEntitySet && GameEventManager.Instance.EventMoveRobberPirateEntityType == "pirate" && getHexTile().IsWater) {
			GetComponent<SpriteRenderer> ().color = new Color32 (0, 0, 255, 255);
		}

		if (GameManager.LocalPlayer.GetComponent<GamePlayer> ().inventorProgressCardInUse) {
			GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 0, 255);
		}
	}

	void OnMouseExit () {
		if (GameEventManager.Instance.IsEventMoveRobberPirateEntitySet) {
			GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 255);
		}

		if (GameManager.LocalPlayer.GetComponent<GamePlayer> ().inventorProgressCardInUse) {
			GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 255);
		}
	}

	void OnMouseDown() {
		if (GameEventManager.Instance.IsEventMoveRobberPirateEntitySet) {
			GameEventManager.Instance.HandleMoveRobberPirate (this);
			GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 255);
		}

		if (GameManager.LocalPlayer.GetComponent<GamePlayer> ().inventorProgressCardInUse) {
			GameManager.LocalPlayer.GetComponent<UIInventorProgressCard> ().SelectTile (this.HexGridCubePosition);
		}
	}

    // Display the coordinates of the hex
	void displayPosition() {
		Vector2 oddR = HexGridPosition;
		Vec3 cubeCoords = HexGridCubePosition;
		Debug.Log ("Position = " +
			"(" + oddR.x + ", " + oddR.y + ") - " +
			"(" + cubeCoords.x + ", " + cubeCoords.y + ", " + cubeCoords.z + ")");
	}

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer>().sortingLayerName = "hex";
		this.loadedSprites = Resources.LoadAll<Sprite> (TilesTexture.name);
	}

	void Update () {
		// poke game state
		if(! GameManager.Instance.GameStateReady() ||
		   GameManager.Instance.GetCurrentGameState().CurrentStatus < GameState.GameStatus.GRID_CREATED) {
			return;
		}

		RobberPiratePlacement robberPlacement = GameManager.Instance.GetCurrentGameState ().CurrentRobberPosition;
		RobberPiratePlacement piratePlacement = GameManager.Instance.GetCurrentGameState ().CurrentPiratePosition;

		if (robberPlacement.IsPlaced && robberPlacement.PlacementPos.Equals (this.HexGridCubePosition)) {
			GetComponent<SpriteRenderer> ().color = new Color32 (0, 0, 0, 255);
		} else if (piratePlacement.IsPlaced && piratePlacement.PlacementPos.Equals (this.HexGridCubePosition)) {
			GetComponent<SpriteRenderer> ().color = new Color32 (0, 0, 255, 255);
		} else if (GameManager.LocalPlayer.GetComponent<GamePlayer> ().inventorProgressCardInUse && GameManager.LocalPlayer.GetComponent<UIInventorProgressCard> ().IsTileSelected (this.HexGridCubePosition)) {
			GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 0, 255);
		} else {
			GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 255);
		}

		HexTile refTile = GameManager.Instance.GetCurrentGameState().CurrentBoard[HexGridCubePosition];
		int newIndex = 0;
		if (refTile.IsWater && !refTile.IsFishingGround) {
			GetComponent<SpriteRenderer>().sprite = loadedSprites[5];
			newIndex = 5 + Random.Range(0, 3);
		} else {
			int[] indexes = resourceSpriteIndex [refTile.Resource];
			newIndex = indexes[Random.Range(0, indexes.Length - 1)];
		}

		if (System.Math.Abs (newIndex - this.oldIndex) > 3) {
			this.oldIndex = newIndex;
		} else {
			newIndex = this.oldIndex;
		}

		GetComponent<SpriteRenderer>().sprite = loadedSprites[newIndex];
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
