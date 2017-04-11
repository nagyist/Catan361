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
            StartCoroutine(GameManager.GUI.ShowMessage("Does not have enough resources."));
            return;
        }
        localPlayer.CmdConsumeResources(requiredRes);

        knight.active = true;
        StartCoroutine(GameManager.GUI.ShowMessage("You have activated your knight."));
        localPlayer.CmdActivateKnight(SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 }));


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
            StartCoroutine(GameManager.GUI.ShowMessage("Knight is already at its maximum level."));
            return;
        }

		if (!localPlayer.smithProgressCardDiscount) {
			// check resources
			Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int> () {
				{ StealableType.Resource_Ore, 1 },
				{ StealableType.Resource_Wool, 1 },
			};
			if (!localPlayer.HasEnoughResources (requiredRes)) {
				StartCoroutine (GameManager.GUI.ShowMessage ("Does not have enough resource to upgrade knight"));
				return;
			}

			localPlayer.CmdConsumeResources(requiredRes);
		} else {
			GameManager.GUI.PostStatusMessage ("You promoted this knight for free (smith progress card).");
			localPlayer.smithProgressCardUsed++;
			if (localPlayer.smithProgressCardUsed == 2) {
				localPlayer.smithProgressCardDiscount = false;
			}
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
        }

        
        StartCoroutine(GameManager.GUI.ShowMessage("You have upgrade your knight."));
        localPlayer.CmdUpgradeKnight(SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 }));

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

        // check for connection 
        if (!isConnectedToOwnedUnit())
        {
            StartCoroutine(GameManager.GUI.ShowMessage("Selected intersection must be connected by an owned road."));
            return;
        }

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
        localPlayer.CmdHireKnight(SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 }));

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

		if (!localPlayer.engineerProgressCardDiscount) {

			// check resources
			Dictionary<StealableType, int> requiredRes = new Dictionary<StealableType, int> () {
				{ StealableType.Resource_Brick, 2 },
			};
			if (!localPlayer.HasEnoughResources (requiredRes)) {
				Debug.Log ("Does not have enough resource to upgrade to city");
				return;
			}
			localPlayer.CmdConsumeResources (requiredRes);
		} else {
			GameManager.GUI.PostStatusMessage ("Engineer progress card used. City wall improvement was free.");
			localPlayer.engineerProgressCardDiscount = false;
		}


        StartCoroutine(GameManager.GUI.ShowMessage("You have placed a city wall."));
        localPlayer.CmdBuildCityWall(SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 }));

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

        if (!setup)
        {
            if (!isConnectedToOwnedUnit())
            {
                StartCoroutine(GameManager.GUI.ShowMessage("Selected intersection must be connected by an owned road."));
                return;
            }
            if (!distanceRuleCheck())
            {
                StartCoroutine(GameManager.GUI.ShowMessage("Adjacent intersections cannot own a settlement or city."));
                return;
            }
        }

        if (setup && localPlayer.placedSettlement)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("You have already placed a settlment."));
            return;
        }

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
        localPlayer.CmdBuildSettlement(SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 }));

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
       
		Dictionary<StealableType, int> requiredRes;
		bool discountUsed = false;
		if (!localPlayer.medicineProgressCardDiscount) {
			// cosume resources
			requiredRes = new Dictionary<StealableType, int> () {
				{ StealableType.Resource_Ore, 3 },
				{ StealableType.Resource_Grain, 2 }
			};
		} else {
			requiredRes = new Dictionary<StealableType, int> () {
				{ StealableType.Resource_Ore, 1 },
				{ StealableType.Resource_Grain, 2 }
			};
			discountUsed = true;
		}

        if (!localPlayer.HasEnoughResources(requiredRes))
        {
            StartCoroutine(GameManager.GUI.ShowMessage("You do not have enough resources."));
            return;
        }
        localPlayer.CmdConsumeResources(requiredRes);
    

        StartCoroutine(GameManager.GUI.ShowMessage("You have upgraded your settlement."));
		if (discountUsed) {
			GameManager.GUI.PostStatusMessage ("Medicine progress card used.");
			localPlayer.medicineProgressCardDiscount = false;
		}
        localPlayer.CmdUpgradeSettlement(SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2, HexPos3 }));

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

            if (i.unit == null)
            {
                intersectionIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("sprite_empty_selection");

                if (IsSelected)
                    intersectionIcon.GetComponent<SpriteRenderer>().color = Color.green;   
                else
                    intersectionIcon.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                
            }
            else
            {
                if (i.unit.GetType() == typeof(Knight))
                    intersectionIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("sprite_knight");
                else if (i.unit.GetType() == typeof(Village))
                    intersectionIcon.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("sprite_village");


                if (IsSelected)
                    intersectionIcon.GetComponent<SpriteRenderer>().color = Color.green;
                else
                    intersectionIcon.GetComponent<SpriteRenderer>().color = GameManager.ConnectedPlayersByName[i.Owner].GetComponent<GamePlayer>().GetPlayerColor();
            }   
        }
    }

    void OnMouseDown()
    {
        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();

        if (localPlayer.selectedUIIntersection == this)
            localPlayer.ResetBuildSelection();
        else
            localPlayer.SetBuildSelection(this);

        updateGUI();
    }


    void updateGUI()
    {
        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        GameObject selectionInfo = GameManager.GUI.guiCanvas.transform.FindChild("SelectionTooltip").gameObject;
        GameObject unitBtn1 = GameManager.GUI.guiCanvas.transform.FindChild("UnitButton1").gameObject;
        GameObject unitBtn2 = GameManager.GUI.guiCanvas.transform.FindChild("UnitButton2").gameObject;
        

        if (localPlayer.selectedUIIntersection == null)
        {
            selectionInfo.SetActive(false);
            unitBtn1.SetActive(false);
            unitBtn2.SetActive(false);
        }
        else
        {
            Intersection i = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));
            unitBtn1.SetActive(true);
            unitBtn2.SetActive(true);
            selectionInfo.SetActive(true);

        }
    }

    // checks if the intersection has an road owned by the local player connected to it
    public bool isConnectedToOwnedUnit()
    {
        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        Intersection i = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));
        Edge roadTest1 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(i.adjTile1, i.adjTile2);
        Edge roadTest2 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(i.adjTile1, i.adjTile3);
        Edge roadTest3 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(i.adjTile2, i.adjTile3);

        if (roadTest1.Owner == localPlayer.myName || roadTest2.Owner == localPlayer.myName || roadTest3.Owner == localPlayer.myName)
            return true;
        else
            return false;
    }

    public bool distanceRuleCheck()
    {
        // get the local player and the current intersection
        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        Intersection currentIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(new Vec3[] { HexPos1, HexPos2, HexPos3 }));

        // add all the connected edges to a list
        List<Edge> connectedEdges = new List<Edge>();
        connectedEdges.Add(GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(currentIntersection.adjTile1, currentIntersection.adjTile2));
        connectedEdges.Add(GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(currentIntersection.adjTile1, currentIntersection.adjTile3));
        connectedEdges.Add(GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(currentIntersection.adjTile2, currentIntersection.adjTile3));

        // loop through edges
        foreach (Edge e in connectedEdges)
        {
            List<List<Vec3>> adjHexIntersectionPos1 = UIHex.getIntersectionsAdjacentPos(this.HexPos1);
            List<List<Vec3>> adjHexIntersectionsPos2 = UIHex.getIntersectionsAdjacentPos(this.HexPos2);
            List<Intersection> intersectionBuffer = new List<Intersection>();
            List<Intersection> adjIntersections = new List<Intersection>();

            // add the intersections of the first adjacent hex to a buffer list
            // this excludes the current intersection
            foreach (List<Vec3> hexIntersectionPos in adjHexIntersectionPos1)
            {
                Intersection hexIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(hexIntersectionPos);

                if(!hexIntersection.Equals(currentIntersection))
                    intersectionBuffer.Add(hexIntersection);
            }

            // add the intersections of the second adjacent hex to the final list if the buffer contains them
            // this excludes the current intersection
            foreach (List<Vec3> hexIntersectionPos in adjHexIntersectionsPos2)
            {
                Intersection hexIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(hexIntersectionPos);

                if (!hexIntersection.Equals(currentIntersection))
                    if (intersectionBuffer.Contains(hexIntersection))
                        adjIntersections.Add(hexIntersection);
            }

            // check for a settlement in the adjacent intersections 
            foreach (Intersection testIntersection in adjIntersections)
            {
                if (testIntersection.unit != null)
                    if (testIntersection.unit.GetType() == typeof(Village))
                            return false;
            }
        }
        return true;
    }
}
