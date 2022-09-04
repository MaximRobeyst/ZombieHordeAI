using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

// https://www.youtube.com/watch?v=mEZMsRouIWo
public class MyNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log("Player Joined");

        base.OnServerAddPlayer(conn);

        CSteamID steamId = SteamMatchmaking.GetLobbyMemberByIndex(SteamLobby.LobbyId, numPlayers - 1);  // This get steam id of the player that just joined

        var playerInfoDisplay = conn.identity.GetComponent<PlayerInfoDisplay>();
        playerInfoDisplay.SetSteamId(steamId.m_SteamID);
    }
}
