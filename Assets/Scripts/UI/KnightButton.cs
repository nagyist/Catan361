using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnightButton : MonoBehaviour
{
    GamePlayer localPlayer;
    string localPlayerName;
    UIIntersection selectedUIIntersection;
    Intersection selectedIntersection;
    IntersectionUnit selectedUnit;

    // this is the function that's called when the player presses on the knight button
    public void ClickBuild()
    {
        localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        localPlayerName = localPlayer.myName;
        selectedUIIntersection = localPlayer.selectedUIIntersection;

        // call the upgrade or hire function depending on the level of the knight
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>
                (new Vec3[] { selectedUIIntersection.HexPos1, selectedUIIntersection.HexPos2, selectedUIIntersection.HexPos3 }));
        
        if (intersection.unit == null)
            selectedUIIntersection.HireKnight();
        else
            selectedUIIntersection.UpgradeKnight();

    }

    // update controls whether or not the button is enabled
    // additionally, it will change the text of the button
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
                GetComponentInChildren<Text>().text = "NO KNIGHT SELECTION";
                return;
            }

            Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>
                (new Vec3[] { selectedUIIntersection.HexPos1, selectedUIIntersection.HexPos2, selectedUIIntersection.HexPos3 }));

            // if intersection is empty
            if (intersection.unit == null)
            {
                GetComponent<Button>().enabled = true;
                GetComponentInChildren<Text>().text = "HIRE KNIGHT";
                return;
            }
            // if someone already owns the intersection
            else
            {
                if (intersection.Owner == localPlayer.myName)
                {
                    GetComponent<Button>().enabled = true;
                    GetComponentInChildren<Text>().text = "UPGRADE KNIGHT";
                    return;
                }
                else
                {
                    GetComponent<Button>().enabled = false;
                    GetComponentInChildren<Text>().text = "KNIGHT ALREADY OWNED";
                    return;
                }
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
