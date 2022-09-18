using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayersCanvasManager : NetworkBehaviour
{
    [SerializeField] private PlayerStatDisplay m_PlayerStateDisplay;
    [SerializeField] private Transform m_Parent;

    public void AddPlayer(Player player)
    {

    }

    public override void OnStartServer()
    {
        NetworkMangerLobby.OnServerReadied += SpawnPlayerDisplay;
    }

    [Server]
    public void SpawnPlayerDisplay(NetworkConnection conn)
    {
        var displayInstance = Instantiate(m_PlayerStateDisplay, m_Parent);
        NetworkServer.Spawn(displayInstance.gameObject, conn);

        //displayInstance.Setup()
    }
}
