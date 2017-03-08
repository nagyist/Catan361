using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour {

	public GameObject Hex;
	public GameObject Harbour;

	public int gridWidth = 10;
	public int gridHeight = 9;

	private Dictionary<Vec3, GameObject> cubeHexes;

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
	};

	private void setHexSizes () 
	{
		hexWidth = Hex.GetComponent<Renderer> ().bounds.size.x;
		hexHeight = Hex.GetComponent<Renderer> ().bounds.size.y;
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
						refTile.SelectedNum = Random.Range (1, 6);
					} 
					else 
					{
						refTile.SelectedNum = Random.Range (8, 12);
					}
				} 
				else if (x == 4 && y > 1 && y < 7) 
				{
					if (y % 2 == 1) 
					{
						refTile.SelectedNum = Random.Range (1, 6);
					} 
					else 
					{
						refTile.SelectedNum = Random.Range (8, 12);
					}
				} 
				else if (x == 5 && y > 1 && y < 7 && y != 4) 
				{
					if (y % 2 == 1) 
					{
						refTile.SelectedNum = Random.Range (1, 6);
					} 
					else 
					{
						refTile.SelectedNum = Random.Range (8, 12);
					}
				} 
				else if (x == 5 && y == 4) 
				{
					refTile.SelectedNum = 7;
				} 
				else if (x == 6 && y > 1 && y < 7) 
				{
					if (y % 2 == 1) {
						refTile.SelectedNum = Random.Range (1, 6);
					} else {
						refTile.SelectedNum = Random.Range (8, 12);
					}

				} 
				else if (x == 6 && y > 3 && y < 7) 
				{
					if (y % 2 == 1) {
						refTile.SelectedNum = Random.Range (1, 6);
					} else {
						refTile.SelectedNum = Random.Range (8, 12);
					}

				} 
				else if (x == 7 && y > 3 && y < 5) 
				{
					if (y % 2 == 1) 
					{
						refTile.SelectedNum = Random.Range (1, 6);
					} 
					else 
					{
						refTile.SelectedNum = Random.Range (8, 12);
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

	public void CreateUIHexGrid()
	{
		cubeHexes = new Dictionary<Vec3, GameObject> ();

		GameObject hexGridObject = new GameObject ("HexGrid");
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
			Edge rightEdge = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(currentCubePos, currentHex.getAdjacentHexPos(global::UIHex.AdjHex.LEFT));
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

			GameObject rightTopEdgeGameObj = currentHexGameObj.transform.FindChild("RTEdge").gameObject;
			UIEdge rightTopEdgeUi = rightTopEdgeGameObj.AddComponent<UIEdge> ();
			rightTopEdgeUi.HexPos1 = rightTopEdge.adjTile1;
			rightTopEdgeUi.HexPos2 = rightTopEdge.adjTile1;

			GameObject leftTopEdgeGameObj = currentHexGameObj.transform.FindChild("LTEdge").gameObject;
			UIEdge leftTopEdgeUi = leftTopEdgeGameObj.AddComponent<UIEdge> ();
			leftTopEdgeUi.HexPos1 = leftTopEdge.adjTile1;
			leftTopEdgeUi.HexPos2 = leftTopEdge.adjTile2;

			GameObject leftEdgeGameObj = currentHexGameObj.transform.FindChild("LEdge").gameObject;
			UIEdge leftEdgeUi = leftEdgeGameObj.AddComponent<UIEdge> ();
			leftEdgeUi.HexPos1 = leftEdge.adjTile1;
			leftEdgeUi.HexPos2 = leftEdge.adjTile2;

			GameObject leftBottomEdgeGameObj = currentHexGameObj.transform.FindChild("LBEdge").gameObject;
			UIEdge leftBottomEdgeUi = leftBottomEdgeGameObj.AddComponent<UIEdge> ();
			leftBottomEdgeUi.HexPos1 = leftBottomEdge.adjTile1;
			leftBottomEdgeUi.HexPos2 = leftBottomEdge.adjTile1;

			GameObject rightBottomEdgeGameObj = currentHexGameObj.transform.FindChild("RBEdge").gameObject;
			UIEdge rightBottomEdgeUi = rightBottomEdgeGameObj.AddComponent<UIEdge> ();
			rightBottomEdgeUi.HexPos1 = rightBottomEdge.adjTile1;
			rightBottomEdgeUi.HexPos2 = rightBottomEdge.adjTile2;

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

			GameObject topGameObj = currentHexGameObj.transform.FindChild("TIntersection").gameObject;
			UIIntersection topIntersectionUi = topGameObj.AddComponent<UIIntersection> ();
			topIntersectionUi.HexPos1 = topIntersection.adjTile1;
			topIntersectionUi.HexPos2 = topIntersection.adjTile2;
			topIntersectionUi.HexPos3 = topIntersection.adjTile3;

			GameObject rightTopGameObj = currentHexGameObj.transform.FindChild("RTIntersection").gameObject;
			UIIntersection rightTopIntersectionUi = rightTopGameObj.AddComponent<UIIntersection> ();
			rightTopIntersectionUi.HexPos1 = rightTopIntersection.adjTile1;
			rightTopIntersectionUi.HexPos2 = rightTopIntersection.adjTile2;
			rightTopIntersectionUi.HexPos3 = rightTopIntersection.adjTile3;

			GameObject rightBottomGameObj = currentHexGameObj.transform.FindChild("RBIntersection").gameObject;
			UIIntersection rightBottomIntersectionUi = rightBottomGameObj.AddComponent<UIIntersection> ();
			rightBottomIntersectionUi.HexPos1 = rightBottomIntersection.adjTile1;
			rightBottomIntersectionUi.HexPos2 = rightBottomIntersection.adjTile2;
			rightBottomIntersectionUi.HexPos3 = rightBottomIntersection.adjTile3;

			GameObject bottomGameObj = currentHexGameObj.transform.FindChild("BIntersection").gameObject;
			UIIntersection bottomIntersectionUi = bottomGameObj.AddComponent<UIIntersection> ();
			bottomIntersectionUi.HexPos1 = bottomIntersection.adjTile1;
			bottomIntersectionUi.HexPos2 = bottomIntersection.adjTile2;
			bottomIntersectionUi.HexPos3 = bottomIntersection.adjTile3;

			GameObject leftBottomGameObj = currentHexGameObj.transform.FindChild("LBIntersection").gameObject;
			UIIntersection leftBottomIntersectionUi = leftBottomGameObj.AddComponent<UIIntersection> ();
			leftBottomIntersectionUi.HexPos1 = leftBottomIntersection.adjTile1;
			leftBottomIntersectionUi.HexPos2 = leftBottomIntersection.adjTile2;
			leftBottomIntersectionUi.HexPos3 = leftBottomIntersection.adjTile3;
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
		
		
		Dictionary<string, GameObject> harbourCollection = new Dictionary<string, GameObject>();
		
		Vector2 gridPosHex1 = new Vector2 (4, 1);
		GameObject newHarbour1 = (GameObject) Instantiate (Harbour);
		newHarbour1.transform.parent = this.transform;
		newHarbour1.transform.position = calcWorldCoord (gridPosHex1);

		TextMesh harbour1Text = newHarbour1.GetComponentInChildren<TextMesh>();
		harbour1Text.text = "1";
		harbourCollection.Add("1", newHarbour1);

		Harbour harbourScript1 = newHarbour1.GetComponent<Harbour> ();
		harbourScript1.exchangeRate = 4;
		harbourScript1.returnedResource = StealableType.Resource_Brick;
		harbourScript1.returnedAmount = 1;


		Vector2 gridPosHex2 = new Vector2 (5, 1);
		GameObject newHarbour2 = (GameObject) Instantiate (Harbour);
		newHarbour2.transform.parent = this.transform;
		newHarbour2.transform.position = calcWorldCoord (gridPosHex2);

		TextMesh harbour2Text = newHarbour2.GetComponentInChildren<TextMesh>();
		harbour2Text.text = "2";
		harbourCollection.Add("2", newHarbour2);

		Harbour harbourScript2 = newHarbour2.GetComponent<Harbour> ();
		harbourScript2.exchangeRate = 4;
		harbourScript2.returnedResource = StealableType.Resource_Grain;
		harbourScript2.returnedAmount = 1;


		Vector2 gridPosHex3 = new Vector2 (7, 2);
		GameObject newHarbour3 = (GameObject) Instantiate (Harbour);
		newHarbour3.transform.parent = this.transform;
		newHarbour3.transform.position = calcWorldCoord (gridPosHex3);

		TextMesh harbour3Text = newHarbour3.GetComponentInChildren<TextMesh>();
		harbour3Text.text = "3";
		harbourCollection.Add("3", newHarbour3);

		Harbour harbourScript3 = newHarbour3.GetComponent<Harbour> ();
		harbourScript3.exchangeRate = 4;
		harbourScript3.returnedResource = StealableType.Resource_Lumber;
		harbourScript3.returnedAmount = 1;


		Vector2 gridPosHex4 = new Vector2 (7, 3);
		GameObject newHarbour4 = (GameObject) Instantiate (Harbour);
		newHarbour4.transform.parent = this.transform;
		newHarbour4.transform.position = calcWorldCoord (gridPosHex4);

		TextMesh harbour4Text = newHarbour4.GetComponentInChildren<TextMesh>();
		harbour4Text.text = "4";
		harbourCollection.Add("4", newHarbour4);

		Harbour harbourScript4 = newHarbour4.GetComponent<Harbour> ();
		harbourScript4.exchangeRate = 4;
		harbourScript4.returnedResource = StealableType.Resource_Ore;
		harbourScript4.returnedAmount = 1;


		Vector2 gridPosHex5 = new Vector2 (8, 4);
		GameObject newHarbour5 = (GameObject) Instantiate (Harbour);
		newHarbour5.transform.parent = this.transform;
		newHarbour5.transform.position = calcWorldCoord (gridPosHex5);

		TextMesh harbour5Text = newHarbour5.GetComponentInChildren<TextMesh>();
		harbour5Text.text = "5";
		harbourCollection.Add("5", newHarbour5);

		Harbour harbourScript5 = newHarbour5.GetComponent<Harbour> ();
		harbourScript5.exchangeRate = 4;
		harbourScript5.returnedResource = StealableType.Resource_Wool;
		harbourScript5.returnedAmount = 1;


		Vector2 gridPosHex6 = new Vector2 (7, 5);
		GameObject newHarbour6 = (GameObject) Instantiate (Harbour);
		newHarbour6.transform.parent = this.transform;
		newHarbour6.transform.position = calcWorldCoord (gridPosHex6);

		TextMesh harbour6Text = newHarbour6.GetComponentInChildren<TextMesh>();
		harbour6Text.text = "6";
		harbourCollection.Add("6", newHarbour6);

		Harbour harbourScript6 = newHarbour6.GetComponent<Harbour> ();
		harbourScript6.exchangeRate = 4;
		harbourScript6.returnedResource = StealableType.Resource_Brick;
		harbourScript6.returnedAmount = 1;


		Vector2 gridPosHex7 = new Vector2 (7, 6);
		GameObject newHarbour7 = (GameObject) Instantiate (Harbour);
		newHarbour7.transform.parent = this.transform;
		newHarbour7.transform.position = calcWorldCoord (gridPosHex7);

		TextMesh harbour7Text = newHarbour7.GetComponentInChildren<TextMesh>();
		harbour7Text.text = "7";
		harbourCollection.Add("7", newHarbour7);

		Harbour harbourScript7 = newHarbour7.GetComponent<Harbour> ();
		harbourScript7.exchangeRate = 4;
		harbourScript7.returnedResource = StealableType.Resource_Grain;
		harbourScript7.returnedAmount = 1;


		Vector2 gridPosHex8 = new Vector2 (5, 7);
		GameObject newHarbour8 = (GameObject) Instantiate (Harbour);
		newHarbour8.transform.parent = this.transform;
		newHarbour8.transform.position = calcWorldCoord (gridPosHex8);

		TextMesh harbour8Text = newHarbour8.GetComponentInChildren<TextMesh>();
		harbour8Text.text = "8";
		harbourCollection.Add("8", newHarbour8);

		Harbour harbourScript8 = newHarbour8.GetComponent<Harbour> ();
		harbourScript8.exchangeRate = 4;
		harbourScript8.returnedResource = StealableType.Resource_Lumber;
		harbourScript8.returnedAmount = 1;


		Vector2 gridPosHex9 = new Vector2 (4, 7);
		GameObject newHarbour9 = (GameObject) Instantiate (Harbour);
		newHarbour9.transform.parent = this.transform;
		newHarbour9.transform.position = calcWorldCoord (gridPosHex9);

		TextMesh harbour9Text = newHarbour9.GetComponentInChildren<TextMesh>();
		harbour9Text.text = "9";
		harbourCollection.Add("9", newHarbour9);

		Harbour harbourScript9 = newHarbour9.GetComponent<Harbour> ();
		harbourScript9.exchangeRate = 4;
		harbourScript9.returnedResource = StealableType.Resource_Ore;
		harbourScript9.returnedAmount = 1;


		Vector2 gridPosHex10 = new Vector2 (3, 6);
		GameObject newHarbour10 = (GameObject) Instantiate (Harbour);
		newHarbour10.transform.parent = this.transform;
		newHarbour10.transform.position = calcWorldCoord (gridPosHex10);

		TextMesh harbour10Text = newHarbour10.GetComponentInChildren<TextMesh>();
		harbour10Text.text = "10";
		harbourCollection.Add("10", newHarbour10);

		Harbour harbourScript10 = newHarbour10.GetComponent<Harbour> ();
		harbourScript10.exchangeRate = 4;
		harbourScript10.returnedResource = StealableType.Resource_Wool;
		harbourScript10.returnedAmount = 1;


		Vector2 gridPosHex11 = new Vector2 (2, 5);
		GameObject newHarbour11 = (GameObject) Instantiate (Harbour);
		newHarbour11.transform.parent = this.transform;
		newHarbour11.transform.position = calcWorldCoord (gridPosHex11);

		TextMesh harbour11Text = newHarbour11.GetComponentInChildren<TextMesh>();
		harbour11Text.text = "11";
		harbourCollection.Add("11", newHarbour11);

		Harbour harbourScript11 = newHarbour11.GetComponent<Harbour> ();
		harbourScript11.exchangeRate = 4;
		harbourScript11.returnedResource = StealableType.Resource_Brick;
		harbourScript11.returnedAmount = 1;


		Vector2 gridPosHex12 = new Vector2 (2, 4);
		GameObject newHarbour12 = (GameObject) Instantiate (Harbour);
		newHarbour12.transform.parent = this.transform;
		newHarbour12.transform.position = calcWorldCoord (gridPosHex12);

		TextMesh harbour12Text = newHarbour12.GetComponentInChildren<TextMesh>();
		harbour12Text.text = "12";
		harbourCollection.Add("12", newHarbour12);

		Harbour harbourScript12 = newHarbour12.GetComponent<Harbour> ();
		harbourScript12.exchangeRate = 4;
		harbourScript12.returnedResource = StealableType.Resource_Grain;
		harbourScript12.returnedAmount = 1;


		Vector2 gridPosHex13 = new Vector2 (2, 3);
		GameObject newHarbour13 = (GameObject) Instantiate (Harbour);
		newHarbour13.transform.parent = this.transform;
		newHarbour13.transform.position = calcWorldCoord (gridPosHex13);

		TextMesh harbour13Text = newHarbour13.GetComponentInChildren<TextMesh>();
		harbour13Text.text = "13";
		harbourCollection.Add("13", newHarbour13);

		Harbour harbourScript13 = newHarbour12.GetComponent<Harbour> ();
		harbourScript13.exchangeRate = 4;
		harbourScript13.returnedResource = StealableType.Resource_Lumber;
		harbourScript13.returnedAmount = 1;


		Vector2 gridPosHex14 = new Vector2 (3, 2);
		GameObject newHarbour14 = (GameObject) Instantiate (Harbour);
		newHarbour14.transform.parent = this.transform;
		newHarbour14.transform.position = calcWorldCoord (gridPosHex14);

		TextMesh harbour14Text = newHarbour14.GetComponentInChildren<TextMesh>();
		harbour14Text.text = "14";
		harbourCollection.Add("14", newHarbour14);

		Harbour harbourScript14 = newHarbour14.GetComponent<Harbour> ();
		harbourScript14.exchangeRate = 4;
		harbourScript14.returnedResource = StealableType.Resource_Ore;
		harbourScript14.returnedAmount = 1;

		

		
		/*harbours = new Dictionary<Edge, StealableType> ();
		Vector3 hex1 = new Vector3 (3, -5, 2);
		GameObject currentHexGameObj1 = cubeHexes [new Vector3(3, -5, 2)];
		Hex hexScript1 = currentHexGameObj1.GetComponent<Hex> ();
		Vector3 hex2 = hexScript1.getAdjacentHexPos (global::Hex.AdjHex.LEFT_TOP);
		Edge harbouredEdge1 = edges.getEdge (hex1, hex2);
		harbours.Add (harbouredEdge1, StealableType.Resource_Brick);

		Vector3 hex3 = new Vector3 (4, -6, 2);
		GameObject currentHexGameObj2 = cubeHexes [hex3];
		Hex hexScript2 = currentHexGameObj2.GetComponent<Hex> ();
		Vector3 hex4 = hexScript2.getAdjacentHexPos (global::Hex.AdjHex.LEFT_TOP);
		Edge harbouredEdge2 = edges.getEdge (hex3, hex4);
		harbours.Add (harbouredEdge2, StealableType.Resource_Grain);

		Vector3 hex5 = new Vector3 (5, -7, 2);
		GameObject currentHexGameObj3 = cubeHexes [hex5];
		Hex hexScript3 = currentHexGameObj3.GetComponent<Hex> ();
		Vector3 hex6 = hexScript3.getAdjacentHexPos (global::Hex.AdjHex.LEFT_TOP);
		Edge harbouredEdge3 = edges.getEdge (hex5, hex6);
		harbours.Add (harbouredEdge3, StealableType.Resource_Lumber);

		Vector3 hex7 = new Vector3 (5, -8, 3);
		GameObject currentHexGameObj4 = cubeHexes [hex7];
		Hex hexScript4 = currentHexGameObj4.GetComponent<Hex> ();
		Vector3 hex8 = hexScript4.getAdjacentHexPos (global::Hex.AdjHex.RIGHT_TOP);
		Edge harbouredEdge4 = edges.getEdge (hex7, hex8);
		harbours.Add (harbouredEdge4, StealableType.Resource_Ore);

		Vector3 hex9 = new Vector3 (5, -9, 4);
		GameObject currentHexGameObj5 = cubeHexes [hex9];
		Hex hexScript5 = currentHexGameObj5.GetComponent<Hex> ();
		Vector3 hex10 = hexScript5.getAdjacentHexPos (global::Hex.AdjHex.RIGHT_TOP);
		Edge harbouredEdge5 = edges.getEdge (hex9, hex10);
		harbours.Add (harbouredEdge5, StealableType.Resource_Wool);

		Vector3 hex11 = new Vector3 (4, -9, 5);
		GameObject currentHexGameObj6 = cubeHexes [hex11];
		Hex hexScript6 = currentHexGameObj6.GetComponent<Hex> ();
		Vector3 hex12 = hexScript6.getAdjacentHexPos (global::Hex.AdjHex.RIGHT);
		Edge harbouredEdge6 = edges.getEdge (hex11, hex12);
		harbours.Add (harbouredEdge6, StealableType.Resource_Brick);

		Vector3 hex13 = new Vector3 (3, -9, 6);
		GameObject currentHexGameObj7 = cubeHexes [hex13];
		Hex hexScript7 = currentHexGameObj7.GetComponent<Hex> ();
		Vector3 hex14 = hexScript7.getAdjacentHexPos (global::Hex.AdjHex.RIGHT_BOTTOM);
		Edge harbouredEdge7 = edges.getEdge (hex13, hex14);
		harbours.Add (harbouredEdge7, StealableType.Resource_Grain);

		Vector3 hex15 = new Vector3 (2, -8, 6);
		GameObject currentHexGameObj8 = cubeHexes [hex15];
		Hex hexScript8 = currentHexGameObj8.GetComponent<Hex> ();
		Vector3 hex16 = hexScript8.getAdjacentHexPos (global::Hex.AdjHex.RIGHT_BOTTOM);
		Edge harbouredEdge8 = edges.getEdge (hex15, hex16);
		harbours.Add (harbouredEdge8, StealableType.Resource_Lumber);

		Vector3 hex17 = new Vector3 (1, -7, 6);
		GameObject currentHexGameObj9 = cubeHexes [hex17];
		Hex hexScript9 = currentHexGameObj9.GetComponent<Hex> ();
		Vector3 hex18 = hexScript9.getAdjacentHexPos (global::Hex.AdjHex.RIGHT_BOTTOM);
		Edge harbouredEdge9 = edges.getEdge (hex17, hex18);
		harbours.Add (harbouredEdge9, StealableType.Resource_Ore);

		Vector3 hex19 = new Vector3 (1, -6, 5);
		GameObject currentHexGameObj10 = cubeHexes [hex19];
		Hex hexScript10 = currentHexGameObj10.GetComponent<Hex> ();
		Vector3 hex20 = hexScript10.getAdjacentHexPos (global::Hex.AdjHex.LEFT_BOTTOM);
		Edge harbouredEdge10 = edges.getEdge (hex19, hex20);
		harbours.Add (harbouredEdge10, StealableType.Resource_Wool);

		Vector3 hex21 = new Vector3 (1, -5, 4);
		GameObject currentHexGameObj11 = cubeHexes [hex21];
		Hex hexScript11 = currentHexGameObj11.GetComponent<Hex> ();
		Vector3 hex22 = hexScript11.getAdjacentHexPos (global::Hex.AdjHex.LEFT_BOTTOM);
		Edge harbouredEdge11 = edges.getEdge (hex21, hex22);
		harbours.Add (harbouredEdge11, StealableType.Resource_Brick);

		Vector3 hex23 = new Vector3 (2, -5, 3);
		GameObject currentHexGameObj12 = cubeHexes [hex23];
		Hex hexScript12 = currentHexGameObj12.GetComponent<Hex> ();
		Vector3 hex24 = hexScript12.getAdjacentHexPos (global::Hex.AdjHex.LEFT);
		Edge harbouredEdge12 = edges.getEdge (hex23, hex24);
		harbours.Add (harbouredEdge12, StealableType.Resource_Grain);*/

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
