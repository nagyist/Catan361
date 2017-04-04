using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableGameState {

    public enum GameStatus
    {
        NOT_READY = -1,
        GRID_CREATED = 0,
        GAME_TURN_SYNC = 1
    }

    public float timestamp = 0;

    Dictionary<Vec3, HexTile> CurrentBoard;
    EdgeCollection CurrentEdges;
    IntersectionCollection CurrentIntersections;
    ResourceCollection CurrentResources = new ResourceCollection();
    GameTurn CurrentTurn = new GameTurn();
    GameStatus CurrentStatus { get; set; }

    public SerializableGameState(GameState curr)
    {
        CurrentBoard = curr.CurrentBoard;
        CurrentEdges = curr.CurrentEdges;
        CurrentIntersections = curr.CurrentIntersections;
        CurrentResources = curr.CurrentResources;
        CurrentTurn = curr.CurrentTurn;
        CurrentStatus = (GameStatus)curr.CurrentStatus;
    }

}
