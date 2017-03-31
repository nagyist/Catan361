using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is where hacky stuff used for debug goes
public class Cheats : MonoBehaviour {

    GameObject goldPopup;

	// Use this for initialization
	void Start () {
        GameObject UICanvas = GameObject.Find("Canvas");
        for (int i = 0; i < UICanvas.transform.childCount; i++)
        {
            if (UICanvas.transform.GetChild(i).name.Equals("GoldPopup"))
            {
                goldPopup = UICanvas.transform.GetChild(i).gameObject;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        // Press G to invoke the gold popup regardless of die roll
		if (Input.GetKey(KeyCode.G))
        {
            goldPopup.SetActive(true);
        }
	}
}
