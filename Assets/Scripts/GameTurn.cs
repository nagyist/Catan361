using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTurn : MonoBehaviour
{
	public int MaximumPlayer = 2;
	public int CurrentPlayerIndex { get; private set; }
	public bool CurrentTurnEnded { get; private set; }

	public GameTurn () { 
		CurrentPlayerIndex = -1;
		CurrentTurnEnded = true;
	}

	public bool LocalPlayerTakeTurn() {
		if (IsLocalPlayerAllowedToTakeTurn()) {
			CurrentPlayerIndex = GetNextPlayerTurn ();
			CurrentTurnEnded = false;
			return true;
		}

		return false;
	}

	public bool LocalPlayerEndTurn() {
		if (IsLocalPlayerTurn ()) {
			CurrentTurnEnded = true;
		}

		return false;
	}

	public int GetNextPlayerTurn() {
		return (CurrentPlayerIndex + 1) % MaximumPlayer;
	}

	public bool IsTurnTaken() {
		return this.CurrentTurnEnded == false;
	}

	public bool IsLocalPlayerAllowedToTakeTurn() {
		return GetNextPlayerTurn () == GameManager.Instance.GetLocalPlayerIndex() && ! IsTurnTaken ();
	}

	public bool IsLocalPlayerTurn() {
		return CurrentPlayerIndex == GameManager.Instance.GetLocalPlayerIndex () && IsTurnTaken ();
	}
}
