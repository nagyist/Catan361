using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyToGame : LobbyHook {

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        base.OnLobbyServerSceneLoadedForPlayer(manager, lobbyPlayer, gamePlayer);
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        GamePlayer game = gamePlayer.GetComponent<GamePlayer>();

        game.myColor = lobby.playerColor;
        game.myName = lobby.playerName;
    }
}
