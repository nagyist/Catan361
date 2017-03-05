using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour {

	public GameObject Hex;

	public int gridWidth = 10;
	public int gridHeight = 9;

	public Dictionary<Vector3, GameObject> cubeHexes;
	public EdgeCollection edges;

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
		GameObject hexGridObject = new GameObject ("HexGrid");
		hexGridObject.transform.parent = this.transform;

		for (int y = 0; y < gridHeight; y++) 
		{
			for (int x = 0; x < gridWidth; x++) 
			{
				GameObject thisHex = (GameObject)Instantiate (Hex);
				Hex hexScript = thisHex.GetComponent<Hex> ();
				if (x == 3 && y > 2 && y < 6) 
				{
					thisHex.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1);

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
					thisHex.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1);

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
					thisHex.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1);

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
					thisHex.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1);

					if (y % 2 == 1) {
						hexScript.selectedNum = Random.Range (1, 6);
					} else {
						hexScript.selectedNum = Random.Range (8, 12);
					}

				} 
				else if (x == 6 && y > 3 && y < 7) 
				{
					thisHex.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1);

					if (y % 2 == 1) {
						hexScript.selectedNum = Random.Range (1, 6);
					} else {
						hexScript.selectedNum = Random.Range (8, 12);
					}

				} 
				else if (x == 7 && y > 3 && y < 5) 
				{
					thisHex.GetComponent<SpriteRenderer> ().color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1);

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
				foreach (Vector3 adjHex in hexScript.getAdjacentHexesPos()) {
					Edge newEdge = new Edge (hexScript.hexGridCubePosition, adjHex);
					edges.addEdge (newEdge);
				}
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
