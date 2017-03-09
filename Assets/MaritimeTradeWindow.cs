using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaritimeTradeWindow : MonoBehaviour {

	public GameObject brick;
	public GameObject grain;
	public GameObject ore;
	public GameObject wool;
	public GameObject lumber;
	public GameObject harbour;

	/*
	public Harbour findHarbour(string harbourNumber)
	{
		if (HexGrid.harbourCollection.ContainsKey(harbourNumber))
			{
				return HexGrid.harbourCollection[harbourNumber];
			} 
			else
			{
				print ("Harbour does not exist");
			}
	}
	*/


	// Use this for initialization
	void Start () {
		var brickNum = brick.GetComponentInChildren<Text>();
		var grainNum = grain.GetComponentInChildren<Text>();
		var oreNum = grain.GetComponentInChildren<Text>();
		var woolNum = grain.GetComponentInChildren<Text>();
		var lumberNum = grain.GetComponentInChildren<Text>();
		var harbourNum = harbour.GetComponentInChildren<Text>();
		Text [] resourceOffer = {brickNum, grainNum, oreNum, woolNum, lumberNum};
	}

	// Update is called once per frame
	void Update () {
		
	}
}
