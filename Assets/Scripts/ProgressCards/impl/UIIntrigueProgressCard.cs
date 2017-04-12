using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIIntrigueProgressCard : MonoBehaviour
{
	public AbstractProgressCard CurrentCard;

	void Start() {
		StartCoroutine (GameManager.GUI.ShowMessage ("Select one of your opponent's knight connected to one of your road."));
	}

	public void ClickChooseIntersection(Intersection i) {
		if (i.Owner == "" || i.Owner == GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName) {
			StartCoroutine (GameManager.GUI.ShowMessage ("You must select a knight of one of your opponent."));
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().intrigueProgressCardUsed = false;
			return;
		}

		if (i.unit == null || i.unit.GetType () != typeof(Knight)) {
			StartCoroutine (GameManager.GUI.ShowMessage ("You must select a knight of one of your opponent."));
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().intrigueProgressCardUsed = false;
			return;
		}

		if (!connectedToAnOwnedRoad (i)) {
			StartCoroutine (GameManager.GUI.ShowMessage ("You must select a knight connected to one of your road."));
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().intrigueProgressCardUsed = false;
			return;
		}
	}

	public bool connectedToAnOwnedRoad(Intersection i) {
		List<Edge> connectedEdges = new List<Edge> ();
		connectedEdges.Add (GameManager.Instance.GetCurrentGameState ().CurrentEdges.getEdge (i.adjTile1, i.adjTile2));
		connectedEdges.Add (GameManager.Instance.GetCurrentGameState ().CurrentEdges.getEdge (i.adjTile1, i.adjTile3));
		connectedEdges.Add (GameManager.Instance.GetCurrentGameState ().CurrentEdges.getEdge (i.adjTile1, i.adjTile2));
		return connectedEdges.Where (x => x.Owner == GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName).ToList ().Count > 0;
	}

	void Update() {
		
	}
}
