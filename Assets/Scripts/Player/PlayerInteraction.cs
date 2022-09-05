using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float m_Distance = 2.0f;
    [SerializeField] private LayerMask m_InteractionMask;
    [SerializeField] private GameObject m_ButtonPrompt;

    Interactible m_Interactible;

    private void Start()
    {
        if (m_ButtonPrompt == null) return;

        m_ButtonPrompt.SetActive(false);
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;


        if (m_ButtonPrompt != null)
            m_ButtonPrompt.SetActive(m_Interactible != null);

        if (Physics.Raycast(ray, out hit, m_Distance, m_InteractionMask))
        {
            m_Interactible = hit.transform.GetComponent<Interactible>();
            if((m_Interactible != null))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    m_Interactible.Interacting(gameObject.transform.parent.gameObject);
                }
            }
            return;
        }

        m_Interactible = null;
    }
}
