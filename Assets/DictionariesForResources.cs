using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DictionariesForResources : MonoBehaviour {

	public static Dictionary<String, Texture2D> resourceTextureList = new Dictionary<String, Texture2D> ();

	public void AddTexture(String num)
	{
		Texture2D texture = (Texture2D)Resources.Load ("GoodDirt.psd");
		resourceTextureList.Add(num, texture);
	}

	// Use this for initialization
	void Start () {
		AddTexture ("1");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
