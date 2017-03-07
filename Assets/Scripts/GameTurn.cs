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

	public bool TakeTurn() {
		if (IsCurrentPlayerAllowedToTakeTurn()) {
			currentPlayerTurn = GetNextPlayerTurn ();
			currentPlayerEndedTurn = false;
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

	public bool IsCurrentPlayerAllowedToTakeTurn() {
		return GetNextPlayerTurn () == GameManager.Instance.currentPlayer && ! IsTurnTaken ();
	}

	public bool IsCurrentPlayerTurn() {
		return this.currentPlayerTurn == GameManager.Instance.currentPlayer && IsTurnTaken ();
	}
}
