using System;
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
    public bool IsSelected = false;

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

    public bool isEmpty()
    {
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));
        if (intersection.unit == null)
            return true;
        else
            return false;
    }

    public bool CanBuild()
    {
        // check if the grid hasn't been created yet
        if (!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED))
        {
            StartCoroutine(GameManager.GUI.ShowMessage("Please wait until the grid is created."));
            return false;
        }
        // check for local player's turn
        else if (!GameManager.Instance.GetCurrentGameState().CurrentTurn.IsLocalPlayerTurn())
        {
            StartCoroutine(GameManager.GUI.ShowMessage("It is not your turn"));
            return false;
        }
        else
            return true;
    }
    public bool IsLocalPlayerMainPhase()
    {

        // check if the grid hasn't been created yet
        if (!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED))
        {
            StartCoroutine(GameManager.GUI.ShowMessage("Please wait until the grid is created."));
            return false;
        }
        // check if we're still in the setup phase
        else if (GameManager.Instance.GetCurrentGameState().CurrentTurn.IsInSetupPhase())
        {
            StartCoroutine(GameManager.GUI.ShowMessage("Still in setup phase."));
            return false;
        }
        // check for local player's turn
        else if (!GameManager.Instance.GetCurrentGameState().CurrentTurn.IsLocalPlayerTurn())
        {
            StartCoroutine(GameManager.GUI.ShowMessage("It is not your turn"));
            return false;
        }
        else
            return true;
    }

    public bool OwnsKnight()
    {
        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        string localPlayerName = localPlayer.myName;
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));

        // check for empty intersection
        if (intersection.unit == null)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("Intersection is empty"));
            return false;
        }
        // check for invalid owner
        if (intersection.Owner != localPlayerName)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("You do not own the intersection"));
            return false;
        }
        // check for non-village unit
        if (intersection.unit.GetType() != typeof(Knight))
        {
            StartCoroutine(GameManager.GUI.ShowMessage("This intersection does not contain the right unit."));
            return false;
        }
        return true;
    }

    public bool OwnsVillage()
    {
        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        string localPlayerName = localPlayer.myName;
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));

        // check for empty intersection
        if (intersection.unit == null)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("Intersection is empty"));
            return false;
        }
        // check for invalid owner
        if (intersection.Owner != localPlayerName)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("You do not own the intersection"));
            return false;
        }
        // check for non-village unit
        if (intersection.unit.GetType() != typeof(Village))
        {
            StartCoroutine(GameManager.GUI.ShowMessage("This intersection does not contain the right unit."));
            return false;
        }
        return true;
    }

    void OnMouseExit()
    {
        GameManager.GUI.GetTooltip("IntersectionTooltip").GetComponent<UIWindow>().Hide();
    }

    public void ActivateKnight()
    {

        if (!IsLocalPlayerMainPhase())
            return;

        if (!OwnsKnight())
            return;

        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        string localPlayerName = localPlayer.myName;
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));

        

        Knight knight = (Knight)intersection.unit;
        if (knight.active)
        {
            Debug.Log("Knight was already active.");
            StartCoroutine(GameManager.GUI.ShowMessage("Knight is already active"));
            return;
        }

        // check resources
        Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int>() {
            {StealableType.Resource_Grain, 1},
        };
        if (!localPlayer.HasEnoughResources(requiredRes))
        {
            Debug.Log("Does not have enough resource to upgrade knight");
            StartCoroutine(GameManager.GUI.ShowMessage("Does not have enough resource to upgrade knight"));
            return;
        }
        localPlayer.CmdConsumeResources(requiredRes);
        
        knight.active = true;
        StartCoroutine(GameManager.GUI.ShowMessage("You have activated your knight."));
        GameManager.LocalPlayer.GetComponent<GamePlayer>().CmdActivateKnight(
            SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 })
        );

        
        return;
    }


    public void UpgradeKnight()
    {

        if (!IsLocalPlayerMainPhase())
            return;

        if (!OwnsKnight())
            return;

        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        string localPlayerName = localPlayer.myName;
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));
        Knight knight = (Knight)intersection.unit;

        if (knight.hasBeenPromotedThisTurn)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("Knight has already been promoted this turn."));
            return;
        }
        // check for max level 
        else if (knight.level == 3)
        {
            Debug.Log("Knight already at max level.");
            StartCoroutine(GameManager.GUI.ShowMessage("Knight is already at its maximum level."));
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

        if (knight.level == 2)
        {
            if (!localPlayer.hasFortress)
            {
                StartCoroutine(GameManager.GUI.ShowMessage("You do not own a fortress"));
                return;
            }
            if (localPlayer.numMightyKnights >= 2)
            {
                StartCoroutine(GameManager.GUI.ShowMessage("LocalPlayer already has maximum number of might knights."));
                return;
            }
            // update local player's number of knights
            localPlayer.numStrongKnights--;
            localPlayer.numMightyKnights++;
            localPlayer.CmdConsumeResources(requiredRes);
        }
        else if (knight.level == 1)
        {
            // check for max number of strong knights
            if (localPlayer.numStrongKnights >= 2)
            {
                StartCoroutine(GameManager.GUI.ShowMessage("LocalPlayer already has maximum number of strong knights."));
                return;
            }
            // update local player's number of knights
            localPlayer.numBasicKnights--;
            localPlayer.numStrongKnights++;
            localPlayer.CmdConsumeResources(requiredRes);
        }
        localPlayer.CmdConsumeResources(requiredRes);
        GameManager.LocalPlayer.GetComponent<GamePlayer>().CmdUpgradeKnight(
            SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 })
        );

        StartCoroutine(GameManager.GUI.ShowMessage("You have upgrade your knight."));
        return;

    }

    public void HireKnight()
    {
        if (!IsLocalPlayerMainPhase())
            return;

        if (!isEmpty())
            return;

        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        string localPlayerName = localPlayer.myName;
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));


        // check for max number of basic knights 
        if (localPlayer.numBasicKnights >= 3)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("You already have the maximum number of basic knights"));
            return;
        }
        // check if local player has already placed knight 
        if (localPlayer.placedKnight)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("You already placed a knight during your turn"));
            return;
        }

        // check resources
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
        localPlayer.CmdConsumeResources(requiredRes);
        
        
        StartCoroutine(GameManager.GUI.ShowMessage("You have placed a knight."));
        GameManager.LocalPlayer.GetComponent<GamePlayer>().CmdHireKnight(
            SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 })
        );
        
        return;
    }

    public void BuildCityWall()
    {
        if (!IsLocalPlayerMainPhase())
            return;

        if (!OwnsVillage())
            return;

        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        string localPlayerName = localPlayer.myName;
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));
        Village village = (Village)intersection.unit;

        // check for max number of walls 
        if (localPlayer.numCityWalls >= 3)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("You already have 3 city walls."));
            return;
        }
        else if (village.myKind == Village.VillageKind.Settlement)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("Settlements cannot have a city wall."));
            return;
        }
        else if (village.cityWall)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("City already has a city wall."));
            return;
        }
        

        // check resources
        Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int>() {
                {StealableType.Resource_Brick, 2},
            };
        if (!localPlayer.HasEnoughResources(requiredRes))
        {
            Debug.Log("Does not have enough resource to upgrade to city");
            return;
        }
        localPlayer.CmdConsumeResources(requiredRes);

        
        StartCoroutine(GameManager.GUI.ShowMessage("You have placed a city wall."));
        GameManager.LocalPlayer.GetComponent<GamePlayer>().CmdBuildCityWall(
            SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 })
        );

        return;
        
    }

    public void CreateSettlement()
    {

        if (!CanBuild())
            return;

        if (!isEmpty())
            return;

        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));
        bool setup = GameManager.Instance.GetCurrentGameState().CurrentTurn.IsInSetupPhase();
        int roundCount = GameManager.Instance.GetCurrentGameState().CurrentTurn.RoundCount;

        // only consume resources if not in setup phase
        if (!setup)
        {
            Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int>() {
                        {StealableType.Resource_Brick, 1},
                        {StealableType.Resource_Lumber, 1},
                        {StealableType.Resource_Wool, 1},
                        {StealableType.Resource_Grain, 1}
                    };

            if (!localPlayer.HasEnoughResources(requiredRes))
            {
                StartCoroutine(GameManager.GUI.ShowMessage("Not enough resources."));
                return;
            }
            localPlayer.CmdConsumeResources(requiredRes);
        }

        if (roundCount == 1)
            StartCoroutine(GameManager.GUI.ShowMessage("You have placed a city."));
        else
            StartCoroutine(GameManager.GUI.ShowMessage("You have placed a settlement."));

        // update the intersection
        GameManager.LocalPlayer.GetComponent<GamePlayer>().CmdBuildSettlement(
            SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 })
        );

        return;
    }

    public void UpgradeSettlement()
    {
        if (!IsLocalPlayerMainPhase())
            return;

        if (!OwnsVillage())
            return;

        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        Intersection intersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));
        bool setup = GameManager.Instance.GetCurrentGameState().CurrentTurn.IsInSetupPhase();
        int roundCount = GameManager.Instance.GetCurrentGameState().CurrentTurn.RoundCount;


        Village village = (Village)intersection.unit;
        if (village.myKind != Village.VillageKind.Settlement)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("Only settlements can be upgraded to a city."));
            return;
        }

        // check for setup phase
        if (!setup)
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

            localPlayer.CmdConsumeResources(requiredRes);
        }

        StartCoroutine(GameManager.GUI.ShowMessage("You have placed a settlement."));
        GameManager.LocalPlayer.GetComponent<GamePlayer>().CmdUpgradeSettlement(
            SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 })
        );

        return;
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
            if (IsSelected)
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
        localPlayer.SetBuildSelection(this);
        return;
    }
}
