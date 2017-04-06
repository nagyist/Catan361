using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*  Unit button 2 is used for:
 *      hiring knights on empty intersections
 *      activating knights
 *      adding city walls 
 */

public class UnitButton2 : MonoBehaviour {

    string localPlayerName;

    public void ClickBuild()
    {
        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        string localPlayerName = localPlayer.myName;
        UIIntersection selectedUIIntersection = localPlayer.selectedUIIntersection;

        // call the upgrade or hire function depending on the level of the knight
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>
                (new Vec3[] { selectedUIIntersection.HexPos1, selectedUIIntersection.HexPos2, selectedUIIntersection.HexPos3 }));

        if (intersection.unit == null)
            selectedUIIntersection.HireKnight();
        else if (intersection.unit.GetType() == typeof(Knight))
            selectedUIIntersection.ActivateKnight();
        else if (intersection.unit.GetType() == typeof(Village))
            selectedUIIntersection.BuildCityWall();

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
            return;
        }

        if (!GameManager.Instance.GetCurrentGameState().CurrentTurn.IsLocalPlayerTurn())
        {
            GetComponent<Button>().enabled = false;
            GetComponentInChildren<Text>().text = "NOT YOUR TURN";
            return;
        }

        GamePlayer localPlayer = getLocalPlayer();
        localPlayerName = localPlayer.myName;

        if (localPlayer.selectedUIIntersection != null)
        {
            UIIntersection selectedUIIntersection = localPlayer.selectedUIIntersection;
            Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>
                (new Vec3[] { selectedUIIntersection.HexPos1, selectedUIIntersection.HexPos2, selectedUIIntersection.HexPos3 }));

            // if intersection is empty
            if (intersection.unit == null)
            { 
                GetComponent<Button>().enabled = true;
                GetComponentInChildren<Text>().text = "HIRE KNIGHT";
                return;
            }
            else if (intersection.unit != null && intersection.Owner != localPlayerName)
            {
                GetComponent<Button>().enabled = false;
                GetComponentInChildren<Text>().text = "INTERSECTION NOT OWNED";
                return;
            }
            // if someone already owns the intersection
            else if (intersection.unit.GetType() == typeof(Village))
            {
                Village v = (Village)intersection.unit;
                if (v.myKind == Village.VillageKind.Settlement)
                {
                    GetComponent<Button>().enabled = false;
                    GetComponentInChildren<Text>().text = "SETTLEMENT SELECTED";
                    return;
                }
                else if (!v.cityWall)
                {
                    GetComponent<Button>().enabled = true;
                    GetComponentInChildren<Text>().text = "ADD CITY WALL";
                    return;
                }
                else
                {
                    GetComponent<Button>().enabled = false;
                    GetComponentInChildren<Text>().text = "UNIT ALREADY HAS CITY WALL";
                    return;
                }
            }
            else if (intersection.unit.GetType() == typeof(Knight))
            {
                Knight k = (Knight)intersection.unit;
                if (k.active)
                {
                    GetComponent<Button>().enabled = false;
                    GetComponentInChildren<Text>().text = "KNIGHT IS ACTIVE";
                    return;
                }
                else
                {
                    GetComponent<Button>().enabled = true;
                    GetComponentInChildren<Text>().text = "ACTIVATE KNIGHT";
                    return;
                }
            }
        }
        else
        {
            GetComponent<Button>().enabled = false;
            GetComponentInChildren<Text>().text = "NO INTERSECTION SELECTED";
            return;
        }


    }
}
