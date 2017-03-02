using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	[SerializeField]
	public GameObject hexPrefab;

	int width = 11;
	int height = 11;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {

				Instantiate (hexPrefab, new Vector3 (i, j, 0), Quaternion.identity);

			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
