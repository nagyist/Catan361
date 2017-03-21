using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourcePanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	string ResDisplay(GamePlayer player, StealableType type) {
		if (player.playerResources.ContainsKey (type)) {
			return "" + player.playerResources [type];
		} else {
			return "0";
		}
	}


	// Update is called once per frame
	void Update () {

		if (GameManager.Instance.GameStateReadyAtStage (GameState.GameStatus.GRID_CREATED)) {

            string buffer;
			GamePlayer currentPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer> ();

            buffer = "Wool: " + ResDisplay(currentPlayer, StealableType.Resource_Wool) + "\n" +
                "Lumber: " + ResDisplay(currentPlayer, StealableType.Resource_Lumber) + "\n" +
                "Ore: " + ResDisplay(currentPlayer, StealableType.Resource_Ore) + "\n" +
                "Brick: " + ResDisplay(currentPlayer, StealableType.Resource_Brick) + "\n" +
                "Grain: " + ResDisplay(currentPlayer, StealableType.Resource_Grain) + "\n";

            List<GameObject> connectedPlayers = GameManager.ConnectedPlayers;
            foreach (GameObject i in connectedPlayers)
            {
                int count = 0;
                if (!i.Equals(currentPlayer))
                {
                    foreach(KeyValuePair<StealableType, int> entry in i.GetComponent<GamePlayer>().playerResources)
                    {
                        count += entry.Value;
                    }
                    buffer += "\nName: " + i.GetComponent<GamePlayer>().myName + "\nResource Cards: " + count + "\n";
                }
            }


            /*
			GetComponentInChildren<Text> ().text = 
				"Wool: " + ResDisplay(currentPlayer, StealableType.Resource_Wool) + "\n" +
				"Lumber: " + ResDisplay(currentPlayer, StealableType.Resource_Lumber) + "\n" + 
				"Ore: " + ResDisplay(currentPlayer, StealableType.Resource_Ore) + "\n" + 
				"Brick: " + ResDisplay(currentPlayer, StealableType.Resource_Brick) + "\n" + 
				"Grain: " + ResDisplay(currentPlayer, StealableType.Resource_Grain) +
                "\n\n" + ;
                */
            GetComponentInChildren<Text>().text = buffer;
		}

	}
}
