using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldPopup : MonoBehaviour {

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

        Debug.Log("Player selected: " + resourceSelected);

        GamePlayer player = GameManager.LocalPlayer.GetComponent<GamePlayer>();
		ResourceCollection.PlayerResourcesCollection playerResources = player.GetPlayerResources ();
		player.CmdUpdateResource (selected, playerResources [selected] + 1);

		GameManager.GUI.HideGoldPopup ();
    }

}
