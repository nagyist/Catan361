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

	public Dictionary<int, int> landNums;
	public Dictionary<int, int> seaNums;

	private float hexWidth;
	private float hexHeight;

	public Dictionary<int, StealableType> resourceType = new Dictionary<int, StealableType>()
	{
		{1, StealableType.Resource_Wool},
		{2, StealableType.Resource_Lumber},
		{3, StealableType.Resource_Ore},
		{4, StealableType.Resource_Brick},
		{5, StealableType.Resource_Grain},
		{6, StealableType.Resource_Gold}
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

		int numL_2 = 1;
		int numL_3 = 2;
		int numL_4 = 2;
		int numL_5 = 2;
		int numL_6 = 2;
		int numL_8 = 2;
		int numL_9 = 2;
		int numL_10 = 2;
		int numL_11 = 2;
		int numL_12 = 1;

		landNums = new Dictionary <int, int> ();

		landNums.Add(2, numL_2);
		landNums.Add(3, numL_3);
		landNums.Add(4, numL_4);
		landNums.Add(5, numL_5);
		landNums.Add(6, numL_6);
		landNums.Add(8, numL_8);
		landNums.Add(9, numL_9);
		landNums.Add(10, numL_10);
		landNums.Add(11, numL_11);
		landNums.Add(12, numL_12);

		seaNums = new Dictionary <int, int> () ;

		int numW_2 = 1;
		int numW_3 = 1;
		int numW_4 = 1;
		int numW_5 = 1;
		int numW_6 = 1;
		int numW_8 = 1;
		int numW_9 = 1;
		int numW_10 = 1;
		int numW_11 = 1;

		seaNums.Add(2, numW_2);
		seaNums.Add(3, numW_3);
		seaNums.Add(4, numW_4);
		seaNums.Add(5, numW_5);
		seaNums.Add(6, numW_6);
		seaNums.Add(8, numW_8);
		seaNums.Add(9, numW_9);
		seaNums.Add(10, numW_10);
		seaNums.Add(11, numW_11);

		Dictionary <int, int> resourceLandTiles = new Dictionary <int, int> () 
		{
			/*
			{ 1, StealableType.Resource_Wool },
			{ 2, StealableType.Resource_Lumber },
			{ 3, StealableType.Resource_Ore },
			{ 4, StealableType.Resource_Brick },
			{ 5, StealableType.Resource_Grain },
			*/

			{ 1, 4 },
			{ 2, 4 },
			{ 3, 3 },
			{ 4, 3 },
			{ 5, 4 },
		};

		Dictionary <int, int> resourceSeaTiles = new Dictionary <int, int> () 
		{
			/*
			{ 1, StealableType.Resource_Wool },
			{ 2, StealableType.Resource_Lumber },
			{ 3, StealableType.Resource_Ore },
			{ 4, StealableType.Resource_Brick },
			{ 5, StealableType.Resource_Grain },
			*/

			{ 1, 1 },
			{ 2, 1 },
			{ 3, 2 },
			{ 4, 2 },
			{ 5, 1 },
		};


		for (int y = 0; y < gridHeight; y++) {
			for (int x = 0; x < gridWidth; x++) {
				HexTile refTile = new HexTile ();

				int resourceTypeNum = Random.Range (1, 6);

				int randomNumTile1 = Random.Range (2, 7);
				int randomNumTile2 = Random.Range (8, 13);
				int randomNumSeaTile1 = Random.Range (2, 7);
				int randomNumSeaTile2 = Random.Range (8, 12);

				//Land tiles
				if (x == 3 && y > 2 && y < 6) 
				{
					if (resourceLandTiles [resourceTypeNum] > 0) 
					{
						refTile.Resource = resourceType [resourceTypeNum];
						resourceLandTiles [resourceTypeNum]--;
					} 
					else 
					{
						int changeCount = 1;
						Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
						foreach (KeyValuePair<int, int> entry in resourceLandTiles) 
						{
							if (entry.Value != 0 && changeCount > 0) 
							{
								refTile.Resource = resourceType [entry.Key];
								keysToNuke.Add(entry.Key, entry.Value);
								changeCount--;
							}
						}
						foreach (KeyValuePair <int, int> entry in keysToNuke)
						{
							resourceLandTiles [entry.Key]--;
						}
					}
						
					if (y % 2 == 1) 
					{
						if (landNums [randomNumTile1] > 0) 
						{
							refTile.SelectedNum = randomNumTile1;
							landNums [randomNumTile1]--;
						} 
						else if (landNums [randomNumTile2] > 0) 
						{
							refTile.SelectedNum = randomNumTile2;
							landNums [randomNumTile2]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in landNums) 
							{
								if (entry.Value != 0 && changeCount > 0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						} else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								landNums [entry.Key]--;
							}
						}
					} 
					else 
					{
						if (landNums [randomNumTile2] > 0) 
						{
							refTile.SelectedNum = randomNumTile2;
							landNums [randomNumTile2]--;
						} 
						else if (landNums [randomNumTile1] > 0) 
						{
							refTile.SelectedNum = randomNumTile1;
							landNums [randomNumTile1]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in landNums) 
							{
								if (entry.Value != 0 && changeCount > 0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								landNums [entry.Key]--;
							}
						}
					}
				} 
				else if (x == 4 && y > 1 && y < 7) 
				{
					if (resourceLandTiles [resourceTypeNum] > 0) 
					{
						refTile.Resource = resourceType [resourceTypeNum];
						resourceLandTiles [resourceTypeNum]--;
					} 
					else 
					{
						int changeCount = 1;
						Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
						foreach (KeyValuePair<int, int> entry in resourceLandTiles) 
						{
							if (entry.Value != 0 && changeCount > 0) 
							{
								refTile.Resource = resourceType [entry.Key];
								keysToNuke.Add(entry.Key, entry.Value);
								changeCount--;
							}
						}
						foreach (KeyValuePair <int, int> entry in keysToNuke)
						{
							resourceLandTiles [entry.Key]--;
						}
					}

					if (y % 2 == 1) 
					{
						if (landNums [randomNumTile1] > 0) 
						{
							refTile.SelectedNum = randomNumTile1;
							landNums [randomNumTile1]--;
						} 
						else if (landNums [randomNumTile2] > 0) 
						{
							refTile.SelectedNum = randomNumTile2;
							landNums [randomNumTile2]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in landNums) 
							{
								if (entry.Value != 0 && changeCount > 0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								landNums [entry.Key]--;
							}
						}
					} 
					else 
					{
						if (landNums [randomNumTile2] > 0) 
						{
							refTile.SelectedNum = randomNumTile2;
							landNums [randomNumTile2]--;
						} 
						else if (landNums [randomNumTile1] > 0) 
						{
							refTile.SelectedNum = randomNumTile1;
							landNums [randomNumTile1]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in landNums) 
							{
								if (entry.Value != 0 && changeCount > 0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								landNums [entry.Key]--;
							}
						}
					}
				} 
				else if (x == 5 && y > 1 && y < 7 && y != 4) 
				{
					if (resourceLandTiles [resourceTypeNum] > 0) 
					{
						refTile.Resource = resourceType [resourceTypeNum];
						resourceLandTiles [resourceTypeNum]--;
					} 
					else 
					{
						int changeCount = 1;
						Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
						foreach (KeyValuePair<int, int> entry in resourceLandTiles) 
						{
							if (entry.Value != 0 && changeCount > 0) 
							{
								refTile.Resource = resourceType [entry.Key];
								keysToNuke.Add(entry.Key, entry.Value);
								changeCount--;
							}
						}
						foreach (KeyValuePair <int, int> entry in keysToNuke)
						{
							resourceLandTiles [entry.Key]--;
						}
					}

					if (y % 2 == 1) 
					{
						if (landNums [randomNumTile1] > 0) 
						{
							refTile.SelectedNum = randomNumTile1;
							landNums [randomNumTile1]--;
						} 
						else if (landNums [randomNumTile2] > 0) 
						{
							refTile.SelectedNum = randomNumTile2;
							landNums [randomNumTile2]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in landNums) 
							{
								if (entry.Value != 0 && changeCount > 0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								landNums [entry.Key]--;
							}
						}
					} 
					else 
					{
						if (landNums [randomNumTile2] > 0) 
						{
							refTile.SelectedNum = randomNumTile2;
							landNums [randomNumTile2]--;
						} 
						else if (landNums [randomNumTile1] > 0) 
						{
							refTile.SelectedNum = randomNumTile1;
							landNums [randomNumTile1]--;
						}
<<<<<<< HEAD
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in landNums) 
							{
								if (entry.Value != 0 && changeCount > 0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								landNums [entry.Key]--;
							}
						}
					}
				} 
				else if (x == 5 && y == 4) 
				{
					refTile.Resource = StealableType.Resource_Fish;
					refTile.IsLakeTile = true;
					refTile.SelectedNum2 = Random.Range (1, 4);
					refTile.SelectedNum3 = Random.Range (4, 7);
					refTile.SelectedNum4 = Random.Range (8, 11);
					refTile.SelectedNum5 = Random.Range (11, 13);
				} 
				else if (x == 6 && y > 1 && y < 7) 
				{
					if (resourceLandTiles [resourceTypeNum] > 0) 
					{
						refTile.Resource = resourceType [resourceTypeNum];
						resourceLandTiles [resourceTypeNum]--;
					} 
					else 
					{
						int changeCount = 1;
						Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
						foreach (KeyValuePair<int, int> entry in resourceLandTiles) 
						{
							if (entry.Value != 0 && changeCount > 0) 
							{
								refTile.Resource = resourceType [entry.Key];
								keysToNuke.Add(entry.Key, entry.Value);
								changeCount--;
							}
						}
						foreach (KeyValuePair <int, int> entry in keysToNuke)
						{
							resourceLandTiles [entry.Key]--;
						}
					}

					if (y % 2 == 1) 
					{
						if (landNums [randomNumTile1] > 0) 
						{
							refTile.SelectedNum = randomNumTile1;
							landNums [randomNumTile1]--;
						} 
						else if (landNums [randomNumTile2] > 0) 
						{
							refTile.SelectedNum = randomNumTile2;
							landNums [randomNumTile2]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in landNums) 
							{
								if (entry.Value != 0 && changeCount > 0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								landNums [entry.Key]--;
							}
						}
					} 
					else 
					{
						if (landNums [randomNumTile2] > 0) 
						{
							refTile.SelectedNum = randomNumTile2;
							landNums [randomNumTile2]--;
						} 
						else if (landNums [randomNumTile1] > 0) 
						{
							refTile.SelectedNum = randomNumTile1;
							landNums [randomNumTile1]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in landNums) 
							{
								if (entry.Value != 0 && changeCount > 0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								landNums [entry.Key]--;
							}
						}
					}
				} 
				else if (x == 6 && y > 3 && y < 7) 
				{
					if (resourceLandTiles [resourceTypeNum] > 0) 
					{
						refTile.Resource = resourceType [resourceTypeNum];
						resourceLandTiles [resourceTypeNum]--;
					} 
					else 
					{
						int changeCount = 1;
						Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
						foreach (KeyValuePair<int, int> entry in resourceLandTiles) 
						{
							if (entry.Value != 0 && changeCount > 0) 
							{
								refTile.Resource = resourceType [entry.Key];
								keysToNuke.Add(entry.Key, entry.Value);
								changeCount--;
							}
						}
						foreach (KeyValuePair <int, int> entry in keysToNuke)
						{
							resourceLandTiles [entry.Key]--;
						}
					}

					if (y % 2 == 1) 
					{
						if (landNums [randomNumTile1] > 0) 
						{
							refTile.SelectedNum = randomNumTile1;
							landNums [randomNumTile1]--;
						} 
						else if (landNums [randomNumTile2] > 0) 
						{
							refTile.SelectedNum = randomNumTile2;
							landNums [randomNumTile2]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in landNums) 
							{
								if (entry.Value != 0 && changeCount > 0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								landNums [entry.Key]--;
							}
						}
					} 
					else 
					{
						if (landNums [randomNumTile2] > 0) 
						{
							refTile.SelectedNum = randomNumTile2;
							landNums [randomNumTile2]--;
						} 
						else if (landNums [randomNumTile1] > 0) 
						{
							refTile.SelectedNum = randomNumTile1;
							landNums [randomNumTile1]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in landNums) 
							{
								if (entry.Value != 0 && changeCount > 0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								landNums [entry.Key]--;
							}
						}
					}
				} 
				else if (x == 7 && y > 3 && y < 5) 
				{
					if (resourceLandTiles [resourceTypeNum] > 0) 
					{
						refTile.Resource = resourceType [resourceTypeNum];
						resourceLandTiles [resourceTypeNum]--;
					} 
					else 
					{
						int changeCount = 1;
						Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
						foreach (KeyValuePair<int, int> entry in resourceLandTiles) 
						{
							if (entry.Value != 0 && changeCount > 0) 
							{
								refTile.Resource = resourceType [entry.Key];
								keysToNuke.Add(entry.Key, entry.Value);
								changeCount--;
							}
						}
						foreach (KeyValuePair <int, int> entry in keysToNuke)
						{
							resourceLandTiles [entry.Key]--;
						}
					}

					if (y % 2 == 1) 
					{
						if (landNums [randomNumTile1] > 0) 
						{
							refTile.SelectedNum = randomNumTile1;
							landNums [randomNumTile1]--;
						} 
						else if (landNums [randomNumTile2] > 0) 
						{
							refTile.SelectedNum = randomNumTile2;
							landNums [randomNumTile2]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in landNums) 
							{
								if (entry.Value != 0 && changeCount > 0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								landNums [entry.Key]--;
							}
						}
					} 
					else 
					{
						if (landNums [randomNumTile2] > 0) 
						{
							refTile.SelectedNum = randomNumTile2;
							landNums [randomNumTile2]--;
						} 
						else if (landNums [randomNumTile1] > 0) 
						{
							refTile.SelectedNum = randomNumTile1;
							landNums [randomNumTile1]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in landNums) 
							{
								if (entry.Value != 0 && changeCount > 0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								landNums [entry.Key]--;
							}
						}
					}
				} 


				// Islands
				else if (x == 9 && y == 6) 
				{

					if (resourceSeaTiles [resourceTypeNum] > 0) 
					{
						refTile.Resource = resourceType [resourceTypeNum];
						resourceSeaTiles [resourceTypeNum]--;
					} 
					else 
					{
						int changeCount = 1;
						Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
						foreach (KeyValuePair<int, int> entry in resourceSeaTiles) 
						{
							if (entry.Value != 0 && changeCount > 0) 
							{
								refTile.Resource = resourceType [entry.Key];
								keysToNuke.Add(entry.Key, entry.Value);
								changeCount--;
							}
						}
						foreach (KeyValuePair <int, int> entry in keysToNuke)
						{
							resourceSeaTiles [entry.Key]--;
						}
					}

					if (y % 2 == 1) 
					{
						if (seaNums [randomNumSeaTile1] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile1;
							seaNums [randomNumSeaTile1]--;
						} 
						else if (seaNums [randomNumSeaTile2] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile2;
							seaNums [randomNumSeaTile2]--;
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in seaNums) 
							{
								if (entry.Value != 0 && changeCount>0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								seaNums [entry.Key]--;
							}

						}
					} 
					else 
					{
						if (seaNums [randomNumSeaTile2] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile2;
							seaNums [randomNumSeaTile2]--;
						} 
						else if (seaNums [randomNumSeaTile1] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile1;
							seaNums [randomNumSeaTile1]--;
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in seaNums) 
							{
								if (entry.Value != 0 && changeCount>0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								seaNums [entry.Key]--;
							}

						}
					}
				} 
				else if (x == 8 && y == 7) 
				{

					if (resourceSeaTiles [resourceTypeNum] > 0) 
					{
						refTile.Resource = resourceType [resourceTypeNum];
						resourceSeaTiles [resourceTypeNum]--;
					} 
					else 
					{
						int changeCount = 1;
						Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
						foreach (KeyValuePair<int, int> entry in resourceSeaTiles) 
						{
							if (entry.Value != 0 && changeCount > 0) 
							{
								refTile.Resource = resourceType [entry.Key];
								keysToNuke.Add(entry.Key, entry.Value);
								changeCount--;
							}
						}
						foreach (KeyValuePair <int, int> entry in keysToNuke)
						{
							resourceSeaTiles [entry.Key]--;
						}
					}

					if (y % 2 == 1) 
					{
						if (seaNums [randomNumSeaTile1] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile1;
							seaNums [randomNumSeaTile1]--;
						} 
						else if (seaNums [randomNumSeaTile2] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile2;
							seaNums [randomNumSeaTile2]--;
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in seaNums) 
							{
								if (entry.Value != 0 && changeCount>0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								seaNums [entry.Key]--;
							}

						}
					} 
					else 
					{
						if (seaNums [randomNumSeaTile2] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile2;
							seaNums [randomNumSeaTile2]--;
						} 
						else if (seaNums [randomNumSeaTile1] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile1;
							seaNums [randomNumSeaTile1]--;
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in seaNums) 
							{
								if (entry.Value != 0 && changeCount>0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								seaNums [entry.Key]--;
							}

						}
					}
				} 
				else if (x == 8 && y == 8) 
				{

					if (resourceSeaTiles [resourceTypeNum] > 0) 
					{
						refTile.Resource = resourceType [resourceTypeNum];
						resourceSeaTiles [resourceTypeNum]--;
					} 
					else 
					{
						int changeCount = 1;
						Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
						foreach (KeyValuePair<int, int> entry in resourceSeaTiles) 
						{
							if (entry.Value != 0 && changeCount > 0) 
							{
								refTile.Resource = resourceType [entry.Key];
								keysToNuke.Add(entry.Key, entry.Value);
								changeCount--;
							}
						}
						foreach (KeyValuePair <int, int> entry in keysToNuke)
						{
							resourceSeaTiles [entry.Key]--;
						}
					}

					if (y % 2 == 1) 
					{
						if (seaNums [randomNumSeaTile1] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile1;
							seaNums [randomNumSeaTile1]--;
						} 
						else if (seaNums [randomNumSeaTile2] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile2;
							seaNums [randomNumSeaTile2]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in seaNums) 
							{
								if (entry.Value != 0 && changeCount>0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								seaNums [entry.Key]--;
							}

						}
					} 
					else 
					{
						if (seaNums [randomNumSeaTile2] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile2;
							seaNums [randomNumSeaTile2]--;
						} 
						else if (seaNums [randomNumSeaTile1] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile1;
<<<<<<< HEAD
							seaNums [randomNumSeaTile1]--;
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in seaNums) 
							{
								if (entry.Value != 0 && changeCount>0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
							landNums [randomNumSeaTile1]--;
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								seaNums [entry.Key]--;
							}

						}
					}
				} 
				else if (x == 9 && y == 7) 
				{

					refTile.Resource = resourceType [6];

					if (y % 2 == 1) 
					{
						if (seaNums [randomNumSeaTile1] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile1;
							seaNums [randomNumSeaTile1]--;
						} 
						else if (seaNums [randomNumSeaTile2] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile2;
							seaNums [randomNumSeaTile2]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in seaNums) 
							{
								if (entry.Value != 0 && changeCount>0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								seaNums [entry.Key]--;
							}

						}
					} 
					else 
					{
						if (seaNums [randomNumSeaTile2] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile2;
							seaNums [randomNumSeaTile2]--;
						} 
						else if (seaNums [randomNumSeaTile1] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile1;
<<<<<<< HEAD
							seaNums [randomNumSeaTile1]--;
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in seaNums) 
							{
								if (entry.Value != 0 && changeCount>0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
							landNums [randomNumSeaTile1]--;
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								seaNums [entry.Key]--;
							}

						}
					}
				} 
				else if (x == 9 && y == 5) 
				{

					refTile.Resource = resourceType [6];

					if (y % 2 == 1) 
					{
						if (seaNums [randomNumSeaTile1] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile1;
							seaNums [randomNumSeaTile1]--;
						} 
						else if (seaNums [randomNumSeaTile2] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile2;
							seaNums [randomNumSeaTile2]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in seaNums) 
							{
								if (entry.Value != 0 && changeCount>0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								seaNums [entry.Key]--;
							}

						}
					} 
					else 
					{
						if (seaNums [randomNumSeaTile2] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile2;
							seaNums [randomNumSeaTile2]--;
						} 
						else if (seaNums [randomNumSeaTile1] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile1;
<<<<<<< HEAD
							seaNums [randomNumSeaTile1]--;
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in seaNums) 
							{
								if (entry.Value != 0 && changeCount>0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
							landNums [randomNumSeaTile1]--;
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								seaNums [entry.Key]--;
							}

						}
					}
				} 
				else if (x == 9 && y == 2) 
				{

					if (resourceSeaTiles [resourceTypeNum] > 0) 
					{
						refTile.Resource = resourceType [resourceTypeNum];
						resourceSeaTiles [resourceTypeNum]--;
					} 
					else 
					{
						int changeCount = 1;
						Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
						foreach (KeyValuePair<int, int> entry in resourceSeaTiles) 
						{
							if (entry.Value != 0 && changeCount > 0) 
							{
								refTile.Resource = resourceType [entry.Key];
								keysToNuke.Add(entry.Key, entry.Value);
								changeCount--;
							}
						}
						foreach (KeyValuePair <int, int> entry in keysToNuke)
						{
							resourceSeaTiles [entry.Key]--;
						}
					}

					if (y % 2 == 1) 
					{
						if (seaNums [randomNumSeaTile1] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile1;
							seaNums [randomNumSeaTile1]--;
						} 
						else if (seaNums [randomNumSeaTile2] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile2;
							seaNums [randomNumSeaTile2]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in seaNums) 
							{
								if (entry.Value != 0 && changeCount>0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								seaNums [entry.Key]--;
							}

						}
					} 
					else 
					{
						if (seaNums [randomNumSeaTile2] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile2;
							seaNums [randomNumSeaTile2]--;
						} 
						else if (seaNums [randomNumSeaTile1] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile1;
<<<<<<< HEAD
							seaNums [randomNumSeaTile1]--;
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in seaNums) 
							{
								if (entry.Value != 0 && changeCount>0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
							landNums [randomNumSeaTile1]--;
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								seaNums [entry.Key]--;
							}

						}
					}
				} 
				else if (x == 8 && y == 1) 
				{

					if (resourceSeaTiles [resourceTypeNum] > 0) 
					{
						refTile.Resource = resourceType [resourceTypeNum];
						resourceSeaTiles [resourceTypeNum]--;
					} 
					else 
					{
						int changeCount = 1;
						Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
						foreach (KeyValuePair<int, int> entry in resourceSeaTiles) 
						{
							if (entry.Value != 0 && changeCount > 0) 
							{
								refTile.Resource = resourceType [entry.Key];
								keysToNuke.Add(entry.Key, entry.Value);
								changeCount--;
							}
						}
						foreach (KeyValuePair <int, int> entry in keysToNuke)
						{
							resourceSeaTiles [entry.Key]--;
						}
					}

					if (y % 2 == 1) 
					{
						if (seaNums [randomNumSeaTile1] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile1;
							seaNums [randomNumSeaTile1]--;
						} 
						else if (seaNums [randomNumSeaTile2] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile2;
							seaNums [randomNumSeaTile2]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in seaNums) 
							{
								if (entry.Value != 0 && changeCount>0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								seaNums [entry.Key]--;
							}

						}
					} 
					else 
					{
						if (seaNums [randomNumSeaTile2] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile2;
							seaNums [randomNumSeaTile2]--;
						} 
						else if (seaNums [randomNumSeaTile1] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile1;
<<<<<<< HEAD
							seaNums [randomNumSeaTile1]--;
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in seaNums) 
							{
								if (entry.Value != 0 && changeCount>0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
							landNums [randomNumSeaTile1]--;
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								seaNums [entry.Key]--;
							}

						}
					}
				} 
				else if (x < 4 && x > 1 && y == 8) 
				{

					if (resourceSeaTiles [resourceTypeNum] > 0) 
					{
						refTile.Resource = resourceType [resourceTypeNum];
						resourceSeaTiles [resourceTypeNum]--;
					} 
					else 
					{
						int changeCount = 1;
						Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
						foreach (KeyValuePair<int, int> entry in resourceSeaTiles) 
						{
							if (entry.Value != 0 && changeCount > 0) 
							{
								refTile.Resource = resourceType [entry.Key];
								keysToNuke.Add(entry.Key, entry.Value);
								changeCount--;
							}
						}
						foreach (KeyValuePair <int, int> entry in keysToNuke)
						{
							resourceSeaTiles [entry.Key]--;
						}
					}

					if (y % 2 == 1) 
					{
						if (seaNums [randomNumSeaTile1] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile1;
							seaNums [randomNumSeaTile1]--;
						} 
						else if (seaNums [randomNumSeaTile2] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile2;
							seaNums [randomNumSeaTile2]--;
<<<<<<< HEAD
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in seaNums) 
							{
								if (entry.Value != 0 && changeCount>0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								seaNums [entry.Key]--;
							}

						}
					} 
					else 
					{
						if (seaNums [randomNumSeaTile2] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile2;
							seaNums [randomNumSeaTile2]--;
						} 
						else if (seaNums [randomNumSeaTile1] > 0) 
						{
							refTile.SelectedNum = randomNumSeaTile1;
<<<<<<< HEAD
							seaNums [randomNumSeaTile1]--;
						} 
						else 
						{
							int changeCount = 1;
							Dictionary <int, int> keysToNuke = new Dictionary<int, int> ();
							foreach (KeyValuePair<int, int> entry in seaNums) 
							{
								if (entry.Value != 0 && changeCount>0) 
								{
									refTile.SelectedNum = entry.Key;
									keysToNuke.Add(entry.Key, entry.Value);
									changeCount--;
=======
							landNums [randomNumSeaTile1]--;
						}
						else {
							foreach (int key in landNums.Keys) {
								if (landNums[key] != 0) {
									refTile.SelectedNum = key;
									landNums [key]--;
									continue;
>>>>>>> origin/slurp
								}
							}
							foreach (KeyValuePair <int, int> entry in keysToNuke)
							{
								seaNums [entry.Key]--;
							}

						}
					}
				}

				//fishing tiles
				else if (x == 3 && y == 2) 
				{
					refTile.SelectedNum = Random.Range (1, 7);
					//refTile.IsFishingGround = true;
					refTile.Resource = StealableType.Resource_Fish;
				} 
				else if (x == 4 && y == 1) 
				{
					refTile.SelectedNum = Random.Range (8, 13);
					//refTile.IsFishingGround = true;
					refTile.Resource = StealableType.Resource_Fish;
				} 
				else if (x == 7 && y == 3) 
				{
					refTile.SelectedNum = Random.Range (1, 7);
					//refTile.IsFishingGround = true;
					refTile.Resource = StealableType.Resource_Fish;
				} 
				else if (x == 7 && y == 5) 
				{
					refTile.SelectedNum = Random.Range (8, 13);
					//refTile.IsFishingGround = true;
					refTile.Resource = StealableType.Resource_Fish;
				} 
				else if (x == 4 && y == 7) 
				{
					refTile.SelectedNum = Random.Range (1, 7);
					//refTile.IsFishingGround = true;
					refTile.Resource = StealableType.Resource_Fish;
				} 
				else if (x == 3 && y == 6) 
				{
					refTile.SelectedNum = Random.Range (8, 13);
					//refTile.IsFishingGround = true;
					refTile.Resource = StealableType.Resource_Fish;
				}

				// else is water
				else {
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
		
		harbours = new Dictionary<GameObject, StealableType> ();
		harbourCollection = new Dictionary<string, GameObject>();


		//harbour 1
		//retrieve top left tile
		Vector2 harbour1Vec2 = new Vector2(3, 1);
		Vec3 harbour1CubePos1 = offsetOddRToCubeCoordinate (harbour1Vec2);
		GameObject currentHexGameObjForHarbour1 = cubeHexes [harbour1CubePos1];

		//retrieve correct intersections
		GameObject h1i1 = currentHexGameObjForHarbour1.transform.FindChild("RBIntersection").gameObject;
		GameObject h1i2 = currentHexGameObjForHarbour1.transform.FindChild("BIntersection").gameObject;

		h1i1.GetComponent<UIIntersection> ().canAccessHarbour = true;
		h1i2.GetComponent<UIIntersection> ().canAccessHarbour = true;

		//vec3 of mainland gameobject
		Vec3 harbour1CubePos2 = offsetOddRToCubeCoordinate (new Vector2(4,2));

		//retrive the edge that the harbour occupies (don't know if this is necessary yet)
		Edge harbour1 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(harbour1CubePos1, harbour1CubePos2);
		harbour1.isHarbour = true;

		//retrieve the correct edge gameobject (will use for location of harbour sprite)
		GameObject firstHarbour = currentHexGameObjForHarbour1.transform.FindChild ("RBEdge").gameObject;

		//add exchangeable resource to harbour 
		harbours.Add (firstHarbour, StealableType.Resource_Brick);

		//instantiate harbour
		GameObject newHarbour1 = (GameObject) Instantiate (Harbour);
		newHarbour1.transform.parent = this.transform;
		newHarbour1.transform.position = firstHarbour.transform.position;

		//change harbour text
		TextMesh harbour1Text = newHarbour1.GetComponentInChildren<TextMesh>();
		harbour1Text.text = "1";
		harbourCollection.Add("1", newHarbour1);

		//set values in harbour
		Harbour harbourScript1 = newHarbour1.GetComponent<Harbour> ();
		harbourScript1.exchangeRate = 3;
		harbourScript1.returnedResource = StealableType.Resource_Brick;
		harbourScript1.returnedAmount = 1;



		//harbour 2
		//retrieve top left tile
		Vector2 harbour2Vec2 = new Vector2(5, 1);
		Vec3 harbour2CubePos1 = offsetOddRToCubeCoordinate (harbour2Vec2);
		GameObject currentHexGameObjForHarbour2 = cubeHexes [harbour2CubePos1];

		Vector2 otherHarbour2Vec2 = new Vector2(4, 1);
		Vec3 otherHarbour2CubePos1 = offsetOddRToCubeCoordinate (otherHarbour2Vec2);
		GameObject currentHexGameObj2ForHarbour2 = cubeHexes [otherHarbour2CubePos1];

		//retrieve correct intersections
		GameObject h2i1 = currentHexGameObj2ForHarbour2.transform.FindChild("BIntersection").gameObject;
		GameObject h2i2 = currentHexGameObjForHarbour2.transform.FindChild("BIntersection").gameObject;

		h2i1.GetComponent<UIIntersection> ().canAccessHarbour = true;
		h2i2.GetComponent<UIIntersection> ().canAccessHarbour = true;

		//vec3 of mainland gameobject
		Vec3 harbour2CubePos2 = offsetOddRToCubeCoordinate (new Vector2(5,2));

		//retrive the edge that the harbour occupies (don't know if this is necessary yet)
		Edge harbour2 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(harbour2CubePos2, harbour2CubePos1);
		harbour2.isHarbour = true;

		//retrieve the correct edge gameobject (will use for location of harbour sprite)
		GameObject secondHarbour = currentHexGameObjForHarbour2.transform.FindChild ("LBEdge").gameObject;

		//add exchangeable resource to harbour 
		harbours.Add (secondHarbour, StealableType.Resource_Grain);

		//instantiate harbour
		GameObject newHarbour2 = (GameObject) Instantiate (Harbour);
		newHarbour2.transform.parent = this.transform;
		newHarbour2.transform.position = secondHarbour.transform.position;

		//change harbour text
		TextMesh harbour2Text = newHarbour2.GetComponentInChildren<TextMesh>();
		harbour2Text.text = "2";
		harbourCollection.Add("2", newHarbour2);

		//set values in harbour
		Harbour harbourScript2 = newHarbour2.GetComponent<Harbour> ();
		harbourScript2.exchangeRate = 3;
		harbourScript2.returnedResource = StealableType.Resource_Grain;
		harbourScript2.returnedAmount = 1;



		//harbour 3
		//retrieve top left tile
		Vector2 harbour3Vec2 = new Vector2(7, 2);
		Vec3 harbour3CubePos1 = offsetOddRToCubeCoordinate (harbour3Vec2);
		GameObject currentHexGameObjForHarbour3 = cubeHexes [harbour3CubePos1];

		Vector2 otherHarbour3Vec2 = new Vector2(6, 2);
		Vec3 otherHarbour3CubePos1 = offsetOddRToCubeCoordinate (otherHarbour3Vec2);
		GameObject currentHexGameObj2ForHarbour3 = cubeHexes [otherHarbour3CubePos1];

		//retrieve correct intersections
		GameObject h3i1 = currentHexGameObj2ForHarbour3.transform.FindChild("RBIntersection").gameObject;
		GameObject h3i2 = currentHexGameObjForHarbour3.transform.FindChild("BIntersection").gameObject;

		h3i1.GetComponent<UIIntersection> ().canAccessHarbour = true;
		h3i2.GetComponent<UIIntersection> ().canAccessHarbour = true;

		//vec3 of mainland gameobject
		Vec3 harbour3CubePos2 = offsetOddRToCubeCoordinate (new Vector2(6,3));

		//retrive the edge that the harbour occupies (don't know if this is necessary yet)
		Edge harbour3 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(harbour3CubePos1, harbour3CubePos2);
		harbour3.isHarbour = true;

		//retrieve the correct edge gameobject (will use for location of harbour sprite)
		GameObject thirdHarbour = currentHexGameObjForHarbour3.transform.FindChild ("LBEdge").gameObject;

		//add exchangeable resource to harbour 
		harbours.Add (thirdHarbour, StealableType.Resource_Lumber);

		//instantiate harbour
		GameObject newHarbour3 = (GameObject) Instantiate (Harbour);
		newHarbour3.transform.parent = this.transform;
		newHarbour3.transform.position = thirdHarbour.transform.position;

		//change harbour text
		TextMesh harbour3Text = newHarbour3.GetComponentInChildren<TextMesh>();
		harbour3Text.text = "3";
		harbourCollection.Add("3", newHarbour3);

		//set values in harbour
		Harbour harbourScript3 = newHarbour3.GetComponent<Harbour> ();
		harbourScript3.exchangeRate = 3;
		harbourScript3.returnedResource = StealableType.Resource_Lumber;
		harbourScript3.returnedAmount = 1;


		//harbour 4
		//retrieve top left tile
		Vector2 harbour4Vec2 = new Vector2(7, 4);
		Vec3 harbour4CubePos1 = offsetOddRToCubeCoordinate (harbour4Vec2);
		GameObject currentHexGameObjForHarbour4 = cubeHexes [harbour4CubePos1];

		Vector2 otherHarbour4Vec2 = new Vector2(7, 3);
		Vec3 otherHarbour4CubePos1 = offsetOddRToCubeCoordinate (otherHarbour4Vec2);
		GameObject currentHexGameObj2ForHarbour4 = cubeHexes [otherHarbour4CubePos1];

		//retrieve correct intersections
		GameObject h4i1 = currentHexGameObjForHarbour4.transform.FindChild("RBIntersection").gameObject;
		GameObject h4i2 = currentHexGameObj2ForHarbour4.transform.FindChild("BIntersection").gameObject;

		h4i1.GetComponent<UIIntersection> ().canAccessHarbour = true;
		h4i2.GetComponent<UIIntersection> ().canAccessHarbour = true;

		//vec3 of mainland gameobject
		Vec3 harbour4CubePos2 = offsetOddRToCubeCoordinate (new Vector2(8,4));

		//retrive the edge that the harbour occupies (don't know if this is necessary yet)
		Edge harbour4 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(harbour4CubePos1, harbour4CubePos2);
		harbour4.isHarbour = true;

		//retrieve the correct edge gameobject (will use for location of harbour sprite)
		GameObject fourthHarbour = currentHexGameObjForHarbour4.transform.FindChild ("REdge").gameObject;

		//add exchangeable resource to harbour 
		harbours.Add (fourthHarbour, StealableType.Resource_Ore);

		//instantiate harbour
		GameObject newHarbour4 = (GameObject) Instantiate (Harbour);
		newHarbour4.transform.parent = this.transform;
		newHarbour4.transform.position = fourthHarbour.transform.position;

		//change harbour text
		TextMesh harbour4Text = newHarbour4.GetComponentInChildren<TextMesh>();
		harbour4Text.text = "4";
		harbourCollection.Add("4", newHarbour4);

		//set values in harbour
		Harbour harbourScript4 = newHarbour4.GetComponent<Harbour> ();
		harbourScript4.exchangeRate = 3;
		harbourScript4.returnedResource = StealableType.Resource_Ore;
		harbourScript4.returnedAmount = 1;


		//harbour 5
		//retrieve top left tile
		Vector2 harbour5Vec2 = new Vector2(6, 5);
		Vec3 harbour5CubePos1 = offsetOddRToCubeCoordinate (harbour5Vec2);
		GameObject currentHexGameObjForHarbour5 = cubeHexes [harbour5CubePos1];

		//retrieve correct intersections
		GameObject h5i1 = currentHexGameObjForHarbour5.transform.FindChild("RBIntersection").gameObject;
		GameObject h5i2 = currentHexGameObjForHarbour5.transform.FindChild("BIntersection").gameObject;

		h5i1.GetComponent<UIIntersection> ().canAccessHarbour = true;
		h5i2.GetComponent<UIIntersection> ().canAccessHarbour = true;

		//vec3 of mainland gameobject
		Vec3 harbour5CubePos2 = offsetOddRToCubeCoordinate (new Vector2(7,6));

		//retrive the edge that the harbour occupies (don't know if this is necessary yet)
		Edge harbour5 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(harbour5CubePos1, harbour5CubePos2);
		harbour5.isHarbour = true;

		//retrieve the correct edge gameobject (will use for location of harbour sprite)
		GameObject fifthHarbour = currentHexGameObjForHarbour5.transform.FindChild ("RBEdge").gameObject;

		//add exchangeable resource to harbour 
		harbours.Add (fifthHarbour, StealableType.Resource_Wool);

		//instantiate harbour
		GameObject newHarbour5 = (GameObject) Instantiate (Harbour);
		newHarbour5.transform.parent = this.transform;
		newHarbour5.transform.position = fifthHarbour.transform.position;

		//change harbour text
		TextMesh harbour5Text = newHarbour5.GetComponentInChildren<TextMesh>();
		harbour5Text.text = "5";
		harbourCollection.Add("5", newHarbour5);

		//set values in harbour
		Harbour harbourScript5 = newHarbour5.GetComponent<Harbour> ();
		harbourScript5.exchangeRate = 3;
		harbourScript5.returnedResource = StealableType.Resource_Wool;
		harbourScript5.returnedAmount = 1;



		//harbour 6
		//retrieve tile
		Vector2 harbour6Vec2 = new Vector2(5, 6);
		Vec3 harbour6CubePos1 = offsetOddRToCubeCoordinate (harbour6Vec2);
		GameObject currentHexGameObjForHarbour6 = cubeHexes [harbour6CubePos1];

		//retrieve correct intersections
		GameObject h6i1 = currentHexGameObjForHarbour6.transform.FindChild("RBIntersection").gameObject;
		GameObject h6i2 = currentHexGameObjForHarbour6.transform.FindChild("BIntersection").gameObject;

		h6i1.GetComponent<UIIntersection> ().canAccessHarbour = true;
		h6i2.GetComponent<UIIntersection> ().canAccessHarbour = true;

		//vec3 of mainland gameobject
		Vec3 harbour6CubePos2 = offsetOddRToCubeCoordinate (new Vector2(5,7));

		//retrive the edge that the harbour occupies (don't know if this is necessary yet)
		Edge harbour6 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(harbour6CubePos1, harbour6CubePos2);
		harbour6.isHarbour = true;

		//retrieve the correct edge gameobject (will use for location of harbour sprite)
		GameObject sixthHarbour = currentHexGameObjForHarbour6.transform.FindChild ("RBEdge").gameObject;

		//add exchangeable resource to harbour 
		harbours.Add (sixthHarbour, StealableType.Resource_Brick);

		//instantiate harbour
		GameObject newHarbour6 = (GameObject) Instantiate (Harbour);
		newHarbour6.transform.parent = this.transform;
		newHarbour6.transform.position = sixthHarbour.transform.position;

		//change harbour text
		TextMesh harbour6Text = newHarbour6.GetComponentInChildren<TextMesh>();
		harbour6Text.text = "6";
		harbourCollection.Add("6", newHarbour6);

		//set values in harbour
		Harbour harbourScript6 = newHarbour6.GetComponent<Harbour> ();
		harbourScript6.exchangeRate = 3;
		harbourScript6.returnedResource = StealableType.Resource_Brick;
		harbourScript6.returnedAmount = 1;



		//harbour 7
		//retrieve tile
		Vector2 harbour7Vec2 = new Vector2(4, 6);
		Vec3 harbour7CubePos1 = offsetOddRToCubeCoordinate (harbour7Vec2);
		GameObject currentHexGameObjForHarbour7 = cubeHexes [harbour7CubePos1];

		Vector2 otherHarbour7Vec2 = new Vector2(3, 6);
		Vec3 otherHarbour7CubePos1 = offsetOddRToCubeCoordinate (otherHarbour7Vec2);
		GameObject currentHexGameObj2ForHarbour7 = cubeHexes [otherHarbour7CubePos1];

		//retrieve correct intersections
		GameObject h7i1 = currentHexGameObj2ForHarbour7.transform.FindChild("RBIntersection").gameObject;
		GameObject h7i2 = currentHexGameObjForHarbour7.transform.FindChild("BIntersection").gameObject;

		h7i1.GetComponent<UIIntersection> ().canAccessHarbour = true;
		h7i2.GetComponent<UIIntersection> ().canAccessHarbour = true;

		//vec3 of mainland gameobject
		Vec3 harbour7CubePos2 = offsetOddRToCubeCoordinate (new Vector2(3,7));

		//retrive the edge that the harbour occupies (don't know if this is necessary yet)
		Edge harbour7 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(harbour7CubePos1, harbour7CubePos2);
		harbour7.isHarbour = true;

		//retrieve the correct edge gameobject (will use for location of harbour sprite)
		GameObject seventhHarbour = currentHexGameObjForHarbour7.transform.FindChild ("LBEdge").gameObject;

		//add exchangeable resource to harbour 
		harbours.Add (seventhHarbour, StealableType.Resource_Grain);

		//instantiate harbour
		GameObject newHarbour7 = (GameObject) Instantiate (Harbour);
		newHarbour7.transform.parent = this.transform;
		newHarbour7.transform.position = seventhHarbour.transform.position;

		//change harbour text
		TextMesh harbour7Text = newHarbour7.GetComponentInChildren<TextMesh>();
		harbour7Text.text = "7";
		harbourCollection.Add("7", newHarbour7);

		//set values in harbour
		Harbour harbourScript7 = newHarbour7.GetComponent<Harbour> ();
		harbourScript7.exchangeRate = 3;
		harbourScript7.returnedResource = StealableType.Resource_Grain;
		harbourScript7.returnedAmount = 1;


		//harbour 8
		//retrieve tile
		Vector2 harbour8Vec2 = new Vector2(2, 5);
		Vec3 harbour8CubePos1 = offsetOddRToCubeCoordinate (harbour8Vec2);
		GameObject currentHexGameObjForHarbour8 = cubeHexes [harbour8CubePos1];

		Vector2 otherHarbour8Vec2 = new Vector2(3, 4);
		Vec3 otherHarbour8CubePos1 = offsetOddRToCubeCoordinate (otherHarbour8Vec2);
		GameObject currentHexGameObj2ForHarbour8 = cubeHexes [otherHarbour8CubePos1];

		//retrieve correct intersections
		GameObject h8i1 = currentHexGameObjForHarbour8.transform.FindChild("RBIntersection").gameObject;
		GameObject h8i2 = currentHexGameObj2ForHarbour8.transform.FindChild("BIntersection").gameObject;

		h8i1.GetComponent<UIIntersection> ().canAccessHarbour = true;
		h8i2.GetComponent<UIIntersection> ().canAccessHarbour = true;

		//vec3 of mainland gameobject
		Vec3 harbour8CubePos2 = offsetOddRToCubeCoordinate (new Vector2(3,5));

		//retrive the edge that the harbour occupies (don't know if this is necessary yet)
		Edge harbour8 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(harbour8CubePos1, harbour8CubePos2);
		harbour8.isHarbour = true;

		//retrieve the correct edge gameobject (will use for location of harbour sprite)
		GameObject eigthHarbour = currentHexGameObjForHarbour8.transform.FindChild ("REdge").gameObject;

		//add exchangeable resource to harbour 
		harbours.Add (eigthHarbour, StealableType.Resource_Lumber);

		//instantiate harbour
		GameObject newHarbour8 = (GameObject) Instantiate (Harbour);
		newHarbour8.transform.parent = this.transform;
		newHarbour8.transform.position = eigthHarbour.transform.position;

		//change harbour text
		TextMesh harbour8Text = newHarbour8.GetComponentInChildren<TextMesh>();
		harbour8Text.text = "8";
		harbourCollection.Add("8", newHarbour8);

		//set values in harbour
		Harbour harbourScript8 = newHarbour8.GetComponent<Harbour> ();
		harbourScript8.exchangeRate = 3;
		harbourScript8.returnedResource = StealableType.Resource_Lumber;
		harbourScript8.returnedAmount = 1;


		//harbour 9
		//retrieve tile
		Vector2 harbour9Vec2 = new Vector2(2, 3);
		Vec3 harbour9CubePos1 = offsetOddRToCubeCoordinate (harbour9Vec2);
		GameObject currentHexGameObjForHarbour9 = cubeHexes [harbour9CubePos1];

		Vector2 otherHarbour9Vec2 = new Vector2(3, 2);
		Vec3 otherHarbour9CubePos1 = offsetOddRToCubeCoordinate (otherHarbour9Vec2);
		GameObject currentHexGameObj2ForHarbour9 = cubeHexes [otherHarbour9CubePos1];

		//retrieve correct intersections
		GameObject h9i1 = currentHexGameObjForHarbour9.transform.FindChild("RBIntersection").gameObject;
		GameObject h9i2 = currentHexGameObj2ForHarbour9.transform.FindChild("BIntersection").gameObject;

		h9i1.GetComponent<UIIntersection> ().canAccessHarbour = true;
		h9i2.GetComponent<UIIntersection> ().canAccessHarbour = true;

		//vec3 of mainland gameobject
		Vec3 harbour9CubePos2 = offsetOddRToCubeCoordinate (new Vector2(3,3));

		//retrive the edge that the harbour occupies (don't know if this is necessary yet)
		Edge harbour9 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(harbour9CubePos1, harbour9CubePos2);
		harbour9.isHarbour = true;

		//retrieve the correct edge gameobject (will use for location of harbour sprite)
		GameObject ninthHarbour = currentHexGameObjForHarbour9.transform.FindChild ("REdge").gameObject;

		//add exchangeable resource to harbour 
		harbours.Add (ninthHarbour, StealableType.Resource_Ore);

		//instantiate harbour
		GameObject newHarbour9 = (GameObject) Instantiate (Harbour);
		newHarbour9.transform.parent = this.transform;
		newHarbour9.transform.position = ninthHarbour.transform.position;

		//change harbour text
		TextMesh harbour9Text = newHarbour9.GetComponentInChildren<TextMesh>();
		harbour9Text.text = "9";
		harbourCollection.Add("9", newHarbour9);

		//set values in harbour
		Harbour harbourScript9 = newHarbour9.GetComponent<Harbour> ();
		harbourScript9.exchangeRate = 3;
		harbourScript9.returnedResource = StealableType.Resource_Ore;
		harbourScript9.returnedAmount = 1;


	}

	public void buildFishingGroundsUI () {

		/*
		Vec3 fg1 = offsetOddRToCubeCoordinate (new Vector2(3,2));
		GameObject fg1h = cubeHexes [fg1];
		GameObject RBEdgeFG1 = fg1h.transform.FindChild ("RBEdge").gameObject;
		GameObject REdgeFG1 = fg1h.transform.FindChild ("REdge").gameObject;


		Vec3 fg2 = offsetOddRToCubeCoordinate (new Vector2(4,1));
		GameObject fg2h = cubeHexes [fg2];
		GameObject LBEdgeFG2 = fg2h.transform.FindChild ("LBEdge").gameObject;
		GameObject RBEdgeFG2 = fg2h.transform.FindChild ("RBEdge").gameObject;


		Vec3 fg3 = offsetOddRToCubeCoordinate (new Vector2(7,3));
		GameObject fg3h = cubeHexes [fg3];
		GameObject LEdgeFG3 = fg3h.transform.FindChild ("LEdge").gameObject;
		GameObject LBEdgeFG3 = fg3h.transform.FindChild ("LBEdge").gameObject;


		Vec3 fg4 = offsetOddRToCubeCoordinate (new Vector2(7,5));
		GameObject fg4h = cubeHexes [fg4];
		GameObject LEdgeFG4 = fg4h.transform.FindChild ("LEdge").gameObject;
		GameObject LTEdgeFG4 = fg4h.transform.FindChild ("LTEdge").gameObject;


		Vec3 fg5 = offsetOddRToCubeCoordinate (new Vector2(4,7));
		GameObject fg5h = cubeHexes [fg5];
		GameObject LTEdgeFG5 = fg5h.transform.FindChild ("LTEdge").gameObject;
		GameObject RTEdgeFG5 = fg5h.transform.FindChild ("RTEdge").gameObject;


		Vec3 fg6 = offsetOddRToCubeCoordinate (new Vector2(3,6));
		GameObject fg6h = cubeHexes [fg6];
		GameObject REdgeFG6 = fg6h.transform.FindChild ("REdge").gameObject;
		GameObject RTEdgeFG6 = fg6h.transform.FindChild ("RTEdge").gameObject;
		*/

	}

	// Use this for initialization
	void Start () 
	{
		setHexSizes ();
		//buildHarbours();
		//buildFishingGroundsUI ();
	}

	// Update is called once per frame
	void Update () {

	}
}
