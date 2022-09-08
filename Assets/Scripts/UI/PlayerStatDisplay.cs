using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatDisplay : MonoBehaviour
{
    [SerializeField] private Slider m_HealthSlider;
    [SerializeField] private TMP_Text m_UsernameText;

    private HealthComponent m_HealthComponent;

    public uint PlayerNetId { get; private set; }

    public void Setup(Player player)
    {
        PlayerNetId = player.netId;

        var gamePlayer = NetworkIdentity.spawned[player.OwnerId].GetComponent<NetworkGamePlayerLobby>();

        m_UsernameText.text = gamePlayer.DisplayName;
        
        m_HealthComponent = gamePlayer.GetComponent<HealthComponent>();
    }

    private void Update()
    {
        if (m_HealthComponent == null) return;
        m_HealthSlider.value = m_HealthComponent.GetHealth();
    }
}
