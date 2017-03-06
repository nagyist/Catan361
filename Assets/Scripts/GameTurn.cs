using System;

public class GameTurn
{
	private int maxPlayer = 0;
	private int currentPlayerTurn = -1;
	private bool currentPlayerEndedTurn = true;

	public GameTurn (int maxPlayer)
	{
		this.maxPlayer = maxPlayer;
	}

	public bool TakeTurn(int playerId) {
		if (IsPlayerAllowedToTakeTurn(playerId)) {
			currentPlayerEndedTurn = false;
			currentPlayerTurn = GetNextPlayerTurn ();
			return true;
		}

		return false;
	}

	public int GetNextPlayerTurn() {
		return currentPlayerTurn + 1 % maxPlayer;
	}

	public bool IsTurnTaken() {
		return !this.currentPlayerEndedTurn;
	}

	public bool IsPlayerAllowedToTakeTurn(int playerId) {
		return GetNextPlayerTurn () == playerId && ! IsTurnTaken ();
	}

	public bool IsPlayerTurn(int playerId) {
		return this.currentPlayerTurn == playerId && IsTurnTaken ();
	}
}
