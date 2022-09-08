using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkGamePlayerLobby : NetworkBehaviour
{
    [SyncVar]
    public string DisplayName = "Loading...";

    private NetworkMangerLobby m_Room;
    private NetworkMangerLobby Room
    {
        get
        {
            if (m_Room != null) return m_Room;
            return m_Room = NetworkManager.singleton as NetworkMangerLobby;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(this);

        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        this.DisplayName = displayName;
    }
}

