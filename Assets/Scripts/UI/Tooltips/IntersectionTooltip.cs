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

		txtIntersectionType.GetComponent<Text>().text = ReferencedIntersection.SettlementLevel == 1 ? "Settlement" : "City";
		txtIntersectionOwner.GetComponent<Text> ().text = ReferencedIntersection.Owner;
	}
}
