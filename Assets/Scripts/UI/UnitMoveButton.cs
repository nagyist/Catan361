using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMoveButton : MonoBehaviour {

	bool inUse = false;

    string localPlayerName;

    public void ClickBuild()
    {
        GamePlayer localPlayer = getLocalPlayer();
        UIIntersection selection = localPlayer.selectedUIIntersection;
        Intersection target = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { selection.HexPos1, selection.HexPos2, selection.HexPos3 }));

        if (!inUse)
        {
            if (target.unit == null)
            {
                StartCoroutine(GameManager.GUI.ShowMessage("Knight must be selected for movement."));
                return;
            }
            else if (target.unit.GetType() != typeof(Knight))
            {
                StartCoroutine(GameManager.GUI.ShowMessage("Knight must be selected for movement."));
                return;
            }
            localPlayer.SetMoveSelection();
            StartCoroutine(GameManager.GUI.ShowMessage("Set the unit to move."));
            inUse = true;
        }
        else
        {
            if (!selection.isConnectedToOwnedUnit())
            {
                StartCoroutine(GameManager.GUI.ShowMessage("Selected intersection must be connected by an owned road."));
                localPlayer.ResetMoveSelection();
                localPlayer.resetBuildSelection();
                inUse = false;
                return;
            }
            if (target.unit != null)
            {
                if (target.unit.GetType() == typeof(Village))
                {
                    StartCoroutine(GameManager.GUI.ShowMessage("Invalid location: selected intersection contains a village."));
                    localPlayer.ResetMoveSelection();
                    localPlayer.resetBuildSelection();
                    inUse = false;
                    return;
                }

                // exits because knight displacement has not been implemented yet
                StartCoroutine(GameManager.GUI.ShowMessage("Unit displacement is not implemented yet."));
                localPlayer.ResetMoveSelection();
                localPlayer.resetBuildSelection();
                inUse = false;
                return;
            }
            localPlayer.CmdMoveUnit();
            StartCoroutine(GameManager.GUI.ShowMessage("Successfully moved the unit."));
            localPlayer.ResetMoveSelection();
            localPlayer.resetBuildSelection();
            inUse = false;
        }
    }

    private GamePlayer getLocalPlayer()
    {
        return GameManager.LocalPlayer.GetComponent<GamePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inUse)
        {
            GetComponent<Button>().enabled = true;
            GetComponentInChildren<Text>().text = "CHOOSE DESTINATION";
            return;
        }
        if (!inUse)
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
