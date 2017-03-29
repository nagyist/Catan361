using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour {

	public GameObject Hex;
	public GameObject Harbour;

	private GameObject hexGridObject;

	public int gridWidth = 10;
	public int gridHeight = 9;

	private Dictionary<Vec3, GameObject> cubeHexes;
	public Dictionary<string, GameObject> harbourCollection;
	public Dictionary<GameObject, StealableType> harbours;

	private float hexWidth;
	private float hexHeight;

	public Dictionary<int, StealableType> resourceType = new Dictionary<int, StealableType>()
	{
		{1, StealableType.Resource_Wool},
		{2, StealableType.Resource_Lumber},
		{3, StealableType.Resource_Ore},
		{4, StealableType.Resource_Brick},
		{5, StealableType.Resource_Grain},
		{6, StealableType.Resource_Wool},
		{7, StealableType.Resource_Lumber},
		{8, StealableType.Resource_Ore},
		{9, StealableType.Resource_Brick},
		{10, StealableType.Resource_Grain},
		{11, StealableType.Resource_Wool},
		{12, StealableType.Resource_Lumber},
		{13, StealableType.Resource_Ore},
		{14, StealableType.Resource_Brick},
		{15, StealableType.Resource_Grain},
		{16, StealableType.Resource_Wool},
		{17, StealableType.Resource_Lumber},
		{18, StealableType.Resource_Ore},
		{19, StealableType.Resource_Brick},
		{20, StealableType.Resource_Grain},
		{21, StealableType.Resource_Gold},
		{22, StealableType.Resource_Lumber},
		{23, StealableType.Resource_Ore},
		{24, StealableType.Resource_Brick},
		{25, StealableType.Resource_Grain},
		{26, StealableType.Resource_Gold}
	};

	private void setHexSizes () 
	{
		hexWidth = Hex.GetComponent<RectTransform> ().rect.size.x;
		hexHeight = Hex.GetComponent<RectTransform> ().rect.size.y;
	}

	private Vector3 calcInitialPos ()
	{
		Vector3 initialPos;
		initialPos = new Vector3 (-hexWidth * gridWidth / 2f + hexWidth / 2, gridHeight / 2f * hexHeight / 2, 0);
		return initialPos;
	}

	private void createHex(Vector3 pos)
	{
		GameObject thisHex = (GameObject) Instantiate (Hex);
		thisHex.transform.position = pos;
	}

	private Vector3 calcUnityCoord(Vector2 gridPos)
	{
		//Position of the first tile
		Vector3 initPos = calcInitialPos();
		float x = initPos.x + gridPos.x * hexWidth;
		float y = initPos.y - gridPos.y * hexHeight;
		return new Vector3 (x, y, 0);
	}

	private Vec3 offsetOddRToCubeCoordinate(Vector2 oddR) {
		int x = (int) oddR.x - ((int)oddR.y - ((int)oddR.y & 1)) / 2;
		int z = (int) oddR.y;
		int y = -x - z;

		return new Vec3 (x, y, z);
	}

	public void CreateHexGrid(GameState createdGameState) {
		createdGameState.CurrentBoard = new Dictionary<Vec3, HexTile> ();
		createdGameState.CurrentEdges = new EdgeCollection ();
		createdGameState.CurrentIntersections = new IntersectionCollection ();

		for (int y = 0; y < gridHeight; y++) {
			for (int x = 0; x < gridWidth; x++) {
				HexTile refTile = new HexTile ();
				refTile.Resource = resourceType [Random.Range (1, 21)];

				if (x == 3 && y > 2 && y < 6) 
				{
					if (x % 2 == 1) 
					{
						refTile.SelectedNum = Random.Range (1, 7);
					} 
					else 
					{
						refTile.SelectedNum = Random.Range (8, 13);
					}
				} 
				else if (x == 4 && y > 1 && y < 7) 
				{
					if (y % 2 == 1) 
					{
						refTile.SelectedNum = Random.Range (1, 7);
					} 
					else 
					{
						refTile.SelectedNum = Random.Range (8, 13);
					}
				} 
				else if (x == 5 && y > 1 && y < 7 && y != 4) 
				{
					if (y % 2 == 1) 
					{
						refTile.SelectedNum = Random.Range (1, 7);
					} 
					else 
					{
						refTile.SelectedNum = Random.Range (8, 13);
					}
				} 
				else if (x == 5 && y == 4) 
				{
					refTile.Resource = StealableType.Resource_Fish;
					refTile.SelectedNum = Random.Range(1,4);
					refTile.SelectedNum2 = Random.Range(4,7);
					refTile.SelectedNum3 = Random.Range(7,10);
					refTile.SelectedNum4 = Random.Range(10,13);

				} 
				else if (x == 6 && y > 1 && y < 7) 
				{
					if (y % 2 == 1) {
						refTile.SelectedNum = Random.Range (1, 7);
					} else {
						refTile.SelectedNum = Random.Range (8, 13);
					}

				} 
				else if (x == 6 && y > 3 && y < 7) 
				{
					if (y % 2 == 1) {
						refTile.SelectedNum = Random.Range (1, 7);
					} else {
						refTile.SelectedNum = Random.Range (8, 13);
					}

				} 
				else if (x == 7 && y > 3 && y < 5) 
				{
					if (y % 2 == 1) 
					{
						refTile.SelectedNum = Random.Range (1, 7);
					} 
					else 
					{
						refTile.SelectedNum = Random.Range (8, 13);
					}
				} 

				// Islands
				else if (x == 8 && y > 4 && y < 7) 
				{
					if (y % 2 == 1) 
					{
						refTile.SelectedNum = Random.Range (1, 7);
						refTile.Resource = resourceType [Random.Range (1, 21)];
					} 
					else 
					{
						refTile.SelectedNum = Random.Range (8, 13);
						refTile.Resource = resourceType [26];
					}
				} 
				else if (x == 7 && y == 7) 
				{
					if (y % 2 == 1) 
					{
						refTile.SelectedNum = Random.Range (1, 7);
						refTile.Resource = resourceType [Random.Range (1, 21)];
					} 
					else 
					{
						refTile.SelectedNum = Random.Range (8, 13);
						refTile.Resource = resourceType [Random.Range (1, 21)];
					}
				} 
				else if (x == 9 && y == 6) 
				{
					if (y % 2 == 1) 
					{
						refTile.SelectedNum = Random.Range (1, 7);
						refTile.Resource = resourceType [26];
					} 
					else 
					{
						refTile.SelectedNum = Random.Range (8, 13);
						refTile.Resource = resourceType [26];
					}
				} 
				else if (x == 9 && y == 4) 
				{
					if (y % 2 == 1) 
					{
						refTile.SelectedNum = Random.Range (1, 7);
						refTile.Resource = resourceType [Random.Range (1, 21)];
					} 
					else 
					{
						refTile.SelectedNum = Random.Range (8, 13);
						refTile.Resource = resourceType [Random.Range (1, 21)];
					}
				} 
				else if (x == 8 && y == 2) 
				{
					if (y % 2 == 1) 
					{
						refTile.SelectedNum = Random.Range (1, 7);
						refTile.Resource = resourceType [Random.Range (1, 21)];
					} 
					else 
					{
						refTile.SelectedNum = Random.Range (8, 13);
						refTile.Resource = resourceType [Random.Range (1, 21)];
					}
				} 
				else if (x == 7 && y == 1) 
				{
					if (y % 2 == 1) 
					{
						refTile.SelectedNum = Random.Range (1, 7);
						refTile.Resource = resourceType [Random.Range (1, 21)];
					} 
					else 
					{
						refTile.SelectedNum = Random.Range (8, 13);
						refTile.Resource = resourceType [Random.Range (1, 21)];
					}
				} 
				else if (x < 7 && x > 4 && y == 8) 
				{
					if (y % 2 == 1) 
					{
						refTile.SelectedNum = Random.Range (1, 7);
						refTile.Resource = resourceType [Random.Range (1, 21)];
					} 
					else 
					{
						refTile.SelectedNum = Random.Range (8, 13);
						refTile.Resource = resourceType [Random.Range (1, 21)];
					}
				} 
				else 
				{
					refTile.SelectedNum = 0;
					refTile.IsWater = true;
				}
			
				Vec3 cubePos = offsetOddRToCubeCoordinate (new Vector2(x, y));

				// add tile to game state
				createdGameState.CurrentBoard.Add (cubePos, refTile);

				// add edges to game state
				foreach (Vec3 adjHex in UIHex.getAdjacentHexesPos(cubePos)) {
					createdGameState.CurrentEdges.addEdge (cubePos, adjHex);
				}

				// add intersections to game state
				foreach(List<Vec3> adjHexInIntersection in UIHex.getIntersectionsAdjacentPos(cubePos)) {
					createdGameState.CurrentIntersections.addIntersection (adjHexInIntersection [0], adjHexInIntersection [1], adjHexInIntersection [2]);
				}
			}
		}
	}

	private Dictionary<Edge, GameObject> instanciatedUiEdges = new Dictionary<Edge, GameObject>();
	private Dictionary<Intersection, GameObject> instanciatedUiIntersections = new Dictionary<Intersection, GameObject>();
	private static int order = 0;
	public void CreateUIHexGrid()
	{
		cubeHexes = new Dictionary<Vec3, GameObject> ();

		if (hexGridObject != null) {
			Destroy (this.hexGridObject);
		}

		hexGridObject = new GameObject ("HexGrid");
		hexGridObject.transform.parent = this.transform;

		for (int y = 0; y < gridHeight; y++) 
		{
			for (int x = 0; x < gridWidth; x++) 
			{
				GameObject thisHex = (GameObject)Instantiate (Hex);

				// setup ui
				Vector2 gridPos = new Vector2(x, y);
				thisHex.transform.position = calcWorldCoord(gridPos);
				thisHex.transform.parent = hexGridObject.transform;

				UIHex uiHex = thisHex.GetComponent<UIHex> ();
				thisHex.GetComponent<SpriteRenderer> ().sortingOrder = order++;
				uiHex.HexGridPosition = gridPos;
				uiHex.HexGridCubePosition = offsetOddRToCubeCoordinate (gridPos);
				cubeHexes.Add (uiHex.HexGridCubePosition, thisHex);
			}
		}

		// attach edges and intersection to UI
		foreach(Vec3 currentCubePos in cubeHexes.Keys) {
			GameObject currentHexGameObj = cubeHexes [currentCubePos];
			UIHex currentHex = currentHexGameObj.GetComponent<UIHex> ();

			// edges
			// get each edge instance 
			Edge rightEdge = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(currentCubePos, currentHex.getAdjacentHexPos(global::UIHex.AdjHex.RIGHT));
			Edge rightTopEdge = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge (currentCubePos, currentHex.getAdjacentHexPos (global::UIHex.AdjHex.RIGHT_TOP));
			Edge leftTopEdge = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge (currentCubePos, currentHex.getAdjacentHexPos (global::UIHex.AdjHex.LEFT_TOP));
			Edge leftEdge = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge (currentCubePos, currentHex.getAdjacentHexPos (global::UIHex.AdjHex.LEFT));
			Edge leftBottomEdge = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge (currentCubePos, currentHex.getAdjacentHexPos (global::UIHex.AdjHex.LEFT_BOTTOM));
			Edge rightBottomEdge = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge (currentCubePos, currentHex.getAdjacentHexPos (global::UIHex.AdjHex.RIGHT_BOTTOM));

			// add each edge instance as a component to corresponding edge game object
			GameObject rightEdgeGameObj = currentHexGameObj.transform.FindChild("REdge").gameObject;
			UIEdge rightEdgeUi = rightEdgeGameObj.AddComponent<UIEdge> ();
			rightEdgeUi.HexPos1 = rightEdge.adjTile1;
			rightEdgeUi.HexPos2 = rightEdge.adjTile2;
			if (!instanciatedUiEdges.ContainsKey (rightEdge)) {
				rightEdgeGameObj.SetActive (true);
				instanciatedUiEdges.Add (rightEdge, rightEdgeGameObj);
			}

			GameObject rightTopEdgeGameObj = currentHexGameObj.transform.FindChild("RTEdge").gameObject;
			UIEdge rightTopEdgeUi = rightTopEdgeGameObj.AddComponent<UIEdge> ();
			rightTopEdgeUi.HexPos1 = rightTopEdge.adjTile1;
			rightTopEdgeUi.HexPos2 = rightTopEdge.adjTile2;
			if (!instanciatedUiEdges.ContainsKey (rightTopEdge)) {
				rightTopEdgeGameObj.SetActive (true);
				instanciatedUiEdges.Add (rightTopEdge, rightTopEdgeGameObj);
			}

			GameObject leftTopEdgeGameObj = currentHexGameObj.transform.FindChild("LTEdge").gameObject;
			UIEdge leftTopEdgeUi = leftTopEdgeGameObj.AddComponent<UIEdge> ();
			leftTopEdgeUi.HexPos1 = leftTopEdge.adjTile1;
			leftTopEdgeUi.HexPos2 = leftTopEdge.adjTile2;
			if (!instanciatedUiEdges.ContainsKey (leftTopEdge)) {
				leftTopEdgeGameObj.SetActive (true);
				instanciatedUiEdges.Add (leftTopEdge, leftTopEdgeGameObj);
			}

			GameObject leftEdgeGameObj = currentHexGameObj.transform.FindChild("LEdge").gameObject;
			UIEdge leftEdgeUi = leftEdgeGameObj.AddComponent<UIEdge> ();
			leftEdgeUi.HexPos1 = leftEdge.adjTile1;
			leftEdgeUi.HexPos2 = leftEdge.adjTile2;
			if (!instanciatedUiEdges.ContainsKey (leftEdge)) {
				leftEdgeGameObj.SetActive (true);
				instanciatedUiEdges.Add (leftEdge, leftEdgeGameObj);
			}

			GameObject leftBottomEdgeGameObj = currentHexGameObj.transform.FindChild("LBEdge").gameObject;
			UIEdge leftBottomEdgeUi = leftBottomEdgeGameObj.AddComponent<UIEdge> ();
			leftBottomEdgeUi.HexPos1 = leftBottomEdge.adjTile1;
			leftBottomEdgeUi.HexPos2 = leftBottomEdge.adjTile2;
			if (!instanciatedUiEdges.ContainsKey (leftBottomEdge)) {
				leftBottomEdgeGameObj.SetActive (true);
				instanciatedUiEdges.Add (leftBottomEdge, leftBottomEdgeGameObj);
			}

			GameObject rightBottomEdgeGameObj = currentHexGameObj.transform.FindChild("RBEdge").gameObject;
			UIEdge rightBottomEdgeUi = rightBottomEdgeGameObj.AddComponent<UIEdge> ();
			rightBottomEdgeUi.HexPos1 = rightBottomEdge.adjTile1;
			rightBottomEdgeUi.HexPos2 = rightBottomEdge.adjTile2;
			if (!instanciatedUiEdges.ContainsKey (rightBottomEdge)) {
				rightBottomEdgeGameObj.SetActive (true);
				instanciatedUiEdges.Add (rightBottomEdge, rightBottomEdgeGameObj);
			}

			// intersections
			// get each intersection instance
			Intersection leftTopIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(currentHex.getIntersectionAdjacentHexPos(global::UIHex.HexIntersection.LEFT_TOP));
			Intersection topIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(currentHex.getIntersectionAdjacentHexPos(global::UIHex.HexIntersection.TOP));
			Intersection rightTopIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(currentHex.getIntersectionAdjacentHexPos(global::UIHex.HexIntersection.RIGHT_TOP));
			Intersection rightBottomIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(currentHex.getIntersectionAdjacentHexPos(global::UIHex.HexIntersection.RIGHT_BOTTOM));
			Intersection bottomIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(currentHex.getIntersectionAdjacentHexPos(global::UIHex.HexIntersection.BOTTOM));
			Intersection leftBottomIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(currentHex.getIntersectionAdjacentHexPos(global::UIHex.HexIntersection.LEFT_BOTTOM));

			// add each intersection instance as a component to corresponding intersection game object
			GameObject leftTopGameObj = currentHexGameObj.transform.FindChild("LTIntersection").gameObject;
			UIIntersection leftTopIntersectionUi = leftTopGameObj.AddComponent<UIIntersection> ();
			leftTopIntersectionUi.HexPos1 = leftTopIntersection.adjTile1;
			leftTopIntersectionUi.HexPos2 = leftTopIntersection.adjTile2;
			leftTopIntersectionUi.HexPos3 = leftTopIntersection.adjTile3;
			if (!instanciatedUiIntersections.ContainsKey (leftTopIntersection)) {
				leftTopGameObj.SetActive (true);
				instanciatedUiIntersections.Add (leftTopIntersection, leftTopGameObj);
			}

			GameObject topGameObj = currentHexGameObj.transform.FindChild("TIntersection").gameObject;
			UIIntersection topIntersectionUi = topGameObj.AddComponent<UIIntersection> ();
			topIntersectionUi.HexPos1 = topIntersection.adjTile1;
			topIntersectionUi.HexPos2 = topIntersection.adjTile2;
			topIntersectionUi.HexPos3 = topIntersection.adjTile3;
			if (!instanciatedUiIntersections.ContainsKey (topIntersection)) {
				topGameObj.SetActive (true);
				instanciatedUiIntersections.Add (topIntersection, topGameObj);
			}

			GameObject rightTopGameObj = currentHexGameObj.transform.FindChild("RTIntersection").gameObject;
			UIIntersection rightTopIntersectionUi = rightTopGameObj.AddComponent<UIIntersection> ();
			rightTopIntersectionUi.HexPos1 = rightTopIntersection.adjTile1;
			rightTopIntersectionUi.HexPos2 = rightTopIntersection.adjTile2;
			rightTopIntersectionUi.HexPos3 = rightTopIntersection.adjTile3;
			if (!instanciatedUiIntersections.ContainsKey (rightTopIntersection)) {
				rightTopGameObj.SetActive (true);
				instanciatedUiIntersections.Add (rightTopIntersection, rightTopGameObj);
			}

			GameObject rightBottomGameObj = currentHexGameObj.transform.FindChild("RBIntersection").gameObject;
			UIIntersection rightBottomIntersectionUi = rightBottomGameObj.AddComponent<UIIntersection> ();
			rightBottomIntersectionUi.HexPos1 = rightBottomIntersection.adjTile1;
			rightBottomIntersectionUi.HexPos2 = rightBottomIntersection.adjTile2;
			rightBottomIntersectionUi.HexPos3 = rightBottomIntersection.adjTile3;
			if (!instanciatedUiIntersections.ContainsKey (rightBottomIntersection)) {
				rightBottomGameObj.SetActive (true);
				instanciatedUiIntersections.Add (rightBottomIntersection, rightBottomGameObj);
			}

			GameObject bottomGameObj = currentHexGameObj.transform.FindChild("BIntersection").gameObject;
			UIIntersection bottomIntersectionUi = bottomGameObj.AddComponent<UIIntersection> ();
			bottomIntersectionUi.HexPos1 = bottomIntersection.adjTile1;
			bottomIntersectionUi.HexPos2 = bottomIntersection.adjTile2;
			bottomIntersectionUi.HexPos3 = bottomIntersection.adjTile3;
			if (!instanciatedUiIntersections.ContainsKey (bottomIntersection)) {
				bottomGameObj.SetActive (true);
				instanciatedUiIntersections.Add (bottomIntersection, bottomGameObj);
			}

			GameObject leftBottomGameObj = currentHexGameObj.transform.FindChild("LBIntersection").gameObject;
			UIIntersection leftBottomIntersectionUi = leftBottomGameObj.AddComponent<UIIntersection> ();
			leftBottomIntersectionUi.HexPos1 = leftBottomIntersection.adjTile1;
			leftBottomIntersectionUi.HexPos2 = leftBottomIntersection.adjTile2;
			leftBottomIntersectionUi.HexPos3 = leftBottomIntersection.adjTile3;
			if (!instanciatedUiIntersections.ContainsKey (leftBottomIntersection)) {
				leftBottomGameObj.SetActive (true);
				instanciatedUiIntersections.Add (leftBottomIntersection, leftBottomGameObj);
			}
		}
	}

	public Vector3 calcWorldCoord(Vector2 gridPos)
	{
		//Position of the first tile
		Vector3 initPos = calcInitialPos();
		float xoffset = 0;
		float yoffset = 0;
		// If the row number is a multiple of 2
		if (gridPos.y % 2 != 0)
			// The x offset is equal to the radius of the inner circle, or half the width for pointy hexes
			xoffset = hexWidth / 2;

		float x = initPos.x + xoffset + gridPos.x * hexWidth;
		// Every new line is offset in y direction by 3/4 of the outer circle diameter, or hexagon height
		yoffset = 0.85f;
		float y = initPos.y - gridPos.y * hexHeight * yoffset;
		return new Vector3(x, y, 0);
	}

	public void buildHarbours () {
		
		//harbourCollection = new Dictionary<string, GameObject>();

		harbours = new Dictionary<GameObject, StealableType> ();

		//harbour 1
		Vec3 harbour1CubePos = offsetOddRToCubeCoordinate (new Vector2(4,2));
		GameObject currentHexGameObjForHarbour1 = cubeHexes [harbour1CubePos];
		UIHex currentHex1 = currentHexGameObjForHarbour1.GetComponent<UIHex> ();
		GameObject ltEdgeHarbour1 = currentHex1.transform.FindChild ("LTEdge").gameObject;
		UIEdge harbour1LTEdge = ltEdgeHarbour1.GetComponent<UIEdge> ();
		harbour1LTEdge.GetComponent<SpriteRenderer> ().color = new Color32 (220, 20, 60, 1);
		harbours.Add (ltEdgeHarbour1, StealableType.Resource_Brick);

		//harbour 2
		Vec3 harbour2CubePos = offsetOddRToCubeCoordinate (new Vector2(5,2));
		GameObject currentHexGameObjForHarbour2 = cubeHexes [harbour2CubePos];
		UIHex currentHex2 = currentHexGameObjForHarbour2.GetComponent<UIHex> ();
		GameObject rtEdgeHarbour2 = currentHex2.transform.FindChild ("RTEdge").gameObject;
		UIEdge harbour2RTEdge = rtEdgeHarbour2.GetComponent<UIEdge> ();
		harbour2RTEdge.GetComponent<SpriteRenderer> ().color = new Color32 (220, 20, 60, 1);
		harbours.Add (rtEdgeHarbour2, StealableType.Resource_Grain);

		//harbour 3
		Vec3 harbour3CubePos = offsetOddRToCubeCoordinate (new Vector2(6,3));
		GameObject currentHexGameObjForHarbour3 = cubeHexes [harbour3CubePos];
		UIHex currentHex3 = currentHexGameObjForHarbour3.GetComponent<UIHex> ();
		GameObject rtEdgeHarbour3 = currentHex3.transform.FindChild ("RTEdge").gameObject;
		UIEdge harbour3RTEdge = rtEdgeHarbour3.GetComponent<UIEdge> ();
		harbour3RTEdge.GetComponent<SpriteRenderer> ().color = new Color32 (220, 20, 60, 1);
		harbours.Add (rtEdgeHarbour3, StealableType.Resource_Wool);

		//harbour 4
		Vec3 harbour4CubePos = offsetOddRToCubeCoordinate (new Vector2(7,4));
		GameObject currentHexGameObjForHarbour4 = cubeHexes [harbour4CubePos];
		UIHex currentHex4 = currentHexGameObjForHarbour4.GetComponent<UIHex> ();
		GameObject rEdgeHarbour4 = currentHex4.transform.FindChild ("REdge").gameObject;
		UIEdge harbour4REdge = rEdgeHarbour4.GetComponent<UIEdge> ();
		harbour4REdge.GetComponent<SpriteRenderer> ().color = new Color32 (220, 20, 60, 1);
		harbours.Add (rEdgeHarbour4, StealableType.Resource_Lumber);

		//harbour 5
		Vec3 harbour5CubePos = offsetOddRToCubeCoordinate (new Vector2(6,5));
		GameObject currentHexGameObjForHarbour5 = cubeHexes [harbour5CubePos];
		UIHex currentHex5 = currentHexGameObjForHarbour5.GetComponent<UIHex> ();
		GameObject rbEdgeHarbour5 = currentHex5.transform.FindChild ("RBEdge").gameObject;
		UIEdge harbour5RBEdge = rbEdgeHarbour5.GetComponent<UIEdge> ();
		harbour5RBEdge.GetComponent<SpriteRenderer> ().color = new Color32 (220, 20, 60, 1);
		harbours.Add (rbEdgeHarbour5, StealableType.Resource_Ore);

		//harbour 6
		Vec3 harbour6CubePos = offsetOddRToCubeCoordinate (new Vector2(5,6));
		GameObject currentHexGameObjForHarbour6 = cubeHexes [harbour6CubePos];
		UIHex currentHex6 = currentHexGameObjForHarbour6.GetComponent<UIHex> ();
		GameObject rbEdgeHarbour6 = currentHex6.transform.FindChild ("RBEdge").gameObject;
		UIEdge harbour6RBEdge = rbEdgeHarbour6.GetComponent<UIEdge> ();
		harbour6RBEdge.GetComponent<SpriteRenderer> ().color = new Color32 (220, 20, 60, 1);
		harbours.Add (rbEdgeHarbour6, StealableType.Resource_Brick);

		//harbour 7
		Vec3 harbour7CubePos = offsetOddRToCubeCoordinate (new Vector2(4,6));
		GameObject currentHexGameObjForHarbour7 = cubeHexes [harbour7CubePos];
		UIHex currentHex7 = currentHexGameObjForHarbour7.GetComponent<UIHex> ();
		GameObject lbEdgeHarbour7 = currentHex7.transform.FindChild ("LBEdge").gameObject;
		UIEdge harbour7LBEdge = lbEdgeHarbour7.GetComponent<UIEdge> ();
		harbour7LBEdge.GetComponent<SpriteRenderer> ().color = new Color32 (220, 20, 60, 1);
		harbours.Add (lbEdgeHarbour7, StealableType.Resource_Grain);

		//harbour 8
		Vec3 harbour8CubePos = offsetOddRToCubeCoordinate (new Vector2(3,5));
		GameObject currentHexGameObjForHarbour8 = cubeHexes [harbour8CubePos];
		UIHex currentHex8 = currentHexGameObjForHarbour8.GetComponent<UIHex> ();
		GameObject lEdgeHarbour8 = currentHex8.transform.FindChild ("LEdge").gameObject;
		UIEdge harbour8LEdge = lEdgeHarbour8.GetComponent<UIEdge> ();
		harbour8LEdge.GetComponent<SpriteRenderer> ().color = new Color32 (220, 20, 60, 1);
		harbours.Add (lEdgeHarbour8, StealableType.Resource_Wool);

		//harbour 9
		Vec3 harbour9CubePos = offsetOddRToCubeCoordinate (new Vector2(3,3));
		GameObject currentHexGameObjForHarbour9 = cubeHexes [harbour9CubePos];
		UIHex currentHex9 = currentHexGameObjForHarbour9.GetComponent<UIHex> ();
		GameObject lEdgeHarbour9 = currentHex9.transform.FindChild ("LEdge").gameObject;
		UIEdge harbour9LEdge = lEdgeHarbour9.GetComponent<UIEdge> ();
		harbour9LEdge.GetComponent<SpriteRenderer> ().color = new Color32 (220, 20, 60, 1);
		harbours.Add (lEdgeHarbour9, StealableType.Resource_Wool);

	}

	public void buildFishingGrounds () {
		
	}

	// Use this for initialization
	void Start () 
	{
		setHexSizes ();
		buildHarbours();
	}

	// Update is called once per frame
	void Update () {

	}
}
