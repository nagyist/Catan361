using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMoveButton : MonoBehaviour {

	bool inUse = false;

    string localPlayerName;

    public void ClickBuild()
    {
        GamePlayer localPlayer = getLocalPlayer();
        UIIntersection newUIIntersection = localPlayer.selectedUIIntersection;
        Vec3[] newPos = new Vec3[] { newUIIntersection.HexPos1, newUIIntersection.HexPos2, newUIIntersection.HexPos3 };
        Intersection newIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(newPos));
        
        if (localPlayer.knightsToMove.Count != 0)
        {
            // player must move his knight first 
            if (newUIIntersection == null)
            {
                StartCoroutine(GameManager.GUI.ShowMessage("Please select an intersection to move to."));
                return;
            }
            else if (newIntersection.Owner != localPlayer.myName)
            {
                StartCoroutine(GameManager.GUI.ShowMessage("You do not own that intersection."));
                return;
            }
            else if (newIntersection.unit != null)
            {
                StartCoroutine(GameManager.GUI.ShowMessage("This intersection already contains a unit."));
                return;
            }
            
            // call command to replace knight and reset the button
            localPlayer.CmdReplaceKnight();
            StartCoroutine(GameManager.GUI.ShowMessage("You have repositioned your knight."));
            inUse = false;
            return;

        }
        // if the local player hasn't selected a unit to move
        else if (!inUse)
        {
            if (newIntersection.unit == null)
            {
                StartCoroutine(GameManager.GUI.ShowMessage("Knight must be selected for movement."));
                return;
            }
            else if (newIntersection.unit.GetType() != typeof(Knight))
            {
                StartCoroutine(GameManager.GUI.ShowMessage("Knight must be selected for movement."));
                return;
            }
            localPlayer.SetMoveSelection();
            StartCoroutine(GameManager.GUI.ShowMessage("Set the unit to move."));
            inUse = true;
        }
        // if the local player has selected a unit 
        else
        {
            UIIntersection oldUIIntersection = localPlayer.uiIntersectionToMove;
            Vec3[] oldPos = new Vec3[] { oldUIIntersection.HexPos1, oldUIIntersection.HexPos2, oldUIIntersection.HexPos3 };
            Intersection oldIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(oldPos));

            if (!newUIIntersection.isConnectedToOwnedUnit())
            {
                StartCoroutine(GameManager.GUI.ShowMessage("Selected intersection must be connected by an owned road."));
                localPlayer.ResetMoveSelection();
                localPlayer.ResetBuildSelection();
                inUse = false;
                return;
            }
            if (newIntersection.Owner != "")
            {
                if (newIntersection.Owner == localPlayer.myName)
                {
                    StartCoroutine(GameManager.GUI.ShowMessage("You already own a unit at the selected location."));
                    localPlayer.ResetMoveSelection();
                    localPlayer.ResetBuildSelection();
                    inUse = false;
                    return;
                }
                else if (newIntersection.unit != null)
                {
                    if (newIntersection.unit.GetType() == typeof(Knight))
                    {
                        
                        Knight replaced = (Knight)newIntersection.unit;
                        GamePlayer intersectionOwner = GameManager.ConnectedPlayersByName[newIntersection.Owner].GetComponent<GamePlayer>();
                        intersectionOwner.knightsToMove.Enqueue(replaced);

                        localPlayer.CmdMoveUnit(SerializationUtils.ObjectToByteArray(oldPos), SerializationUtils.ObjectToByteArray(newPos));
                        StartCoroutine(GameManager.GUI.ShowMessage("Successfully moved the unit."));
                        localPlayer.ResetMoveSelection();
                        localPlayer.ResetBuildSelection();
                        inUse = false;
                        return;
                    }
                    else
                    {
                        StartCoroutine(GameManager.GUI.ShowMessage("Intersection contains a non-knight unit."));
                        localPlayer.ResetMoveSelection();
                        localPlayer.ResetBuildSelection();
                        inUse = false;
                        return;
                    }
                }
            }
            else
            {
                localPlayer.CmdMoveUnit(SerializationUtils.ObjectToByteArray(oldPos), SerializationUtils.ObjectToByteArray(newPos));
                StartCoroutine(GameManager.GUI.ShowMessage("Successfully moved the unit."));
                localPlayer.ResetMoveSelection();
                localPlayer.ResetBuildSelection();
                inUse = false;
                return;
            }
        }
    }

    private GamePlayer getLocalPlayer()
    {
        return GameManager.LocalPlayer.GetComponent<GamePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        GamePlayer localPlayer = getLocalPlayer();
        if (localPlayer.knightsToMove.Count != 0)
        {
            GetComponent<Button>().enabled = true;
            GetComponentInChildren<Text>().text = "REPOSITION KNIGHT";
            return;
            // player must move his knight first 
        }
        else if (inUse)
        {
            GetComponent<Button>().enabled = true;
            GetComponentInChildren<Text>().text = "CHOOSE DESTINATION";
            return;
        }
        else
        {
            if (!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED))
            {
                GetComponent<Button>().enabled = false;
                GetComponentInChildren<Text>().text = "SETTING UP GAME";
                return;
            }

            if (!GameManager.Instance.GetCurrentGameState().CurrentTurn.IsLocalPlayerTurn())
            {
                GetComponent<Button>().enabled = false;
                GetComponentInChildren<Text>().text = "NOT YOUR TURN";
                return;
            }
            if (localPlayer.selectedUIIntersection != null)
            {
                UIIntersection selectedUIIntersection = localPlayer.selectedUIIntersection;
                Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>
                    (new Vec3[] { selectedUIIntersection.HexPos1, selectedUIIntersection.HexPos2, selectedUIIntersection.HexPos3 }));

                if (intersection.unit == null)
                {
                    GetComponent<Button>().enabled = false;
                    GetComponentInChildren<Text>().text = "EMPTY INTERSECTION";
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
                    GetComponentInChildren<Text>().text = "MOVE KNIGHT";
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
        
}
