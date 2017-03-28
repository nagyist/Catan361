using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildUpgradeButton : MonoBehaviour
{

    GamePlayer localPlayer;
    string localPlayerName;
    UIIntersection selectedUIIntersection;
    Intersection selectedIntersection;
    IntersectionUnit selectedUnit;

    public void ClickBuild()
    {
        localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        localPlayerName = localPlayer.myName;
        selectedUIIntersection = localPlayer.selectedUIIntersection;

        selectedUIIntersection.CreateSettlement();
       
    }

    // Update is called once per frame
    void Update()
    {


        if (!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED))
        {
            return;
        }

        if (GameManager.Instance.GetCurrentGameState().CurrentTurn.IsLocalPlayerTurn())
        {
            localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
            selectedUIIntersection = localPlayer.selectedUIIntersection;

            // check to see if there is a selection
            if (localPlayer.selectedUIIntersection == null)
            {
                GetComponent<Button>().enabled = false;
                GetComponentInChildren<Text>().text = "BUILD: NO SELECTION";
                return;
            }

            Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>
                (new Vec3[] { selectedUIIntersection.HexPos1, selectedUIIntersection.HexPos2, selectedUIIntersection.HexPos3 }));

            // if intersection is empty
            if (intersection.unit == null)
            {
                GetComponent<Button>().enabled = true;
                GetComponentInChildren<Text>().text = "BUILD SETTLEMENT";
                return;
            }
            // if someone already owns the intersection
            else
            {
                GetComponent<Button>().enabled = false;
                GetComponentInChildren<Text>().text = "INTERSECTION OWNED";
                return;

            }
        }
        // if it isn't the player's turn
        else
        {
            GetComponent<Button>().enabled = false;
            GetComponentInChildren<Text>().text = "NOT YOUR TURN";
            return;
        }
    }

}
