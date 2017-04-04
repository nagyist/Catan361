using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveAndLoad {
    
    static SerializableGameState savedState = null;

    public static void save()
    {
        GameState current = GameObject.Find("GameState").GetComponent<GameState>();
        savedState = new SerializableGameState(current);
        savedState.timestamp = Time.time;
        Debug.Log("Saved at timestamp: " + savedState.timestamp);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.CoCsave");
        bf.Serialize(file, savedState);
        file.Close();
    }

    public static void load()
    {
        if (File.Exists(Application.persistentDataPath + "/save.CoCsave"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.CoCsave", FileMode.Open);
            savedState = (SerializableGameState)bf.Deserialize(file);
            Debug.Log("Loaded, timestamp is: " + savedState.timestamp);
            file.Close();
        }
    }

}
