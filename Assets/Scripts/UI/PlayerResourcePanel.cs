using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// this class is used to control the text displayed in the left pnale showing all the players' resources
public class PlayerResourcePanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // this function prints out the local player's resources
	string ResDisplay(GamePlayer player, StealableType type) {
		if (player.playerResources.ContainsKey (type)) {
			return "" + player.playerResources [type];
		} else {
			return "0";
		}
	}


	// Update is called once per frame
	void Update () {
        // check if the game is ready  (if the grid is created)
		if (GameManager.Instance.GameStateReadyAtStage (GameState.GameStatus.GRID_CREATED)) {

            //initialiaze variables
            string buffer;
			GamePlayer currentPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer> ();
            List<GameObject> connectedPlayers = GameManager.ConnectedPlayers;

            // get the resouces for the local player, add it to the buffer
            buffer = "Wool: " + ResDisplay(currentPlayer, StealableType.Resource_Wool) + "\n" +
                "Lumber: " + ResDisplay(currentPlayer, StealableType.Resource_Lumber) + "\n" +
                "Ore: " + ResDisplay(currentPlayer, StealableType.Resource_Ore) + "\n" +
                "Brick: " + ResDisplay(currentPlayer, StealableType.Resource_Brick) + "\n" +
                "Grain: " + ResDisplay(currentPlayer, StealableType.Resource_Grain) + "\n";

            // go through all connected players
            foreach (GameObject i in connectedPlayers)
            {
                int count = 0;
                // only do this for the connected players who aren't the local player
                if (!i.Equals(currentPlayer))
                {
                    // add all their resources to count
                    foreach(KeyValuePair<StealableType, int> entry in i.GetComponent<GamePlayer>().playerResources)
                    {
                        count += entry.Value;
                    }
                    // add the count to resource card count to the buffer
                    buffer += "\nName: " + i.GetComponent<GamePlayer>().myName + "\nResource Cards: " + count + "\n";
                }
            }
            
            // set the text's value to buffer
            GetComponentInChildren<Text>().text = buffer;
		}

	}
}
