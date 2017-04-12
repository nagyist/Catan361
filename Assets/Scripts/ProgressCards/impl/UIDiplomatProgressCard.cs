using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIDiplomatProgressCard : MonoBehaviour
{
	public AbstractProgressCard CurrentCard;

	void Start() {
		StartCoroutine(GameManager.GUI.ShowMessage ("Select an open road to remove."));
		GameManager.GUI.PostStatusMessage ("Select an open road to remove from the game board (diplomat card).");
	}

	public void SelectRoad(Edge selectedEdge) {
		if (selectedEdge.Owner == "") {
			StartCoroutine (GameManager.GUI.ShowMessage ("You must select an active road."));
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().diplomatProgressCardUsed = false;
			return;
		}

		if (!isOpenRoad (selectedEdge)) {
			StartCoroutine (GameManager.GUI.ShowMessage ("You must select an open road."));
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().diplomatProgressCardUsed = false;
			return;
		}

		if (selectedEdge.Owner == GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName) {
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().diplomatCanPlaceRoadForFree = true;
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().diplomatProgressCardUsed = false;
			GameManager.GUI.PostStatusMessage ("You removed one of you own road. Place it anywhere else.");
		}

		selectedEdge.Owner = "";
		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdUpdateEdge (selectedEdge.getKey (), SerializationUtils.ObjectToByteArray (selectedEdge));
		GameManager.Instance.GetCurrentGameState ().RpcClientShowMessage (GameManager.LocalPlayer.GetComponent<GamePlayer>().myName + " removed a road !", 2.0f);

		CurrentCard.RemoveFromPlayerHand ();
		Destroy (gameObject);
	}


	private bool isOpenRoad(Edge edge) {
		IntersectionCollection intersectionCollection = GameManager.Instance.GetCurrentGameState ().CurrentIntersections;
		List<Intersection> connectedIntersectionsFound = new List<Intersection> ();
		foreach (Intersection i in intersectionCollection.Intersections.Values) {
			if (i.Owner == "") {
				continue;
			}
			List<Vec3> adjTiles = new List<Vec3> ();
			adjTiles.Add (i.adjTile1);
			adjTiles.Add (i.adjTile2);
			adjTiles.Add (i.adjTile3);

			int numConnected = adjTiles.Where (x => x.Equals (edge.adjTile1) || x.Equals (edge.adjTile2)).ToList().Count;
			if (numConnected >= 2) {
				if (!connectedIntersectionsFound.Contains (i)) {
					connectedIntersectionsFound.Add (i);
				}
			}

			if (connectedIntersectionsFound.Count >= 2) {
				return false;
			}
		}

		return true;
	}

	void Update() {
		
	}
}

