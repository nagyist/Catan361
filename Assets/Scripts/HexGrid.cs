using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour {

	public GameObject Hex;

	public int gridWidth = 10;
	public int gridHeight = 9;

	public Dictionary<Vector3, GameObject> cubeHexes;
	public EdgeCollection edges;
	public IntersectionCollection intersections;

	public Dictionary<int, Color> resourceColor = new Dictionary<int, Color>()
	{
		{1, new Color (0.255f, 0.255f, 0.205f, 1)},
		{2, new Color (0, 1, 0, 1)},
		{3, new Color (0.5f, 0.5f, 0.5f, 1)},
		{4, new Color (1, 0, 0, 1)},
		{5, new Color (1, 0.92f, 0.016f, 1)}
	};


	public Dictionary<int, StealableType> resourceType = new Dictionary<int, StealableType>()
	{
		{1, StealableType.Resource_Wool},
		{2, StealableType.Resource_Lumber},
		{3, StealableType.Resource_Ore},
		{4, StealableType.Resource_Brick},
		{5, StealableType.Resource_Grain}
	};


	private float hexWidth;
	private float hexHeight;

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
		GameObject thisHex = (GameObject)Instantiate (Hex);
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

	private Vector3 offsetOddRToCubeCoordinate(Vector2 oddR) {
		float x = oddR.x - (oddR.y - (float)((int)oddR.y & 1)) / 2.0f;
		float z = oddR.y;
		float y = -x - z;

		return new Vector3 (x, y, z);
	}

	void createHexGrid()
	{
		cubeHexes = new Dictionary<Vector3, GameObject> ();
		edges = new EdgeCollection ();
		intersections = new IntersectionCollection ();

		GameObject hexGridObject = new GameObject ("HexGrid");
		hexGridObject.transform.parent = this.transform;

		//Texture2D testerTexture;
		//testerTexture = (Texture2D)Resources.Load ("organic34.png");

		for (int y = 0; y < gridHeight; y++) 
		{
			for (int x = 0; x < gridWidth; x++) 
			{
				GameObject thisHex = (GameObject)Instantiate (Hex);
				int resourceNum = Random.Range (1, 5);
				Hex hexScript = thisHex.GetComponent<Hex> ();
				if (x == 3 && y > 2 && y < 6) 
				{
					//thisHex.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1);
					thisHex.GetComponent<SpriteRenderer> ().color = resourceColor [resourceNum];
					//thisHex.GetComponent<SpriteRenderer>().sprite = Resources.Load("organic34.png", ) as Sprite;

					if (x % 2 == 1) 
					{
						hexScript.selectedNum = Random.Range (1, 6);
					} 
					else 
					{
						hexScript.selectedNum = Random.Range (8, 12);
					}
				} 
				else if (x == 4 && y > 1 && y < 7) 
				{
					//thisHex.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1);
					thisHex.GetComponent<SpriteRenderer> ().color = resourceColor [resourceNum];

					if (y % 2 == 1) 
					{
						hexScript.selectedNum = Random.Range (1, 6);
					} 
					else 
					{
						hexScript.selectedNum = Random.Range (8, 12);
					}
				} 
				else if (x == 5 && y > 1 && y < 7 && y != 4) 
				{
					//thisHex.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1);
					thisHex.GetComponent<SpriteRenderer> ().color = resourceColor [resourceNum];

					if (y % 2 == 1) 
					{
						hexScript.selectedNum = Random.Range (1, 6);
					} 
					else 
					{
						hexScript.selectedNum = Random.Range (8, 12);
					}
				} 
				else if (x == 5 && y == 4) 
				{
					thisHex.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1);
					hexScript.selectedNum = 7;
				} 
				else if (x == 6 && y > 1 && y < 7) 
				{
					//thisHex.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1);
					thisHex.GetComponent<SpriteRenderer> ().color = resourceColor [resourceNum];

					if (y % 2 == 1) {
						hexScript.selectedNum = Random.Range (1, 6);
					} else {
						hexScript.selectedNum = Random.Range (8, 12);
					}

				} 
				else if (x == 6 && y > 3 && y < 7) 
				{
					//thisHex.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1);
					thisHex.GetComponent<SpriteRenderer> ().color = resourceColor [resourceNum];

					if (y % 2 == 1) {
						hexScript.selectedNum = Random.Range (1, 6);
					} else {
						hexScript.selectedNum = Random.Range (8, 12);
					}

				} 
				else if (x == 7 && y > 3 && y < 5) 
				{
					//thisHex.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1);
					thisHex.GetComponent<SpriteRenderer> ().color = resourceColor [resourceNum];

					if (y % 2 == 1) 
					{
						hexScript.selectedNum = Random.Range (1, 6);
					} 
					else 
					{
						hexScript.selectedNum = Random.Range (8, 12);
					}

				} 
				else 
				{
					thisHex.GetComponent<SpriteRenderer> ().color = new Color (0, 0, 1);
				}

				Vector2 gridPos = new Vector2(x, y);

				thisHex.transform.position = calcWorldCoord(gridPos);
				thisHex.transform.parent = hexGridObject.transform;

				hexScript.hexGridPosition = gridPos;
				hexScript.hexGridCubePosition = offsetOddRToCubeCoordinate (gridPos);

				// add to cube board
				cubeHexes.Add (hexScript.hexGridCubePosition, thisHex);
				// add edges
				foreach (Vector3 adjHex in hexScript.getAdjacentHexesPos()) {
					edges.addEdge (hexScript.hexGridCubePosition, adjHex);
				}
				// add intersections
				foreach(List<Vector3> adjHexInIntersection in hexScript.getIntersectionsAdjacentPos()) {
					intersections.addIntersection (adjHexInIntersection [0], adjHexInIntersection [1], adjHexInIntersection [2]);
				}
			}
		}

		// attach edges and intersection to UI
		foreach(Vector3 tilePos in cubeHexes.Keys) {
			GameObject currentHexGameObj = cubeHexes [tilePos];
			Hex currentHex = currentHexGameObj.GetComponent<Hex> ();
			Vector3 currentCubePos = currentHex.hexGridCubePosition;

			//edges
			// get each edge instance 
			Edge rightEdge = edges.getEdge(currentCubePos, currentHex.getAdjacentHexPos(global::Hex.AdjHex.LEFT));
			Edge rightTopEdge = edges.getEdge (currentCubePos, currentHex.getAdjacentHexPos (global::Hex.AdjHex.RIGHT_TOP));
			Edge leftTopEdge = edges.getEdge (currentCubePos, currentHex.getAdjacentHexPos (global::Hex.AdjHex.LEFT_TOP));
			Edge leftEdge = edges.getEdge (currentCubePos, currentHex.getAdjacentHexPos (global::Hex.AdjHex.LEFT));
			Edge leftBottomEdge = edges.getEdge (currentCubePos, currentHex.getAdjacentHexPos (global::Hex.AdjHex.LEFT_BOTTOM));
			Edge rightBottomEdge = edges.getEdge (currentCubePos, currentHex.getAdjacentHexPos (global::Hex.AdjHex.RIGHT_BOTTOM));

			// add each edge instance as a component to corresponding edge game object
			GameObject rightEdgeGameObj = currentHexGameObj.transform.FindChild("REdge").gameObject;
			UIEdge rightEdgeUi = rightEdgeGameObj.AddComponent<UIEdge> ();
			rightEdgeUi.referencedEdge = rightEdge;

			GameObject rightTopEdgeGameObj = currentHexGameObj.transform.FindChild("RTEdge").gameObject;
			UIEdge rightTopEdgeUi = rightTopEdgeGameObj.AddComponent<UIEdge> ();
			rightTopEdgeUi.referencedEdge = rightTopEdge;

			GameObject leftTopEdgeGameObj = currentHexGameObj.transform.FindChild("LTEdge").gameObject;
			UIEdge leftTopEdgeUi = leftTopEdgeGameObj.AddComponent<UIEdge> ();
			leftTopEdgeUi.referencedEdge = leftTopEdge;

			GameObject leftEdgeGameObj = currentHexGameObj.transform.FindChild("LEdge").gameObject;
			UIEdge leftEdgeUi = leftEdgeGameObj.AddComponent<UIEdge> ();
			leftEdgeUi.referencedEdge = leftEdge;

			GameObject leftBottomEdgeGameObj = currentHexGameObj.transform.FindChild("LBEdge").gameObject;
			UIEdge leftBottomEdgeUi = leftBottomEdgeGameObj.AddComponent<UIEdge> ();
			leftBottomEdgeUi.referencedEdge = leftBottomEdge;

			GameObject rightBottomEdgeGameObj = currentHexGameObj.transform.FindChild("RBEdge").gameObject;
			UIEdge rightBottomEdgeUi = rightBottomEdgeGameObj.AddComponent<UIEdge> ();
			rightBottomEdgeUi.referencedEdge = rightBottomEdge;

			// intersections
			// get each intersection instance
			Intersection leftTopIntersection = intersections.getIntersection(currentHex.getIntersectionAdjacentHexPos(global::Hex.HexIntersection.LEFT_TOP));
			Intersection topIntersection = intersections.getIntersection(currentHex.getIntersectionAdjacentHexPos(global::Hex.HexIntersection.TOP));
			Intersection rightTopIntersection = intersections.getIntersection(currentHex.getIntersectionAdjacentHexPos(global::Hex.HexIntersection.RIGHT_TOP));
			Intersection rightBottomIntersection = intersections.getIntersection(currentHex.getIntersectionAdjacentHexPos(global::Hex.HexIntersection.RIGHT_BOTTOM));
			Intersection bottomIntersection = intersections.getIntersection(currentHex.getIntersectionAdjacentHexPos(global::Hex.HexIntersection.BOTTOM));
			Intersection leftBottomIntersection = intersections.getIntersection(currentHex.getIntersectionAdjacentHexPos(global::Hex.HexIntersection.LEFT_BOTTOM));

			// add each intersection instance as a component to corresponding intersection game object
			GameObject leftTopGameObj = currentHexGameObj.transform.FindChild("LTIntersection").gameObject;
			UIIntersection leftTopIntersectionUi = leftTopGameObj.AddComponent<UIIntersection> ();
			leftTopIntersectionUi.referencedIntersection = leftTopIntersection;

			GameObject topGameObj = currentHexGameObj.transform.FindChild("TIntersection").gameObject;
			UIIntersection topIntersectionUi = topGameObj.AddComponent<UIIntersection> ();
			topIntersectionUi.referencedIntersection = topIntersection;

			GameObject rightTopGameObj = currentHexGameObj.transform.FindChild("RTIntersection").gameObject;
			UIIntersection rightTopIntersectionUi = rightTopGameObj.AddComponent<UIIntersection> ();
			rightTopIntersectionUi.referencedIntersection = rightTopIntersection;

			GameObject rightBottomGameObj = currentHexGameObj.transform.FindChild("RBIntersection").gameObject;
			UIIntersection rightBottomIntersectionUi = rightBottomGameObj.AddComponent<UIIntersection> ();
			rightBottomIntersectionUi.referencedIntersection = rightBottomIntersection;

			GameObject bottomGameObj = currentHexGameObj.transform.FindChild("BIntersection").gameObject;
			UIIntersection bottomIntersectionUi = bottomGameObj.AddComponent<UIIntersection> ();
			bottomIntersectionUi.referencedIntersection = bottomIntersection;

			GameObject leftBottomGameObj = currentHexGameObj.transform.FindChild("LBIntersection").gameObject;
			UIIntersection leftBottomIntersectionUi = leftBottomGameObj.AddComponent<UIIntersection> ();
			leftBottomIntersectionUi.referencedIntersection = leftBottomIntersection;
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

	// Use this for initialization
	void Start () 
	{
		setHexSizes ();
		createHexGrid ();
	}

	// Update is called once per frame
	void Update () {

	}
}
