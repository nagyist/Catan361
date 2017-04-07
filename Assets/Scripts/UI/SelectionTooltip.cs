using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionTooltip : MonoBehaviour {

    // Update is called once per frame
    void Update()
    {
        Text body = GetComponentInChildren<Text>();
        GamePlayer localPlayer = GameManager.LocalPlayer.GetComponent<GamePlayer>();

        if (localPlayer.selectedUIEdge != null)
        {
            Edge edge = GameManager.Instance.GetCurrentGameState().CurrentEdges.getEdge(localPlayer.selectedUIEdge.HexPos1, localPlayer.selectedUIEdge.HexPos2);
            if (edge.Owner == "")
                body.text = "\n  Selected Unit:\n\n\tEdge\n\tNo Owner";
            else
                body.text = "\n  Selected Unit:\n\n\tEdge\n\tOwner: " + edge.Owner;
        }
        else if (localPlayer.selectedUIIntersection != null)
		{
			Intersection i = GameManager.Instance.GetCurrentGameState().CurrentIntersections.getIntersection
				(new List<Vec3>(new Vec3[] { localPlayer.selectedUIIntersection.HexPos1,localPlayer.selectedUIIntersection.HexPos2, localPlayer.selectedUIIntersection.HexPos3 }));

			if (i.unit == null)
				body.text = "\n  Selected Unit:\n\n\tIntersection\n\tNo Owner";
			else if (i.unit.GetType() == typeof(Knight))
			{
                Knight k = (Knight)i.unit;
                body.text = "\n  Selected Unit:\n"
					+ "\n\tKnight" 
					+ "\n\tOwner: " + i.Owner 
					+ "\n\tLevel: " + k.level 
					+ "\n\tHas been promoted this turn: " + k.hasBeenPromotedThisTurn 
					+ "\n\tHas been moved this turn: " + k.hasMovedThisTurn;
            }
			else if (i.unit.GetType() == typeof(Village))
			{
                Village v = (Village)i.unit;
                body.text = "\n  Selected Unit:\n"
                    + "\n\tVillage"
                    + "\n\tOwner: " + i.Owner
                    + "\n\tType: " + v.myKind
                    + "\n\tHas city wall: " + v.cityWall;
            }
		}
		else
		{
            body.text = "this shouldn't be showing";
        }
    }
   
}

