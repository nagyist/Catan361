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
	public List<string> PlayerOrder = new List<string> (new string[] { "Player1", "Player2" }); 
    // holds the current player's index in the list
	public int CurrentPlayerIndex { get; private set; }
    // also holds boolean values to denote a players turn has ended
	public bool CurrentTurnEnded { get; private set; }
    // holds a counter for rounds
	public int RoundCount { get; private set; }

    // contructor
    // set's the current player's index to invalid value
	public GameTurn () { 
		CurrentPlayerIndex = -1;
		RoundCount = 0;
		CurrentTurnEnded = true;
	}

    // if return true if less than 2 rounds have passed
	public bool IsInSetupPhase() {
		return RoundCount < 2;
	}

	public List<string> OrderedPlayers() {
		if (IsInSetupPhase () && RoundCount == 1) {
			List<string> newOrder = new List<string>(PlayerOrder);
			newOrder.Reverse ();
			return newOrder;
		} else {
			return PlayerOrder;
		}
	}

    // this function is used to check whether a player can take their turn
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

    // function is used when a player ends their turn
    // called by GamePlayer
	public bool EndTurn(string name) {

        // returns false it is not the current player's turn
        if (!IsPlayerTurn(name))
            return false;

        // set the current turn as ended
        CurrentTurnEnded = true;

		if (CurrentPlayerIndex == (OrderedPlayers().Count - 1))
        {
            RoundCount++;
        }
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
        if (RoundCount == 1)
			return (OrderedPlayers().Count + CurrentPlayerIndex - 1) % OrderedPlayers().Count;
        else
			return (CurrentPlayerIndex + 1) % OrderedPlayers().Count;
    }

    // getter for turn ended boolean
	public bool IsTurnTaken() {
		return this.CurrentTurnEnded == false;
	}

    /* returns true if both:
     *      1. the index retreived by GetNextPlayerTurn() function is the the local player
     *      2. the current turn is not taken
    */
    public bool IsLocalPlayerAllowedToTakeTurn() {
		return GetNextPlayerTurn () == OrderedPlayers().IndexOf(GameManager.LocalPlayer.GetComponent<GamePlayer>().myName) && ! IsTurnTaken ();
	}

    /* returns true if both:
     *      1. the index retreived by the GetNextPlayerTurn() function equivalent to the player argument's index 
     *      2. the current turn is not taken
    */
    public bool IsPlayerAllowedToTakeTurn(string name) {
		return GetNextPlayerTurn () == OrderedPlayers().IndexOf (name) && !IsTurnTaken ();
	}

    // note function needs review, doesn't seem to be called anywhere
    /* returns true if both:
     *      1. the index retreived by the GetNextPlayerTurn() function is equal to the index argument
     *      2. the current turn is not taken
    */
	public bool IsPlayerAllowedToTakeTurn(int idx) {
		return GetNextPlayerTurn () == idx && !IsTurnTaken ();
	}

    /* return true if both:
     *      1. the index retreived by the GetNextPlayerTurn() function equivalent to the player argument's index 
     *      2. the curren turn is not taken
    */
    public bool IsLocalPlayerTurn() {
		return CurrentPlayerIndex == OrderedPlayers().IndexOf(GameManager.LocalPlayer.GetComponent<GamePlayer>().myName) && IsTurnTaken ();
	}

    /* returns true if both:
     *      1. the index retreived by the GetNextPlayerTurn() function equivalent to the player argument's index 
     *      2. the current turn is not taken
    */
    public bool IsPlayerTurn(string name) {
		return CurrentPlayerIndex == OrderedPlayers().IndexOf (name) && IsTurnTaken ();
	}
}
