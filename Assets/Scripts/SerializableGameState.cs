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

    public Dictionary<Vec3, HexTile> CurrentBoard;
    public EdgeCollection CurrentEdges;
    public IntersectionCollection CurrentIntersections;
    public ResourceCollection CurrentResources;
    public GameTurn CurrentTurn;
    public GameStatus CurrentStatus;
    public RobberPiratePlacement CurrentRobberPosition;
    public RobberPiratePlacement CurrentPiratePosition;
    public VictoryPointsCollection CurrentVictoryPoints;
    public BarbarianEvent CurrentBarbarianEvent;
    public ProgressCardDeck CurrentProgressCardDeck;
    public ProgressCardCollection CurrentProgressCardHands;
    public PlayerImprovementCollection CurrentPlayerImprovements;

    public SerializableGameState(GameState curr)
    {
        CurrentBoard = curr.CurrentBoard;
        CurrentEdges = curr.CurrentEdges;
        CurrentIntersections = curr.CurrentIntersections;
        CurrentResources = curr.CurrentResources;
        CurrentTurn = curr.CurrentTurn;
        CurrentStatus = (GameStatus)curr.CurrentStatus;
        CurrentRobberPosition = curr.CurrentRobberPosition;
        CurrentPiratePosition = curr.CurrentPiratePosition;
        CurrentVictoryPoints = curr.CurrentVictoryPoints;
        CurrentBarbarianEvent = curr.CurrentBarbarianEvent;
        CurrentProgressCardDeck = curr.CurrentProgressCardDeck;
        CurrentProgressCardHands = curr.CurrentProgressCardHands;
        CurrentPlayerImprovements = curr.CurrentPlayerImprovements;
    }

}
