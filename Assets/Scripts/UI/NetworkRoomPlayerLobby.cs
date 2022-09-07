using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject m_LobbyUI = null;
    [SerializeField] private TMP_Text[] m_PlayerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] m_PlayerReadyTexts = new TMP_Text[4];
    [SerializeField] private Button m_StartGameButton = null;

    [SyncVar(hook = nameof(HandleDIsplayNameChanged))]
    public string m_DisplayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool m_IsReady = false;

    private bool m_IsLeader;

    public bool IsLeader { set { m_IsLeader = value; m_StartGameButton.gameObject.SetActive(value); } }

    private NetworkMangerLobby m_Room;
    private NetworkMangerLobby Room
    {
        get
        {
            if (m_Room != null) return m_Room;
            return m_Room = NetworkManager.singleton as NetworkMangerLobby;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName);
        m_LobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);
        UpdateDisplay();
    }

    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);
        UpdateDisplay();
    }

    public void HandleDIsplayNameChanged(string oldValue, string newValue)
    {
        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue)
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if(!hasAuthority)
        {
            foreach(var player in Room.RoomPlayers)
            {
                if(player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }

        for(int i = 0; i < m_PlayerNameTexts.Length; ++i)
        {
            m_PlayerNameTexts[i].text = "Waiting for player...";
            m_PlayerReadyTexts[i].text = String.Empty;
        }

        for(int i = 0; i < Room.RoomPlayers.Count; ++i)
        {
            m_PlayerNameTexts[i].text = Room.RoomPlayers[i].m_DisplayName;
            m_PlayerReadyTexts[i].text = Room.RoomPlayers[i].m_IsReady ? "<color=green>Ready</color>" : "<color=red>Not Ready</color>";
        }
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if(!m_IsLeader) { return; }

        m_StartGameButton.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        m_DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        m_IsReady = !m_IsReady;
        Room.NotifyPlayerOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient) return;

        // Start game
    }
}

