using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInteraction : NetworkBehaviour
{
    [SerializeField] private float m_Distance = 2.0f;
    [SerializeField] private LayerMask m_InteractionMask;
    [SerializeField] private GameObject m_ButtonPrompt;

    [SerializeField] private Camera m_Camera;

    Interactible m_Interactible;

    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
        if (m_ButtonPrompt == null) return;

        m_ButtonPrompt.SetActive(false);
    }

    private void Update()
    {
        Ray ray = new Ray(m_Camera.transform.position, m_Camera.transform.forward);
        RaycastHit hit;


        if (m_ButtonPrompt != null)
            m_ButtonPrompt.SetActive(m_Interactible != null);

        if (Physics.Raycast(ray, out hit, m_Distance, m_InteractionMask, QueryTriggerInteraction.Ignore))
        {
            m_Interactible = hit.transform.GetComponent<Interactible>();
            if((m_Interactible != null))
            {
                Debug.Log(m_Interactible.name + "Is interactible");
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Debug.Log($"interacting with {m_Interactible.name}");
                    m_Interactible.CmdInteract(gameObject);
                }
            }
            return;
        }

        m_Interactible = null;
    }
}
