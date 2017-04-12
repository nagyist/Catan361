using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// This is where hacky stuff used for debug goes
public class Cheats : NetworkBehaviour {

    GameObject goldPopup;

    FileBrowser fb = new FileBrowser();
    public GUISkin[] skins;
    public Texture2D file, folder, back, drive;
    bool drawBrowser = false;
    bool drawTextbox = false;
    string saveFile = "";
    string loadFile;

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

        //setup file browser style
        fb.guiSkin = skins[0]; //set the starting skin
        //set the various textures
        fb.fileTexture = file;
        fb.directoryTexture = folder;
        fb.backTexture = back;
        fb.driveTexture = drive;
        //show the search bar
        fb.showSearch = false;
        //search recursively (setting recursive search may cause a long delay)
        fb.searchRecursively = false;
    }
	
	// Update is called once per frame
	void Update () {
        // Press S for quicksave (only invokes if server) and L for quickload (also server-only)
        if (Input.GetKey(KeyCode.F5) && isServer)
        {
            drawTextbox = true;
        }
        if (Input.GetKey(KeyCode.F6) && isServer)
        {
            drawBrowser = true;
        }
        if (Input.GetKey(KeyCode.Return) && isServer && saveFile.Equals("") && drawTextbox)
        {
            drawTextbox = false;
        }
        if (Input.GetKey(KeyCode.Return) && isServer && !saveFile.Equals("") && drawTextbox)
        {
            SaveAndLoad.save(saveFile);
            drawTextbox = false;
        }
	}

    private void OnGUI()
    {
        if (drawTextbox)
        {
            saveFile = GUI.TextField(new Rect(250, 100, 200, 20), saveFile, 25);
        }
        if (drawBrowser)
        {
            fb.setDirectory(Application.persistentDataPath);
            fb.setLayout(1);
            if (fb.draw())
            {
                loadFile = (fb.outputFile == null) ? "cancel hit" : fb.outputFile.ToString();
                if (!loadFile.Equals("cancel hit"))
                {
                    SaveAndLoad.load(loadFile);
                }
                drawBrowser = false;
            }
        }
    }
}
