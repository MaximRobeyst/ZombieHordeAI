using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private NetworkMangerLobby networkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject m_LandingPagePanel = null;
    [SerializeField] private TMP_InputField m_IpAddressInputField = null;
    [SerializeField] private Button joinButton = null;

    private void OnEnable()
    {
        NetworkMangerLobby.OnClientConnected += HandleClientConnected;
        NetworkMangerLobby.OnClientDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        NetworkMangerLobby.OnClientConnected -= HandleClientConnected;
        NetworkMangerLobby.OnClientDisconnected -= HandleClientDisconnected;
    }

    public void JoinLobby()
    {
        string ipAddress = m_IpAddressInputField.text;

        networkManager.networkAddress = ipAddress;
        networkManager.StartClient();

        joinButton.interactable = false;
    }

    public void HandleClientConnected()
    {
        joinButton.interactable = true;
        gameObject.SetActive(false);
        m_LandingPagePanel.SetActive(false);
    }

    public void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
}
