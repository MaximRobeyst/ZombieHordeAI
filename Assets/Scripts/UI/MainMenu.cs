using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkMangerLobby m_NetworkManager = null;

    [Header("UI")]
    [SerializeField] private GameObject m_LandingPagePanel = null;

    public void HostLobby()
    {
        m_NetworkManager.StartHost();
        m_LandingPagePanel.SetActive(false);
    }
}
