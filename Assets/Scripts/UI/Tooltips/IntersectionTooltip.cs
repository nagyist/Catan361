using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IntersectionTooltip : MonoBehaviour {

	public Intersection ReferencedIntersection = null;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (ReferencedIntersection == null) {
			return;
		}

		GameObject iconIntersection = transform.FindChild ("Window").FindChild ("IconIntersection").gameObject;
		GameObject txtIntersectionType = transform.FindChild ("Window").FindChild ("TxtIntersectionType").gameObject;
		GameObject txtIntersectionOwner = transform.FindChild ("Window").FindChild ("TxtIntersectionOwner").gameObject;

        // only display if there is a unit
        if (ReferencedIntersection.unit != null)
        {
            // get the village
            if (ReferencedIntersection.unit.GetType() == typeof(Village))
            {
                Village ReferencedVillage = (Village)(ReferencedIntersection.unit);
                // change the text to the kind of setlement
                if (ReferencedVillage.cityWall)
                    txtIntersectionType.GetComponent<Text>().text = "Walled " + ReferencedVillage.myKind.ToString();
                else
                    txtIntersectionType.GetComponent<Text>().text = ReferencedVillage.myKind.ToString();
            }
            else if (ReferencedIntersection.unit.GetType() == typeof(Knight))
            {
                Knight refKnight = (Knight)(ReferencedIntersection.unit);
                if (refKnight.active)
                    txtIntersectionType.GetComponent<Text>().text = "Active Knight Level " + refKnight.level;
                else
                    txtIntersectionType.GetComponent<Text>().text = "Inactive Knight Level " + refKnight.level;

            }
            // add the player's name
            txtIntersectionOwner.GetComponent<Text>().text = ReferencedIntersection.Owner;
        }
		
	}
}
