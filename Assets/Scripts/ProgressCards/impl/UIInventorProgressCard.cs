using System;
using UnityEngine;

public class UIInventorProgressCard : MonoBehaviour
{
	public bool FirstTileSelected;
	public Vec3 FirstTile;

	public bool SecondTileSelected;
	public Vec3 SecondTile;

	void Start() {
		FirstTileSelected = false;
		SecondTileSelected = false;
		StartCoroutine (GameManager.GUI.ShowMessage ("Select two tiles two swap token except 2, 12, 6 or 8."));
		GameManager.GUI.PostStatusMessage ("Select two tiles two swap token except 2, 12, 6 or 8.");
	}

	public void SelectTile(Vec3 tile) {
		if (!FirstTileSelected) {
			FirstTile = tile;
			FirstTileSelected = true;
		} else {
			SecondTile = tile;
			SecondTileSelected = true;
		}
	}

	public bool IsTileSelected(Vec3 tile) {
		return FirstTile.Equals(tile) || SecondTile.Equals(tile);
	}

	void Update() {
		if (FirstTileSelected && SecondTileSelected) {
			HexTile t1 = GameManager.Instance.GetCurrentGameState ().CurrentBoard [FirstTile];
			HexTile t2 = GameManager.Instance.GetCurrentGameState ().CurrentBoard [SecondTile];

			int tmp = t1.SelectedNum;
			t1.SelectedNum = t2.SelectedNum;
			t2.SelectedNum = tmp;

			GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdUpdateHexTile (SerializationUtils.ObjectToByteArray (FirstTile), SerializationUtils.ObjectToByteArray (t1));
			GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdUpdateHexTile (SerializationUtils.ObjectToByteArray (SecondTile), SerializationUtils.ObjectToByteArray (t2));

			GameManager.Instance.GetCurrentGameState ().RpcClientShowMessage (GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName + " swapped two tiles!", 2.75f);
			GameManager.Instance.GetCurrentGameState ().RpcClientPostStatusMessage (GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName + " used his inventory card!");

			GameManager.LocalPlayer.GetComponent<GamePlayer> ().inventorProgressCardInUse = false;
			Destroy (gameObject);
		}
	}
}

