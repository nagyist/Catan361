using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : Singleton<GameEventManager> {
	public bool IsEventMoveRobberPirateEntitySet = false;
	public string EventMoveRobberPirateEntityType = "";

	public bool IsEventBarbarianSet = false;
	public BarbarianInvasion CurrentBarbarianInvasion = null;

	public void HandleMoveRobberPirateDecision(string type) {
		if (IsEventMoveRobberPirateEntitySet) {
			return;
		}

		if (type == "robber") {
			EventMoveRobberPirateEntityType = "robber";
		} else if (type == "pirate") {
			EventMoveRobberPirateEntityType = "pirate";
		}

		this.IsEventMoveRobberPirateEntitySet = true;
		GameManager.GUI.PostStatusMessage ("Choose a tile to place the " + type + " on.");
	}

	public void HandleMoveRobberPirate(UIHex hex) {
		if (!IsEventMoveRobberPirateEntitySet) {
			return;
		}

		HexTile moveToHexTile = GameManager.Instance.GetCurrentGameState ().CurrentBoard [hex.HexGridCubePosition];
		if (EventMoveRobberPirateEntityType == "robber") {
			if (moveToHexTile.IsWater) {
				StartCoroutine (GameManager.GUI.ShowMessage ("You cannot place the robber on water tiles."));
				return;
			}
		} else if (EventMoveRobberPirateEntityType == "pirate") {
			if (!moveToHexTile.IsWater) {
				StartCoroutine (GameManager.GUI.ShowMessage ("You cannot place the pirate on land tiles."));
				return;
			}
		}

		GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdHandleMoveRobberPirateEntity (EventMoveRobberPirateEntityType, SerializationUtils.ObjectToByteArray(hex.HexGridCubePosition));
		IsEventMoveRobberPirateEntitySet = false;
	}

	public void TriggerBarbarianInvasion(BarbarianInvasion invasion) {
		CurrentBarbarianInvasion = invasion;
	}

}
