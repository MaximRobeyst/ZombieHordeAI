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

    [Header("Game")]
    [SerializeField] private NetworkGamePlayerLobby m_GamePlayerPrefab = null;
    [SerializeField] private GameObject m_PlayerSpawnSystem = null;
    [SerializeField] private GameObject m_SharedCanvas = null;

    // Listen in on these actions without having to call them
    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;
    public static event Action<NetworkConnection> OnServerReadied;

    public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();
    public List<NetworkGamePlayerLobby> GamePlayers { get; } = new List<NetworkGamePlayerLobby>();

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

    public void StartGame()
    {
        if(m_MenuScene.Contains(SceneManager.GetActiveScene().name))
        {
            if (!IsReadyToStart()) return;

            ServerChangeScene("SampleScene");
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        if(m_MenuScene.Contains(SceneManager.GetActiveScene().name))
        {
            for (int i = RoomPlayers.Count - 1; i >= 0; --i)
            {
                var conn = RoomPlayers[i].connectionToClient;
                var gameplayInstance = Instantiate(m_GamePlayerPrefab);
                gameplayInstance.SetDisplayName(RoomPlayers[i].name);

                NetworkServer.Destroy(conn.identity.gameObject);
                NetworkServer.ReplacePlayerForConnection(conn, gameplayInstance.gameObject);
            }
        }

        base.ServerChangeScene(newSceneName);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if(sceneName.StartsWith("SampleScene"))
        {
            GameObject playerSpawnSystemInstance = Instantiate(m_PlayerSpawnSystem);
            NetworkServer.Spawn(playerSpawnSystemInstance);
        }
    }

    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        base.OnServerReady(conn);

        OnServerReadied?.Invoke(conn);
    }
}
