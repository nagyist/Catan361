using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harbour : MonoBehaviour {

	public int exchangeRate;
	public StealableType returnedResource;
	public int returnedAmount;
	public Edge harbourEdge;

	void OnMouseEnter(){
		GetComponentInChildren<SpriteRenderer>().color = new Color32 (0, 255, 0, 255);
	}

	void OnMouseExit(){
		GetComponentInChildren<SpriteRenderer>().color = new Color32 (255, 255, 255, 255);
	}

	void OnMouseDown() 
	{
		//TODO: 
		// if localplayer does not have settlement on intersections adj to edge:
		//StartCoroutine (GameManager.GUI.ShowMessage("You cannot access this harbour until you have a settlement or city adjacent to it."));
		//else
		StartCoroutine (GameManager.GUI.ShowMessage("Harbour trade, resource returned: " + returnedResource));

		GameManager.GUI.ShowHarbourTradePopup ();

		GameObject tradeWindow = GameManager.GUI.guiCanvas.transform.FindChild ("HarbourTradeMenu").gameObject;

		if(tradeWindow.activeSelf == false)
		{
			tradeWindow.SetActive(true);
		}

		// if returnedResource != null then
		HarbourTrade tradeWindowScript = tradeWindow.GetComponent<HarbourTrade> ();
		tradeWindowScript.exchangeRate = exchangeRate;
		tradeWindowScript.returnedResource = returnedResource;
		tradeWindowScript.returnedAmount = returnedAmount;

		/* else
		HarbourTrade tradeWindowScript = tradeWindow.GetComponent<HarbourTrade> ();
		tradeWindowScript.exchangeRate = exchangeRate;
		tradeWindowScript.returnedResource = returnedResource;
		tradeWindowScript.returnedAmount = returnedAmount;
		*/
	}

	// Update is called once per frame
	void Update () {
		
	}
}
