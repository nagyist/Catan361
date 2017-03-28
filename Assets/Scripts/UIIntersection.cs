using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIIntersection : MonoBehaviour
{

    public Vec3 HexPos1;
    public Vec3 HexPos2;
    public Vec3 HexPos3;

    private GameObject intersectionIcon;

    void OnMouseEnter()
    {
        if (GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED))
        {
            Intersection refIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));

            if (refIntersection.unit != null)
            {
                if (refIntersection.unit.GetType() == typeof(Village))
                {
                    GameObject tooltipObj = GameManager.GUI.GetTooltip("IntersectionTooltip");
                    tooltipObj.GetComponent<IntersectionTooltip>().ReferencedIntersection = refIntersection;
                    tooltipObj.GetComponent<UIWindow>().Show();
                }
            }
        }
        GetComponent<SpriteRenderer>().color = Color.blue;
    }

    void OnMouseExit()
    {
        GameManager.GUI.GetTooltip("IntersectionTooltip").GetComponent<UIWindow>().Hide();
    }

    void OnMouseDown()
    {
        if (!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED))
        {
            Debug.Log("Grid not created");
            return;
        }

        if (!GameManager.Instance.GetCurrentGameState().CurrentTurn.IsLocalPlayerTurn())
        {
            Debug.Log("Is not local player turn");
            return;
        }

        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        if (GameManager.Instance.GetCurrentGameState().CurrentTurn.IsInSetupPhase() && localPlayer.placedSettlement)
        {
            Debug.Log("You already placed a settlement during your turn");
            return;
        }

        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));
        string localPlayerName = GameManager.LocalPlayer.GetComponent<GamePlayer>().myName;


        // check to see if there's currently a unit on the intersection
        if (intersection.unit != null)
        {
            if (intersection.Owner != localPlayerName)
            {
                Debug.Log("Does not own the intersection.");
                return;
            }

            // check for non-village unit
            if (intersection.unit.GetType() != typeof(Village))
            {
                Debug.Log("The intersection contains a knight.");
                return;
            }
            // if there is a village here
            else
            {
                Village village = (Village)intersection.unit;

                // if the player's village is a settlement
                if (village.myKind == Village.VillageKind.Settlement)
                {
                    // check for setup phase
                    if (!GameManager.Instance.GetCurrentGameState().CurrentTurn.IsInSetupPhase())
                    {
                        // upgrade the settlement to city
                        Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int>() {
                            {StealableType.Resource_Ore, 3},
                            {StealableType.Resource_Grain, 2}
                        };

                        if (!localPlayer.HasEnoughResources(requiredRes))
                        {
                            Debug.Log("Does not have enough resource to upgrade to city");
                            return;
                        }

                        localPlayer.ConsumeResources(requiredRes);
                    }

                   
                    GameManager.LocalPlayer.GetComponent<GamePlayer>().CmdUpgradeSettlement(
                        SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 })
                    );

                    // local player has now placed settlement
                    localPlayer.placedSettlement = true;

                    return;
                }
            }

        }
        // if the intersection was empty 
        else
        {
            // only consume resources if not in setup phase
            if (!GameManager.Instance.GetCurrentGameState().CurrentTurn.IsInSetupPhase())
            {
                Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int>() {
                        {StealableType.Resource_Brick, 1},
                        {StealableType.Resource_Lumber, 1},
                        {StealableType.Resource_Wool, 1},
                        {StealableType.Resource_Grain, 1}
                    };

                if (!localPlayer.HasEnoughResources(requiredRes))
                {
                    Debug.Log("Not enough resources");
                    return;
                }

                localPlayer.ConsumeResources(requiredRes);
            }

            localPlayer.placedSettlement = true;

            Debug.Log("Created the settlement");
            GameManager.LocalPlayer.GetComponent<GamePlayer>().CmdBuildSettlement(
                SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 })
            );

            // local player has now placed settlement
            localPlayer.placedSettlement = true;

            return;
        }

        // jeremy's old code, changed it to work with new intersection class
        /*
        if (intersection.SettlementLevel > 0 && intersection.Owner != GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName) {
            Debug.Log ("Does not own the settlement");
            return;
        } else if (intersection.SettlementLevel == 1 && intersection.Owner == GameManager.LocalPlayer.GetComponent<GamePlayer> ().myName) {
            // upgrade the settlement to city
            Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int> () {
                {StealableType.Resource_Ore, 3},
                {StealableType.Resource_Grain, 2}
            };

            if (!localPlayer.HasEnoughResources (requiredRes)) {
                Debug.Log ("Does not have enough resource to upgrade to city");
                return;
            }

            localPlayer.ConsumeResources (requiredRes);
            GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdUpgradeSettlement (
                SerializationUtils.ObjectToByteArray (new Vec3[] { HexPos1, HexPos2, HexPos3 })
            );
            return;
        }

        if (!GameManager.Instance.GetCurrentGameState ().CurrentTurn.IsInSetupPhase ()) {
            Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int> () {
                {StealableType.Resource_Brick, 1},
                {StealableType.Resource_Lumber, 1},
                {StealableType.Resource_Wool, 1},
                {StealableType.Resource_Grain, 1}
            };

            if (!localPlayer.HasEnoughResources (requiredRes)) {
                Debug.Log ("Not enough resources");
                return;
            }

            localPlayer.ConsumeResources (requiredRes);
        }

        localPlayer.placedSettlement = true;

        Debug.Log ("Created the settlement");
        GameManager.LocalPlayer.GetComponent<GamePlayer> ().CmdBuildSettlement (
            SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 })
        );
        */

    }



    // Use this for initialization
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingLayerName = "intersection";
        intersectionIcon = transform.FindChild("IntersectionSlot").FindChild("IntersectionIcon").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED))
        {
            Intersection i = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));
            if (i.unit != null)
            {
                intersectionIcon.GetComponent<SpriteRenderer>().color = GameManager.ConnectedPlayersByName[i.Owner].GetComponent<GamePlayer>().GetPlayerColor();
            }
            else
            {
                intersectionIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ore_f_b_03");
                intersectionIcon.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            }
        }
    }
}
