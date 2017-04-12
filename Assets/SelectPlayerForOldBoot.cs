using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayerForOldBoot : MonoBehaviour {

	public void accepted()
	{
		ToggleGroup toggles = gameObject.GetComponentInChildren<ToggleGroup> ();
		IEnumerator<Toggle> togglesEnum = toggles.ActiveToggles ().GetEnumerator ();
		togglesEnum.MoveNext ();
		Toggle toggle = togglesEnum.Current;
		string actionSelected = toggle.transform.parent.name;
		string entityType = "";

		GamePlayer player = GameManager.LocalPlayer.GetComponent<GamePlayer> ();
		ResourceCollection.PlayerResourcesCollection playerResources = player.GetPlayerResources ();

		if (actionSelected.Equals ("SelectPlayerEntry1")) {
			string name1 = "Player1";
			if (name1 == player.myName) 
			{
				GameManager.GUI.HidePlayerSelectPopup ();
				StartCoroutine (GameManager.GUI.ShowMessage ("You already have the boot! Select another player."));
			} 
			else if (name1 != player.myName) 
			{
				GamePlayer playerForBoot = GameManager.ConnectedPlayersByName[name1].GetComponent<GamePlayer>();
				playerForBoot.gotOldBoot = true;
				GameManager.GUI.HidePlayerSelectPopup ();

			}
			GameManager.GUI.HidePlayerSelectPopup ();

		} else if (actionSelected.Equals ("SelectPlayerEntry2")) {
			string name2 = "Player2";
			if (name2 == player.myName) 
			{
				GameManager.GUI.HidePlayerSelectPopup ();
				StartCoroutine (GameManager.GUI.ShowMessage ("You already have the boot! Select another player."));
			} 
			else if (name2 != player.myName) 
			{
				GamePlayer playerForBoot = GameManager.ConnectedPlayersByName[name2].GetComponent<GamePlayer>();
				playerForBoot.gotOldBoot = true;
				GameManager.GUI.HidePlayerSelectPopup ();

			}
			GameManager.GUI.HidePlayerSelectPopup ();

		} else if (actionSelected.Equals ("SelectPlayerEntry3")) {
			string name3 = "Player3";
			if (name3 == player.myName) 
			{
				GameManager.GUI.HidePlayerSelectPopup ();
				StartCoroutine (GameManager.GUI.ShowMessage ("You already have the boot! Select another player."));
			} 
			else if (name3 != player.myName) 
			{
				GamePlayer playerForBoot = GameManager.ConnectedPlayersByName[name3].GetComponent<GamePlayer>();
				playerForBoot.gotOldBoot = true;
				GameManager.GUI.HidePlayerSelectPopup ();

			}
			GameManager.GUI.HidePlayerSelectPopup ();

		}
	}
}
