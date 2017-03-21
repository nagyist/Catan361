using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * this class holds information about the ggame's current turn
*/

// seriazalable means the class can be turned into a stream of bytes for networking purposes
[Serializable]
public class GameTurn
{
    // it holds an ordered list of players
	public List<string> OrderedPlayers = new List<String> (new string[] { "Player1", "Player2", "Player3" }); 
    // holds the current player's index in the list
	public int CurrentPlayerIndex { get; private set; }
    // also holds boolean values to denote a players turn has ended
	public bool CurrentTurnEnded { get; private set; }
    // holds a counter for rounds
	public int turnCount { get; private set; }

    // construct and set's the current player's index to invalid value
	public GameTurn () { 
		CurrentPlayerIndex = 3;
		turnCount = 0;
		CurrentTurnEnded = true;
	}

    // if return true if less than 2 rounds have passed
	public bool IsInSetupPhase() {
		return turnCount < 6;
	}

    // this function is used when a player can take their turn
	public bool PlayerTakeTurn(string name) {

        // only execute if the player is allowed to take their turn
        if (IsPlayerAllowedToTakeTurn(name)) {
            // set the player index to next player and set the current turn as not ented
			CurrentPlayerIndex = GetNextPlayerTurn ();
			CurrentTurnEnded = false;
			return true;
		}
        else
		    return false;
	}

    // function is called by GamePlayer when a player ends their turn
	public bool EndTurn(string name) {

        // returns false it is not the current player's turn
        if (!IsPlayerTurn(name))
            return false;

        // set the current turn as ended
        CurrentTurnEnded = true;
        
        turnCount++;
		return true;
	}

    // review for this function is needed; it always returns true
    // seems its not called anywhere either
	public bool LocalPlayerEndTurn() {
		if (IsLocalPlayerTurn ()) {
			CurrentTurnEnded = true;
		}
		return true;
	}

    // this function upgate the next player's turn
	public int GetNextPlayerTurn() {
        if (IsInSetupPhase())
            return (CurrentPlayerIndex - 1) % OrderedPlayers.Count;
        else
            return (CurrentPlayerIndex + 1) % OrderedPlayers.Count;      
	}

    // getter for turn ended boolean
	public bool IsTurnTaken() {
		return this.CurrentTurnEnded == false;
	}

    // returns true is local player is allowed to take a turn and the turn is not already taken
    public bool IsLocalPlayerAllowedToTakeTurn() {
		return GetNextPlayerTurn () == OrderedPlayers.IndexOf(GameManager.LocalPlayer.GetComponent<GamePlayer>().myName) && ! IsTurnTaken ();
	}

    // returns true if player with myName = name is allowed to take a turn and the turn is not already taken
    public bool IsPlayerAllowedToTakeTurn(string name) {
		return GetNextPlayerTurn () == OrderedPlayers.IndexOf (name) && !IsTurnTaken ();
	}

    // return true is player with index idx is allowed to take a turn and the turn is not already taken
    public bool IsPlayerAllowedToTakeTurn(int idx) {
		return GetNextPlayerTurn () == idx && !IsTurnTaken ();
	}

    // returns true if it is the local player's turn and the turn is not already taken
    public bool IsLocalPlayerTurn() {
		return CurrentPlayerIndex == OrderedPlayers.IndexOf(GameManager.LocalPlayer.GetComponent<GamePlayer>().myName) && IsTurnTaken ();
	}

    // returns true if player with myName = name is allowed to take a turn and the turn is not already taken
    public bool IsPlayerTurn(string name) {
		return CurrentPlayerIndex == OrderedPlayers.IndexOf (name) && IsTurnTaken ();
	}
}
