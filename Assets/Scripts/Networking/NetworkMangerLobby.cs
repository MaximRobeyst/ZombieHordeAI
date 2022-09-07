using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkMangerLobby : NetworkManager
{
    [SerializeField] private int m_MinPlayers = 1;
    [Scene] [SerializeField] private string m_MenuScene = string.Empty;

    [Header("Room")]
    [SerializeField] private NetworkRoomPlayerLobby m_RoomPlayerPrefab = null;

    // Listen in on these actions without having to call them
    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        OnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        // If there are too many players disconnect
        if(numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;
        }

        // if the game is in progress stop them from joining
        if(!m_MenuScene.Contains(SceneManager.GetActiveScene().name))
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (m_MenuScene.Contains(SceneManager.GetActiveScene().name))
        {
            bool isLeader = RoomPlayers.Count == 0;

            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(m_RoomPlayerPrefab);

            roomPlayerInstance.IsLeader = isLeader;

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if(conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();
            RoomPlayers.Remove(player);

            NotifyPlayerOfReadyState();
        }

        base.OnServerDisconnect(conn);
    }

    public void NotifyPlayerOfReadyState()
    {
        foreach(var player in RoomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if(numPlayers < m_MinPlayers) { return false; }

        foreach(var player in RoomPlayers)
        {
            if (!player.m_IsReady) 
                return false;
        }

        return true;
    }

    public override void OnStopServer()
    {
        RoomPlayers.Clear();
    }
}
