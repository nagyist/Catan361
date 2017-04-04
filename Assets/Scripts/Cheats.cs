using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// This is where hacky stuff used for debug goes
public class Cheats : NetworkBehaviour {

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
        // Press S for quicksave (only invokes if server) and L for quickload (also server-only)
        if (Input.GetKey(KeyCode.S) && isServer)
        {
            SaveAndLoad.save();
        }
        if (Input.GetKey(KeyCode.L) && isServer)
        {
            SaveAndLoad.load();
        }
	}
}
