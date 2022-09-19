using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayersCanvasManager : NetworkBehaviour
{
    [SerializeField] private PlayerStatDisplay m_PlayerStateDisplay;
    [SerializeField] private Transform m_Parent;

    public void AddPlayer(GameObject player)
    {
        var displayInstance = Instantiate(m_PlayerStateDisplay, m_Parent);
        NetworkServer.Spawn(displayInstance.gameObject, connectionToServer);

        var statDisplay = displayInstance.GetComponent<PlayerStatDisplay>();
        statDisplay.Setup(player);

    }
}
