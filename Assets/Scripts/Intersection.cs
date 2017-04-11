using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Intersection {

	public Vec3 adjTile1;
	public Vec3 adjTile2;
	public Vec3 adjTile3;

    public IntersectionUnit unit;
	public String Owner = "";

	//public bool canAccessHarbour { get; set; }

	public Intersection(Vec3 i1, Vec3 i2, Vec3 i3) {
		adjTile1 = i1;
		adjTile2 = i2;
		adjTile3 = i3;
	}

	public bool IsSurroundedByWater() {
		bool allWater = true;
		Dictionary<Vec3, HexTile> board = GameManager.Instance.GetCurrentGameState ().CurrentBoard;

		if (board.ContainsKey(this.adjTile1)) {
			allWater = allWater && GameManager.Instance.GetCurrentGameState ().CurrentBoard [adjTile1].IsWater;
		}

		if (board.ContainsKey(this.adjTile2)) {
			allWater = allWater && GameManager.Instance.GetCurrentGameState ().CurrentBoard [adjTile2].IsWater;
		}

			if (board.ContainsKey(this.adjTile3)) {
			allWater = allWater && GameManager.Instance.GetCurrentGameState ().CurrentBoard [adjTile3].IsWater;
		}

		return allWater;
	}

	public override bool Equals(System.Object obj) {
		if (obj == null || GetType () != obj.GetType ())
			return false;

		Intersection i = (Intersection) obj;
		return computeKey (this.adjTile1, this.adjTile2, this.adjTile3) == computeKey (i.adjTile1, i.adjTile2, i.adjTile3);
	}

	public override int GetHashCode() {
		byte[] hashFirst = PositionUtilities.PosToByte(this.adjTile1);
		byte[] hashSecond = PositionUtilities.PosToByte(this.adjTile2);
		byte[] hashThird = PositionUtilities.PosToByte(this.adjTile3);

		Int32 firstHashVal = BitConverter.ToInt32 (hashFirst, 0);
		Int32 secondHashVal = BitConverter.ToInt32 (hashSecond, 0);
		Int32 thirdHashVal = BitConverter.ToInt32 (hashThird, 0);

		return firstHashVal + secondHashVal + thirdHashVal;
	}

	public string getKey() {
		return computeKey (adjTile1, adjTile2, adjTile3);
	}

	private string computeKey(Vec3 hex1, Vec3 hex2, Vec3 hex3) {
		// apply simple heuristic : "flatten" xyz coords of both coords and take the lowest one
		byte[] hashFirst = PositionUtilities.PosToByte(hex1);
		byte[] hashSecond = PositionUtilities.PosToByte(hex2);
		byte[] hashThird = PositionUtilities.PosToByte(hex3);

		string flattenFirst = hex1.x + "" + hex1.y + "" + hex1.z;
		string flattenSecond = hex2.x + "" + hex2.y + "" + hex2.z;
		string flattenThird = hex3.x + "" + hex3.y + "" + hex3.z;

		UInt64 firstHashVal = BitConverter.ToUInt64 (hashFirst, 0);
		UInt64 secondHashVal = BitConverter.ToUInt64 (hashSecond, 0);
		UInt64 thirdHashVal = BitConverter.ToUInt64 (hashThird, 0);

		Dictionary<UInt64, string> hashToFlatten = new Dictionary<UInt64, string> ();
		hashToFlatten.Add (firstHashVal, flattenFirst);
		hashToFlatten.Add (secondHashVal, flattenSecond);
		hashToFlatten.Add (thirdHashVal, flattenThird);

		UInt64[] sortArr = new ulong[] { firstHashVal, secondHashVal, thirdHashVal };
		Array.Sort (sortArr);

		string key = "";
		foreach (UInt64 k in sortArr) {
			key += hashToFlatten [k];
		}

		return key;
	}

	// function that checks for a path of owned edges between two intersections
	public static bool checkForPath(Vec3[] start, Vec3[] end)
    {

		GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();
        String localPlayerName = localPlayer.myName;
        List<Intersection> visitedIntersections = new List<Intersection>();
        Queue<Intersection> intersectionQueue = new Queue<Intersection>();
		Intersection initialIntersection = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(start));
		Intersection goal = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection(new List<Vec3>(end));
        Intersection current;

        visitedIntersections.Add(initialIntersection);
        intersectionQueue.Enqueue(initialIntersection);

        while (intersectionQueue.Count != 0)
        {
            current = intersectionQueue.Dequeue();
            if (current.Equals(goal))
                return true;

            // now we need to get all the nodes adjacent to the current intersection which are also connected to the current intersection

            // add all the connected edges to a list if the local player owns them
            List<Edge> ownedConnectedEdges = new List<Edge>();
            Edge e1 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(current.adjTile1, current.adjTile2);
            if (e1.Owner == localPlayerName)
                ownedConnectedEdges.Add(e1);
            Edge e2 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(current.adjTile1, current.adjTile3);
            if (e2.Owner == localPlayerName)
                ownedConnectedEdges.Add(e2);
            Edge e3 = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(current.adjTile2, current.adjTile3);
            if (e3.Owner == localPlayerName)
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


                foreach (Intersection testIntersection in adjIntersections)
                {
                    if (!visitedIntersections.Contains(testIntersection))
                    {
                        visitedIntersections.Add(testIntersection);
                        intersectionQueue.Enqueue(testIntersection);
                    }
                }

            }
        }

        return false;
    }
}
