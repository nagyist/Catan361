using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveAndLoad {
    
    static SerializableGameState savedState = null;

    public static void save(string filename)
    {

        GameState current = GameObject.Find("GameState").GetComponent<GameState>();
        savedState = new SerializableGameState(current);
        savedState.timestamp = Time.time;
        Debug.Log("Saved at timestamp: " + savedState.timestamp);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + filename + ".CoCSave");
        bf.Serialize(file, savedState);
        file.Close();
    }

    public static void load(string path)
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            savedState = (SerializableGameState)bf.Deserialize(file);
            Debug.Log("Loaded, timestamp is: " + savedState.timestamp);
            file.Close();

            // Actual loading
            GameState current = GameObject.Find("GameState").GetComponent<GameState>();
            current.CurrentBoard = savedState.CurrentBoard;
            current.CurrentEdges = savedState.CurrentEdges;
            current.CurrentIntersections = savedState.CurrentIntersections;
            current.CurrentResources = savedState.CurrentResources;
            current.CurrentTurn = savedState.CurrentTurn;
            current.CurrentStatus = (GameState.GameStatus) savedState.CurrentStatus;
            current.SyncGameBoard();
            current.SyncGameTurns();
        }
    }

}
