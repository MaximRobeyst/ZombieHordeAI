using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatDisplayManager : MonoBehaviour
{
    [SerializeField] private PlayerStatDisplay m_StatEntityDisplay = null;
    [SerializeField] private Transform m_StatEntityHolderTransform = null;

    private readonly List<PlayerStatDisplay> m_StatEntityDisplays = new List<PlayerStatDisplay>();

    private void Awake()
    {
        Player.OnPlayerSpawned += HandlePlayerSpawned;
        Player.OnPlayerDespawned += HandlePlayerDespawned;
    }

    private void OnDestroy()
    {
        Player.OnPlayerSpawned -= HandlePlayerSpawned;
        Player.OnPlayerDespawned -= HandlePlayerDespawned;
    }

    private void HandlePlayerSpawned(Player player)
    {
        PlayerStatDisplay displayInstance = Instantiate(m_StatEntityDisplay, m_StatEntityHolderTransform);

        displayInstance.Setup(player);

        m_StatEntityDisplays.Add(displayInstance);
    }

    private void HandlePlayerDespawned(Player player)
    {
        PlayerStatDisplay displayInstance = m_StatEntityDisplays.FirstOrDefault(x => x.PlayerNetId == player.netId);
        if (displayInstance == null) return;

        m_StatEntityDisplays.Remove(displayInstance);
        Destroy(displayInstance);
    }
}
