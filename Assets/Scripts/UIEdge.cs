using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIEdge : MonoBehaviour
{

    public Vec3 HexPos1 { get; set; }
    public Vec3 HexPos2 { get; set; }
    public bool IsSelected = false;

    // void OnMouseEnter() {
    // 	GetComponent<SpriteRenderer> ().color = Color.red;
    // }

    public bool CanBuild()
    {

        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();

        if (!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED))
        {
            StartCoroutine(GameManager.GUI.ShowMessage("Grid not created."));
            return false;
        }

        if (!GameManager.Instance.GetCurrentGameState().CurrentTurn.IsLocalPlayerTurn())
        {
            StartCoroutine(GameManager.GUI.ShowMessage("It is not your turn."));
            return false;
        }

        Edge currentEdge = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(HexPos1, HexPos2);

		if (currentEdge.Owner != "")
		{
			if (currentEdge.Owner != localPlayer.myName)
            {
                StartCoroutine(GameManager.GUI.ShowMessage("You do not own this edge."));
                return false;
            }
		}
			

        return true;
    }


    public void ConstructRoad()
    {

        if (!CanBuild())
            return;

        Edge currentEdge = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(HexPos1, HexPos2);
        bool setupPhase = GameManager.Instance.GetCurrentGameState().CurrentTurn.IsInSetupPhase();
        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();

		if (setupPhase && localPlayer.placedRoad)
        {
            StartCoroutine(GameManager.GUI.ShowMessage("You already placed a road during this round."));
            return;
        }

        if (!isConnectedToOwnedUnit())
        {
            StartCoroutine(GameManager.GUI.ShowMessage("Selected edge must be connected to owned intersection or road."));
            return;
        }

		// only consume resources if not in setup phase
		if (!setupPhase && localPlayer.fishBuild != true &&
		    !(localPlayer.roadBuildingProgressCardDiscount && localPlayer.roadBuildingProgressCardUsed < 2) &&
		    !localPlayer.diplomatCanPlaceRoadForFree) {
			Dictionary<StealableType, int> requiredRes;

			if (currentEdge.IsShip ()) {
				requiredRes = new Dictionary<StealableType, int> () {
					{ StealableType.Resource_Wool, 1 },
					{ StealableType.Resource_Lumber, 1 }
				};
			} else {
				requiredRes = new Dictionary<StealableType, int> () {
					{ StealableType.Resource_Brick, 1 },
					{ StealableType.Resource_Lumber, 1 }
				};
			}

			if (!localPlayer.HasEnoughResources (requiredRes)) {
				StartCoroutine (GameManager.GUI.ShowMessage ("You don't have enough resources."));
				return;
			}

			localPlayer.CmdConsumeResources (requiredRes);

		} else if (localPlayer.roadBuildingProgressCardDiscount && localPlayer.roadBuildingProgressCardUsed < 2) {
			localPlayer.roadBuildingProgressCardUsed++;

			GameManager.GUI.PostStatusMessage ("You built this road for free (road building card). You have " + localPlayer.roadBuildingProgressCardUsed + " free road left.");

			if (localPlayer.roadBuildingProgressCardUsed == 2) {
				localPlayer.roadBuildingProgressCardDiscount = false;
			}
		} else if (!setupPhase && localPlayer.fishBuild == true) {
			Dictionary<StealableType, int> requiredRes;

			requiredRes = new Dictionary<StealableType, int> () {
				{ StealableType.Resource_Fish, 5 }
			};

			StartCoroutine (GameManager.GUI.ShowMessage ("You paid in fish!"));

			if (!localPlayer.HasEnoughResources (requiredRes)) {
				StartCoroutine (GameManager.GUI.ShowMessage ("You don't have enough resources."));
				return;
			}

			localPlayer.CmdConsumeResources (requiredRes);

			localPlayer.fishBuild = false;
		} else if (!setupPhase && localPlayer.diplomatCanPlaceRoadForFree) {
			localPlayer.diplomatCanPlaceRoadForFree = false;
		}

        StartCoroutine(GameManager.GUI.ShowMessage("You have placed a road."));
        // localPlayer.placedRoad = true;
        localPlayer.CmdBuildRoad(SerializationUtils.ObjectToByteArray(new Vec3[] { HexPos1, HexPos2 }));
        GameManager.GUI.guiCanvas.transform.FindChild("SelectionTooltip").gameObject.SetActive(true);
    }

    private bool isConnectedToOwnedUnit()
    {
        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        List<List<Vec3>> adjIntersectionsPos1 = UIHex.getIntersectionsAdjacentPos(this.HexPos1);
        List<List<Vec3>> adjIntersectionsPos2 = UIHex.getIntersectionsAdjacentPos(this.HexPos2);
        List<Intersection> adjIntersections = new List<Intersection>();
        List<Intersection> intersectionsIntersection = new List<Intersection>();

        // get the intersection of both list
        foreach (List<Vec3> intersection1 in adjIntersectionsPos1)
        {
            Intersection i = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(intersection1);
            adjIntersections.Add(i);
        }

        foreach (List<Vec3> intersection2 in adjIntersectionsPos2)
        {
            Intersection i = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(intersection2);
            if (adjIntersections.Contains(i))
            {
                intersectionsIntersection.Add(i);
            }
        }

        // check if any of the intersections intersection is owned by the local player
        foreach (Intersection i in intersectionsIntersection)
        {
            if (i.Owner == localPlayer.myName)
            {
                return true;
            }

            Edge roadTest1 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(i.adjTile1, i.adjTile2);
            Edge roadTest2 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(i.adjTile1, i.adjTile3);
            Edge roadTest3 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(i.adjTile2, i.adjTile3);

            if (roadTest1.Owner == localPlayer.myName || roadTest2.Owner == localPlayer.myName || roadTest3.Owner == localPlayer.myName)
            {
                return true;
            }
        }

        return false;
    }

    // void OnMouseExit() {
    // 	GetComponent<SpriteRenderer> ().color = new Color (0.0f, 0.0f, 0.0f, 0.3f);
    // }

    // Use this for initialization
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingLayerName = "edge";
        GetComponent<SpriteRenderer>().color = new Color((20F / 255), (20F / 255), (20F / 255), 1F);
    }


    void OnMouseDown()
    {
        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();

		if (localPlayer.diplomatProgressCardUsed) {
			localPlayer.GetComponent<UIDiplomatProgressCard> ().SelectRoad (GameManager.Instance.GetCurrentGameState ().CurrentEdges.getEdge (this.HexPos1, this.HexPos2));
			return;
		}

        if (localPlayer.selectedUIEdge == this)
            localPlayer.ResetBuildSelection();
        else
            localPlayer.SetBuildSelection(this);

        updateGUI();
    }

    void updateGUI()
    {
        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        //GameObject selectionInfo = GameManager.GUI.guiCanvas.transform.FindChild("SelectionTooltip").gameObject;
        GameObject unitBtn1 = GameManager.GUI.guiCanvas.transform.FindChild("UnitButton1").gameObject;
        GameObject unitBtn2 = GameManager.GUI.guiCanvas.transform.FindChild("UnitButton2").gameObject;
        if (localPlayer.selectedUIEdge == null)
        {
            unitBtn1.SetActive(false);
            unitBtn2.SetActive(false);
            //selectionInfo.SetActive(false);
        }

        else
        {
            Edge currentEdge = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(HexPos1, HexPos2);

            unitBtn2.SetActive(false);
            //selectionInfo.SetActive(true);

            if (currentEdge.Owner == "")
                unitBtn1.SetActive(true);
            else
                unitBtn1.SetActive(false);


        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.GameStateReadyAtStage(GameState.GameStatus.GRID_CREATED))
        {
            return;
        }

        Edge e = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(HexPos1, HexPos2);
        if (e == null) { return; }

        if (this.IsSelected)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            GetComponent<SpriteRenderer>().sortingOrder = 4;
            return;
        }

        if (e.Owner != "")
        {
            GetComponent<SpriteRenderer>().color = GameManager.ConnectedPlayersByName[e.Owner].GetComponent<GamePlayer>().GetPlayerColor();
            GetComponent<SpriteRenderer>().sortingOrder = 2;
            return;
        }

        if (e.isHarbour == true)
        {
            //GetComponent<SpriteRenderer>().color = Color.yellow;
            GetComponent<SpriteRenderer>().sortingOrder = 3;
            return;
        }

        GetComponent<SpriteRenderer>().sortingOrder = 1;
        GetComponent<SpriteRenderer>().color = new Color((20F / 255), (20F / 255), (20F / 255), 1F);
        return;


    }
}
