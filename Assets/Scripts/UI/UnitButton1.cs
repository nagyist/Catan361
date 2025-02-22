using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*  Unit button 1 is used for:
 *      creating settlements on empty intersections
 *      upgrading knights
 *      upgrading intersections 
 */

public class UnitButton1 : MonoBehaviour
{

    string localPlayerName;

    public void ClickBuild()
    {
        GamePlayer localPlayer = getLocalPlayer();
        if (localPlayer.selectedUIEdge != null)
        { // build a road
            UIEdge selectedUIEdge = localPlayer.selectedUIEdge;
            selectedUIEdge.ConstructRoad();
        }
        else if (localPlayer.selectedUIIntersection != null)
        {
            UIIntersection selectedUIIntersection = localPlayer.selectedUIIntersection;
            Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>
                (new Vec3[] { selectedUIIntersection.HexPos1, selectedUIIntersection.HexPos2, selectedUIIntersection.HexPos3 }));

            if (intersection.unit == null)
                selectedUIIntersection.CreateSettlement();
            else if (intersection.unit.GetType() == typeof(Knight))
                selectedUIIntersection.UpgradeKnight();
            else
                selectedUIIntersection.UpgradeSettlement();

        }

    }

    private GamePlayer getLocalPlayer()
    {
        return GameManager.LocalPlayer.GetComponent<GamePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED))
        {
            GetComponent<Button>().enabled = false;
            GetComponentInChildren<Text>().text = "CREATING MAP";
            return;
        }

        if (!GameManager.Instance.GetCurrentGameState().CurrentTurn.IsLocalPlayerTurn())
        {
            GetComponent<Button>().enabled = false;
            GetComponentInChildren<Text>().text = "NOT YOUR TURN";
            return;
        }

        GamePlayer localPlayer = getLocalPlayer();

        if (localPlayer.selectedUIEdge != null)
        {
            UIEdge selectedUIEdge = localPlayer.selectedUIEdge;
            Edge edge = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(selectedUIEdge.HexPos1, selectedUIEdge.HexPos2);

            if (edge.Owner != "")
            {
                GetComponent<Button>().enabled = false;
                GetComponentInChildren<Text>().text = "ROAD IS OWNED";
                return;
            }

            GetComponent<Button>().enabled = true;
            if (edge.IsShip())
            {
                GetComponentInChildren<Text>().text = "BUILD SHIP";
            }
            else
            {
                GetComponentInChildren<Text>().text = "BUILD ROAD";
            }
            return;
        }
        else if (localPlayer.selectedUIIntersection != null)
        {
            UIIntersection selectedUIIntersection = localPlayer.selectedUIIntersection;
            Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>
                (new Vec3[] { selectedUIIntersection.HexPos1, selectedUIIntersection.HexPos2, selectedUIIntersection.HexPos3 }));



            // if intersection is empty
            if (intersection.unit == null)
            {
                GetComponent<Button>().enabled = true;
                GetComponentInChildren<Text>().text = "BUILD SETTLEMENT";
                return;
            }
            else if (intersection.Owner != localPlayer.myName)
            {
                GetComponent<Button>().enabled = false;
                GetComponentInChildren<Text>().text = "INTERSECTION NOT OWNED";
                return;
            }
            else if (intersection.unit.GetType() == typeof(Knight))
            {
                GetComponent<Button>().enabled = true;
                GetComponentInChildren<Text>().text = "UPGRADE KNIGHT";
                return;
            }
            // if someone already owns the intersection
            else
            {
                GetComponent<Button>().enabled = true;
                GetComponentInChildren<Text>().text = "UPGRADE SETTLEMENT";
                return;

            }
        }
        else
        {
            GetComponent<Button>().enabled = false;
            GetComponentInChildren<Text>().text = "NO BUILD SELECTION";
            return;
        }


    }

}
