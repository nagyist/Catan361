using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameTurn
{
	public List<string> OrderedPlayers = new List<String> (new string[] { "Player1", "Player2", "Player3" }); 
	public int CurrentPlayerIndex { get; private set; }
	public bool CurrentTurnEnded { get; private set; }
	public int RoundCount { get; private set; }

	public GameTurn () { 
		CurrentPlayerIndex = -1;
		RoundCount = 0;
		CurrentTurnEnded = true;
	}

	public bool IsInSetupPhase() {
		return RoundCount < 2;
	}

	public bool PlayerTakeTurn(string name) {
		if (IsPlayerAllowedToTakeTurn(name)) {
			CurrentPlayerIndex = GetNextPlayerTurn ();
			CurrentTurnEnded = false;

			return true;
		}

		return false;
	}

	public bool EndTurn(string name) {
		if (!IsPlayerTurn (name)) {
			return false;
		}

		CurrentTurnEnded = true;

		if (CurrentPlayerIndex == 1) {
			RoundCount++;
		}

		return true;
	}

	public bool LocalPlayerEndTurn() {
		if (IsLocalPlayerTurn ()) {
			CurrentTurnEnded = true;
		}

		return true;
	}

	public int GetNextPlayerTurn() {
		return (CurrentPlayerIndex + 1) % OrderedPlayers.Count;
	}

	public bool IsTurnTaken() {
		return this.CurrentTurnEnded == false;
	}

	public bool IsLocalPlayerAllowedToTakeTurn() {
		return GetNextPlayerTurn () == OrderedPlayers.IndexOf(GameManager.LocalPlayer.GetComponent<GamePlayer>().myName) && ! IsTurnTaken ();
	}

	public bool IsPlayerAllowedToTakeTurn(string name) {
		return GetNextPlayerTurn () == OrderedPlayers.IndexOf (name) && !IsTurnTaken ();
	}

	public bool IsPlayerAllowedToTakeTurn(int idx) {
		return GetNextPlayerTurn () == idx && !IsTurnTaken ();
	}

	public bool IsLocalPlayerTurn() {
		return CurrentPlayerIndex == OrderedPlayers.IndexOf(GameManager.LocalPlayer.GetComponent<GamePlayer>().myName) && IsTurnTaken ();
	}

	public bool IsPlayerTurn(string name) {
		return CurrentPlayerIndex == OrderedPlayers.IndexOf (name) && IsTurnTaken ();
	}
}
