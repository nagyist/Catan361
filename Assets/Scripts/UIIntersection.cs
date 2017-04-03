using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIIntersection : MonoBehaviour
{
	public bool canAccessHarbour { get; set; }
    public Vec3 HexPos1;
    public Vec3 HexPos2;
    public Vec3 HexPos3;
    private GameObject intersectionIcon;
    bool selected = false;

    void OnMouseEnter()
    {
        // check for created grid
        if (GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED))
        {
            Intersection refIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));

            if (refIntersection.unit != null)
            {

                GameObject tooltipObj = GameManager.GUI.GetTooltip("IntersectionTooltip");
                tooltipObj.GetComponent<IntersectionTooltip>().ReferencedIntersection = refIntersection;
                tooltipObj.GetComponent<UIWindow>().Show();

            }
        }

        GetComponent<SpriteRenderer>().color = Color.blue;
    }

    void OnMouseExit()
    {
        GameManager.GUI.GetTooltip("IntersectionTooltip").GetComponent<UIWindow>().Hide();

    }

    public void UpgradeKnight()
    {
        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        string localPlayerName = localPlayer.myName;
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));

        // check if the grid hasn't been created yet
        if (!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED))
        {
            Debug.Log("Grid not created");
            return;
        }
        // check if we're still in the setup phase
        if (GameManager.Instance.GetCurrentGameState().CurrentTurn.IsInSetupPhase())
        {
            StartCoroutine(GameManager.GUI.ShowMessage("Can't upgrade knight in setup phase."));
            return;
        }
        // check for local player's turn
        if (!GameManager.Instance.GetCurrentGameState().CurrentTurn.IsLocalPlayerTurn())
        {
            StartCoroutine(GameManager.GUI.ShowMessage("It is not your turn"));
            return;
        }
        // check for empty intersection
        if (intersection.unit == null)
        {
            Debug.Log("Intersection is empty");
            StartCoroutine(GameManager.GUI.ShowMessage("Intersection is empty"));
            return;
        }
        // check for invalid owner
        if (intersection.Owner != localPlayerName)
        {
            Debug.Log("Does not own the intersection.");
            StartCoroutine(GameManager.GUI.ShowMessage("You do not own the intersection"));
            return;
        }
        // check for non-village unit
        if (intersection.unit.GetType() != typeof(Knight))
        {
            Debug.Log("The intersection contains a village.");
            StartCoroutine(GameManager.GUI.ShowMessage("This intersection contains a non-knight unit."));
            return;
        }

        // check resources
        Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int>() {
            {StealableType.Resource_Ore, 1},
            {StealableType.Resource_Wool, 1},
        };
        if (!localPlayer.HasEnoughResources(requiredRes))
        {
            Debug.Log("Does not have enough resource to upgrade knight");
            StartCoroutine(GameManager.GUI.ShowMessage("Does not have enough resource to upgrade knight"));
            return;
        }

        Knight knight = (Knight)intersection.unit;

        // check for max level 
        if (knight.level == 3)
        {
            Debug.Log("Knight already at max level.");
            StartCoroutine(GameManager.GUI.ShowMessage("Knight is already at its maximum level"));
            return;
        }
        // if we have a strong knight 
        else
        {
            if (knight.level == 2)
            {
                if (!localPlayer.hasFortress)
                {
                    StartCoroutine(GameManager.GUI.ShowMessage("You do not own a fortress"));
                    return;
                }
                if (localPlayer.numMightyKnights >= 2)
                {
                    Debug.Log("LocalPlayer already has maximum number of strong knights.");
                    StartCoroutine(GameManager.GUI.ShowMessage("LocalPlayer already has maximum number of might knights."));
                    return;
                }
                // update local player's number of knights
                localPlayer.numStrongKnights--;
                localPlayer.numMightyKnights++;
            }
            else if (knight.level == 1)
            {
                // check for max number of strong knights
                if (localPlayer.numStrongKnights >= 2)
                {
                    Debug.Log("LocalPlayer already has maximum number of strong knights.");
                    StartCoroutine(GameManager.GUI.ShowMessage("LocalPlayer already has maximum number of strong knights."));
                    return;
                }
                // update local player's number of knights
                localPlayer.numBasicKnights--;
                localPlayer.numStrongKnights++;
            }
            
            // consume resources
            localPlayer.ConsumeResources(requiredRes);

            GameManager.LocalPlayer.GetComponent<GamePlayer>().CmdUpgradeKnight(
                SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 })
            );

            StartCoroutine(GameManager.GUI.ShowMessage("You have upgrade your knight."));
            // reset the intersection selection
            localPlayer.selectedUIIntersection = null;
            selected = false;

            return;
        }
    }

    public void HireKnight()
    {
        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        string localPlayerName = localPlayer.myName;
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));

        // check that the grid has been created
        if (!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED))
        {
            Debug.Log("Grid not created");
            return;
        }
        // check for setup phase
        if (GameManager.Instance.GetCurrentGameState().CurrentTurn.IsInSetupPhase())
        {
            StartCoroutine(GameManager.GUI.ShowMessage("Can't place knight in setup phase."));
            return;
        }
        // check for local player turn
        if (!GameManager.Instance.GetCurrentGameState().CurrentTurn.IsLocalPlayerTurn())
        {
            Debug.Log("Is not local player turn");
            StartCoroutine(GameManager.GUI.ShowMessage("It is not your turn"));
            return;
        }
        // check for non-empty intersection
        if (intersection.unit != null)
        {
            Debug.Log("Intersection is not empty");
            StartCoroutine(GameManager.GUI.ShowMessage("Intersection is not empty"));
            return;
        }
        // check for max number of basic knights 
        if (localPlayer.numBasicKnights >= 2)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("You already have the maximum number of basic knights"));
            return;
        }
        // check if local player has already placed knight 
        if (localPlayer.placedKnight)
        {
            Debug.Log("You already placed a knight during your turn");
            StartCoroutine(GameManager.GUI.ShowMessage("You already placed a knight during your turn"));
            return;
        }

        // consume resources
        Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int>() {
            {StealableType.Resource_Ore, 1},
            {StealableType.Resource_Wool, 1},
        };
        if (!localPlayer.HasEnoughResources(requiredRes))
        {
            Debug.Log("Not enough resources");
            StartCoroutine(GameManager.GUI.ShowMessage("Not enough resources."));
            return;
        }
        localPlayer.ConsumeResources(requiredRes);


        // here we're supposed to call the cmd function
        // not created yet
        GameManager.LocalPlayer.GetComponent<GamePlayer>().CmdHireKnight(
            SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 })
        );


        StartCoroutine(GameManager.GUI.ShowMessage("You have placed a knight."));
        // increment local player's number of basic knights
        localPlayer.numBasicKnights++;
        // local player has now placed knight
        localPlayer.placedKnight = true;
        // reset intersection selection
        localPlayer.selectedUIIntersection = null;
        selected = false;

        return;
    }

    public void CreateSettlement()
    {
        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        string localPlayerName = localPlayer.myName;
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));

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

        if (GameManager.Instance.GetCurrentGameState().CurrentTurn.IsInSetupPhase() && localPlayer.placedSettlement)
        {
            Debug.Log("You already placed a settlement during your turn");
            return;
        }

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

                    StartCoroutine(GameManager.GUI.ShowMessage("You have placed a settlement."));
                    // local player has now placed settlement
                    localPlayer.placedSettlement = true;
                    localPlayer.selectedUIIntersection = null;
                    selected = false;

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
            localPlayer.selectedUIIntersection = null;
            selected = false;

            return;
        }

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

            // show yellow if selected
            if (selected)
            {
                intersectionIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ore_f_b_03");
                intersectionIcon.GetComponent<SpriteRenderer>().color = Color.yellow;
                return;
            }


            // set to player color if there is a unit there
            if (i.unit != null)
            {
                if (i.unit.GetType() == typeof(Knight))
                    intersectionIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("cobbler's_hammer_b");
                else if (i.unit.GetType() == typeof(Village))
                    intersectionIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ore_f_b_03");

                intersectionIcon.GetComponent<SpriteRenderer>().color = GameManager.ConnectedPlayersByName[i.Owner].GetComponent<GamePlayer>().GetPlayerColor();
            }
            // set to black if there are no units 
            else
            {
                intersectionIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ore_f_b_03");
                intersectionIcon.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            }
        }
    }

    void OnMouseDown()
    {

        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();

        // if player had no previous selection
        if (localPlayer.selectedUIIntersection == null)
        {
            localPlayer.selectedUIIntersection = this;
            selected = true;
        }
        // if the player had a previous selection
        else
        {
            // if this is the player's previous selection
            if (selected)
            {
                selected = false;
                localPlayer.selectedUIIntersection = null;
            }
            // otherwise
            else
            {
                localPlayer.selectedUIIntersection.selected = false;
                localPlayer.selectedUIIntersection = this;
                selected = true;
            }
        }

        return;
    }
}
