using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harbour : MonoBehaviour {

	public int exchangeRate;
	public StealableType returnedResource;
	public int returnedAmount;
	public Edge harbourEdge;
	public List <Intersection> intersectionsCanAccessHarbour;

	void OnMouseEnter(){
		GetComponentInChildren<SpriteRenderer>().color = new Color32 (0, 255, 0, 255);
	}

	void OnMouseExit(){
		GetComponentInChildren<SpriteRenderer>().color = new Color32 (255, 255, 255, 255);
	}

	void OnMouseDown() 
	{
		GamePlayer player = GameManager.LocalPlayer.GetComponent<GamePlayer> ();

		if (CanOpen (player)) {

			GameManager.GUI.ShowHarbourTradePopup ();

			GameObject tradeWindow = GameManager.GUI.guiCanvas.transform.FindChild ("HarbourTradeMenu").gameObject;

			if (tradeWindow.activeSelf == false) {
				tradeWindow.SetActive (true);
			}

			HarbourTrade tradeWindowScript = tradeWindow.GetComponent<HarbourTrade> ();
			tradeWindowScript.exchangeRate = exchangeRate;
			tradeWindowScript.returnedResource = returnedResource;
			tradeWindowScript.returnedAmount = returnedAmount;

		} 
		else 
		{
			StartCoroutine (GameManager.GUI.ShowMessage ("You cannot access this harbour until you have a settlement or city adjacent to it."));
		}
	}
			
	public bool CanOpen(GamePlayer aPlayer)
	{
		foreach (Intersection i in intersectionsCanAccessHarbour) {
			if (i.Owner == aPlayer.myName) {
				return true;
			}
		}

		return false;
		
	}
		
	public void SetSprites () 
	{
		GameObject harbour = this.gameObject;
		/*
		if (returnedResource == StealableType.Resource_Brick)
		{
			Sprite brickSprite = harbour.GetComponent<SpriteRenderer> ().sprite; 
			brickSprite = FindObjectOfType <Sprite> ("ore_f_b_02");	
		}
		else if (returnedResource == StealableType.Resource_Grain)
		{
			Sprite grainSprite = harbour.GetComponent<SpriteRenderer> ().sprite;
			grainSprite = (Sprite) FindObjectOfType ("hb_b_15");	
		}	

		else if (returnedResource == StealableType.Resource_Lumber)
		{
			Sprite lumberSprite = harbour.GetComponent<SpriteRenderer> ().sprite; 
			lumberSprite = (Sprite) FindObjectOfType ("wd_b_06");	
		}
		else if (returnedResource == StealableType.Resource_Ore)
		{
			Sprite oreSprite = harbour.GetComponent<SpriteRenderer> ().sprite;
			oreSprite = (Sprite) FindObjectOfType ("ore_n_01_b");	
		}
		else if (returnedResource == StealableType.Resource_Wool)
		{
			Sprite woolSprite = harbour.GetComponent<SpriteRenderer> ().sprite; 
			woolSprite = (Sprite) FindObjectOfType ("lz_b_07");	
		}
		*/
	}
}
