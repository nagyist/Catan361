using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceMonopolyPopup : MonoBehaviour {

	public AbstractProgressCard CurrentCard;

	public void accepted()
	{
		ToggleGroup toggles = gameObject.GetComponentInChildren<ToggleGroup>();
		IEnumerator<Toggle> togglesEnum = toggles.ActiveToggles().GetEnumerator();
		togglesEnum.MoveNext();
		Toggle toggle = togglesEnum.Current;
		string resourceSelected = toggle.transform.parent.name;
		StealableType selected;

		if (resourceSelected.Equals("Brick"))
		{
			selected = StealableType.Resource_Brick;
		}
		else if (resourceSelected.Equals("Grain"))
		{
			selected = StealableType.Resource_Grain;
		}
		else if (resourceSelected.Equals("Lumber"))
		{
			selected = StealableType.Resource_Lumber;
		}
		else if (resourceSelected.Equals("Ore"))
		{
			selected = StealableType.Resource_Ore;
		}
		else
		{
			selected = StealableType.Resource_Wool;
		}

		foreach (GameObject player in GameManager.ConnectedPlayers) {
			Dictionary<StealableType, int> reqResources = new Dictionary<StealableType, int> () {
				{selected, 2}
			};

			if (GameManager.Instance.GetCurrentGameState ().CurrentResources.PlayerHasEnoughResources (player.GetComponent<GamePlayer>().myName, reqResources)) {
				player.GetComponent<GamePlayer> ().CmdConsumeResources (reqResources);
				GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdAddResourcesResources (SerializationUtils.ObjectToByteArray(reqResources));
			}
		}

		StartCoroutine (GameManager.GUI.ShowMessage ("You got 2 " + resourceSelected + " from each of your opponent."));
		CurrentCard.RemoveFromPlayerHand ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
