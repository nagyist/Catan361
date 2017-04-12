using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMoveButton : MonoBehaviour {

	bool inUse = false;

    string localPlayerName;

    public void ClickBuild()
    {
        // get local player and current intersection selection
        GamePlayer localPlayer = getLocalPlayer();
        String localPlayerName = localPlayer.myName;
        UIIntersection selectedUIIntersection = localPlayer.selectedUIIntersection;
        Vec3[] selectedPos = new Vec3[] { selectedUIIntersection.HexPos1, selectedUIIntersection.HexPos2, selectedUIIntersection.HexPos3 };
        Intersection selectedIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(selectedPos));
        
        // check if player has knights he must reposition
        if (localPlayer.knightsToMove.Count != 0)
        {
            // game rules checks 
            if (selectedUIIntersection == null)
            {
                StartCoroutine(GameManager.GUI.ShowMessage("Please select an intersection to move to."));
                return;
            }
            else if (selectedIntersection.Owner != localPlayer.myName && selectedIntersection.Owner != "")
            {
                StartCoroutine(GameManager.GUI.ShowMessage("You do not own that intersection."));
                return;
            }
            else if (selectedIntersection.unit != null)
            {
                StartCoroutine(GameManager.GUI.ShowMessage("This intersection already contains a unit."));
                return;
            }

            // check for valid path
            KeyValuePair<Vec3[], Knight> pair = localPlayer.knightsToMove.Peek();
            Vec3[] oldPos = pair.Key;
            if (!checkForPath(oldPos, selectedPos, localPlayerName))
            {
                StartCoroutine(GameManager.GUI.ShowMessage("Selected intersection must on valid path from previous Knight location."));
                return;
            }
            // remove entry from the queue
            pair = localPlayer.knightsToMove.Dequeue();
            Knight k = pair.Value;

            // call command to replace knight and reset the button
            localPlayer.CmdReplaceKnight(SerializationUtils.ObjectToByteArray(selectedPos), SerializationUtils.ObjectToByteArray(k));
            StartCoroutine(GameManager.GUI.ShowMessage("You have repositioned your knight."));
            inUse = false;
        }
        // if the local player hasn't selected a unit to move
        else if (!inUse)
        {
            // invalid selection checks 
            if (selectedIntersection.unit == null)
            {
                StartCoroutine(GameManager.GUI.ShowMessage("Knight must be selected for movement."));
                return;
            }
            else if (selectedIntersection.unit.GetType() != typeof(Knight))
            {
                StartCoroutine(GameManager.GUI.ShowMessage("Knight must be selected for movement."));
                return;
            }
            else
            {
                Knight k = (Knight)selectedIntersection.unit;

                if (!k.active)
                {
                    StartCoroutine(GameManager.GUI.ShowMessage("Only active knights may move."));
                    return;
                }
                else if (k.exhausted)
                {
                    StartCoroutine(GameManager.GUI.ShowMessage("This knight has already completed an action on the current turn."));
                    return;
                }
                // set the local player's move selection and set the button to in use
                localPlayer.SetMoveSelection();
                StartCoroutine(GameManager.GUI.ShowMessage("Set the unit to move."));
                inUse = true;
            }

           
        }
        // if the local player has selected a unit 
        else
        {
            UIIntersection oldUIIntersection = localPlayer.uiIntersectionToMove;
            Vec3[] oldPos = new Vec3[] { oldUIIntersection.HexPos1, oldUIIntersection.HexPos2, oldUIIntersection.HexPos3 };
            Intersection oldIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(oldPos));

            // check for a valid path
            if (!checkForPath(oldPos, selectedPos, localPlayerName))
            {
                StartCoroutine(GameManager.GUI.ShowMessage("Selected intersection must on a valid path."));
                inUse = false;
                return;
            }
            // check for owned intersection
            else if (selectedIntersection.Owner != "")
            {
                // throw error if local player already owns selected intersection
                if (selectedIntersection.Owner == localPlayer.myName)
                {
                    StartCoroutine(GameManager.GUI.ShowMessage("You already own a unit at the selected location."));
                    inUse = false;
                    return;
                }
                // check for a knight in the selected intersection
                else if (selectedIntersection.unit != null)
                {
                    if (selectedIntersection.unit.GetType() == typeof(Knight))
                    {
                        // get the knights and the other owner's knight
                        Knight localPlayerKnight = (Knight)oldIntersection.unit;
                        Knight replacedKnight = (Knight)selectedIntersection.unit;
                        String oldOwnerName = selectedIntersection.Owner;

                        // compare the knight's levels 
                        if (localPlayerKnight.level <= replacedKnight.level)
                        {
                            StartCoroutine(GameManager.GUI.ShowMessage("You may only displace a lower level knight."));
                            inUse = false;
                            return;
                        }

                        // check to see if the displaced knight will be removed from play
                        if (checkKnightRemoval(selectedPos, oldOwnerName))
                        {
                            // moves knight by overriding old position
                            // also calls rpc function to lower the number of knights owned of the old intersection owner
                            localPlayer.CmdMoveUnitWithRemoval(
                                SerializationUtils.ObjectToByteArray(oldPos),
                                SerializationUtils.ObjectToByteArray(selectedPos),
                                SerializationUtils.ObjectToByteArray(oldOwnerName),
                                SerializationUtils.ObjectToByteArray(replacedKnight));

                            StartCoroutine(GameManager.GUI.ShowMessage("You have removed " + oldOwnerName + "'s knight."));
                            inUse = false;
                            return;
                        }
                        else
                        {
                            // moves knight and calls rpc function to add replaced knight to queue for the previous owner
                            localPlayer.CmdMoveUnitWithReplacement(
                                SerializationUtils.ObjectToByteArray(oldPos),
                                SerializationUtils.ObjectToByteArray(selectedPos),
                                SerializationUtils.ObjectToByteArray(oldOwnerName),
                                SerializationUtils.ObjectToByteArray(replacedKnight));

                            StartCoroutine(GameManager.GUI.ShowMessage("You have displaced " + oldOwnerName + "'s knight."));
                            inUse = false;
                            return;
                        }
                    }
                    else
                    {
                        StartCoroutine(GameManager.GUI.ShowMessage("Intersection contains a non-knight unit."));
                        inUse = false;
                        return;
                    }
                }
            }
            // if selection is empty intersection
            else
            {
                // move the knight and reset the button
                localPlayer.CmdMoveUnit(SerializationUtils.ObjectToByteArray(oldPos), SerializationUtils.ObjectToByteArray(selectedPos));
                StartCoroutine(GameManager.GUI.ShowMessage("Successfully moved the unit."));
                inUse = false;
                return;
            }
        }
    }

    // function that checks if a knight can be repositioned
    // if the knight will be removed from play, the function will return true
	public static bool checkKnightRemoval(Vec3[] position, String name)
    {
        String ownerName = name;
        List<Intersection> visitedIntersections = new List<Intersection>();
        Queue<Intersection> intersectionQueue = new Queue<Intersection>();
        Intersection current = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(position));

        visitedIntersections.Add(current);
        intersectionQueue.Enqueue(current);

        if (current.Owner == "")
            return false;

        while (intersectionQueue.Count != 0)
        {
            
            current = intersectionQueue.Dequeue();

            // now we need to get all the nodes adjacent to the current intersection which are also connected to the current intersection

            // add all the connected edges to a list if the local player owns them
            List<Edge> ownedConnectedEdges = new List<Edge>();
            Edge e1 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(current.adjTile1, current.adjTile2);
            if (e1.Owner == ownerName)
                ownedConnectedEdges.Add(e1);
            Edge e2 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(current.adjTile1, current.adjTile3);
            if (e2.Owner == ownerName)
                ownedConnectedEdges.Add(e2);
            Edge e3 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(current.adjTile2, current.adjTile3);
            if (e3.Owner == ownerName)
                ownedConnectedEdges.Add(e3);

            // loop through edges found
            foreach (Edge e in ownedConnectedEdges)
            {
                // get the positions of all the intersections of the two adjacent hexes
                List<List<Vec3>> adjHexIntersectionPos1 = UIHex.getIntersectionsAdjacentPos(e.adjTile1);
                List<List<Vec3>> adjHexIntersectionsPos2 = UIHex.getIntersectionsAdjacentPos(e.adjTile2);

                List<Intersection> intersectionBuffer = new List<Intersection>();
                List<Intersection> adjIntersections = new List<Intersection>();

                // add all the intersections of the first adjacent hex to a buffer list
                foreach (List<Vec3> hexIntersectionPos in adjHexIntersectionPos1)
                {
                    Intersection hexIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(hexIntersectionPos);
                    intersectionBuffer.Add(hexIntersection);
                }

                // add the intersections of the second adjacent hex to the final list if the buffer already contains them
                foreach (List<Vec3> hexIntersectionPos in adjHexIntersectionsPos2)
                {
                    Intersection hexIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(hexIntersectionPos);
                    if (intersectionBuffer.Contains(hexIntersection))
                        adjIntersections.Add(hexIntersection);
                }

                // loop through the found intersections 
                foreach (Intersection testIntersection in adjIntersections)
                {
                    // if we have not visited the intersection, add it to the list and the queue
                    if (!visitedIntersections.Contains(testIntersection))
                    {
                        
                        // if we find an empty intersection in the path, then the knight can be repositioned
                        // therefore return false
                        if (testIntersection.Owner == "")
                            return false;
                        // check to see if the owner of the knight owns the current intersection
                        // knights may only go over intersections if you own them
                        else if (testIntersection.Owner == ownerName)
                        {
                            visitedIntersections.Add(testIntersection);
                            intersectionQueue.Enqueue(testIntersection);
                        }
                    }
                }
            }
        }
        // we've reached an empty queue, so we haven't found any intersections along any path
        // knight must be removed
        return true;
    }

    // function that checks for a path of owned edges and owned intersections between a start and end position
    private bool checkForPath(Vec3[] start, Vec3[] end, String name)
    {

        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        String ownerName = name;
        List<Intersection> visitedIntersections = new List<Intersection>();
        Queue<Intersection> intersectionQueue = new Queue<Intersection>();
        Intersection initialIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(start));
        Intersection goal = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(end));
        Intersection current;

        if (initialIntersection.Equals(goal))
            return true;

        visitedIntersections.Add(initialIntersection);
        intersectionQueue.Enqueue(initialIntersection);

        while (intersectionQueue.Count != 0)
        {
            current = intersectionQueue.Dequeue();

            // NOTE:  we need to get all the nodes adjacent to the current intersection which are also connected to the current intersection

            // add all the connected edges to a list if the local player owns them
            List<Edge> ownedConnectedEdges = new List<Edge>();
            Edge e1 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(current.adjTile1, current.adjTile2);
            if (e1.Owner == ownerName)
                ownedConnectedEdges.Add(e1);
            Edge e2 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(current.adjTile1, current.adjTile3);
            if (e2.Owner == ownerName)
                ownedConnectedEdges.Add(e2);
            Edge e3 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(current.adjTile2, current.adjTile3);
            if (e3.Owner == ownerName)
                ownedConnectedEdges.Add(e3);

            // loop through edges found
            foreach (Edge e in ownedConnectedEdges)
            {
                // get the positions of all the intersections of the two adjacent hexes
                List<List<Vec3>> adjHexIntersectionPos1 = UIHex.getIntersectionsAdjacentPos(e.adjTile1);
                List<List<Vec3>> adjHexIntersectionsPos2 = UIHex.getIntersectionsAdjacentPos(e.adjTile2);

                List<Intersection> intersectionBuffer = new List<Intersection>();
                List<Intersection> adjIntersections = new List<Intersection>();

                // add the intersections of the first adjacent hex to a buffer list
                foreach (List<Vec3> hexIntersectionPos in adjHexIntersectionPos1)
                {
                    Intersection hexIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(hexIntersectionPos);
                    intersectionBuffer.Add(hexIntersection);
                }

                // add the intersections of the second adjacent hex to the final list if the buffer contains them
                foreach (List<Vec3> hexIntersectionPos in adjHexIntersectionsPos2)
                {
                    Intersection hexIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(hexIntersectionPos);
                    if (intersectionBuffer.Contains(hexIntersection))
                        adjIntersections.Add(hexIntersection);
                }

                // loop through the found intersections 
                foreach (Intersection testIntersection in adjIntersections)
                {
                    // if we have not visited the intersection, add it to the list and the queue
                    if (!visitedIntersections.Contains(testIntersection))
                    {
                        // check to see if the owner of the knight owns the current intersection
                        // knights may only go over intersections if you own them
                        if (testIntersection.Equals(goal))
                            return true;
                        else if (testIntersection.Owner == ownerName)
                        {
                            visitedIntersections.Add(testIntersection);
                            intersectionQueue.Enqueue(testIntersection);
                        }
                    }
                }
            }
        }

        return false;
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
