using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIIntrigueProgressCard : MonoBehaviour
{
	public AbstractProgressCard CurrentCard;
	public Intersection toMoveKnight;
	public Intersection moveSelection;

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

		toMoveKnight = i;

		StartCoroutine (GameManager.GUI.ShowMessage ("Select the destination where to displace the selected knight."));
		GameManager.GUI.PostStatusMessage ("Select the destination where to displace the selected knight.");
	}

	public void ClickChooseDestination(Intersection i) {
		moveSelection = i;

		Vec3[] oldPos = new Vec3[] { toMoveKnight.adjTile1, toMoveKnight.adjTile2, toMoveKnight.adjTile3 };
		Vec3[] selectedPos = new Vec3[] { moveSelection.adjTile1, moveSelection.adjTile2, moveSelection.adjTile3 };
		String owner = toMoveKnight.Owner;
		Knight replacedKnight = (Knight)toMoveKnight.unit;
		
		
		if (UnitMoveButton.checkKnightRemoval(oldPos, toMoveKnight.Owner))
		{
			// moves knight by overriding old position
			// also calls rpc function to lower the number of knights owned of the old intersection owner
			GameManager.LocalPlayer.GetComponent<GamePlayer>().CmdMoveUnitWithRemoval(
				SerializationUtils.ObjectToByteArray(oldPos),
				SerializationUtils.ObjectToByteArray(selectedPos),
				SerializationUtils.ObjectToByteArray(toMoveKnight.Owner),
				SerializationUtils.ObjectToByteArray(replacedKnight));

			StartCoroutine(GameManager.GUI.ShowMessage("You have removed " + toMoveKnight.Owner + "'s knight."));
		} else {

			if (!UnitMoveButton.checkForPath(oldPos, selectedPos, owner))
			{
				StartCoroutine(GameManager.GUI.ShowMessage("Selected intersection must be on valid path."));
				return;
			}
			
			// moves knight and calls rpc function to add replaced knight to queue for the previous owner
			GameManager.LocalPlayer.GetComponent<GamePlayer>().CmdMoveUnitWithReplacement(
				SerializationUtils.ObjectToByteArray(oldPos),
				SerializationUtils.ObjectToByteArray(selectedPos),
				SerializationUtils.ObjectToByteArray(toMoveKnight.Owner),
				SerializationUtils.ObjectToByteArray(replacedKnight));

			StartCoroutine(GameManager.GUI.ShowMessage("You have displaced " + toMoveKnight.Owner + "'s knight."));
		}

		CurrentCard.RemoveFromPlayerHand ();
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
