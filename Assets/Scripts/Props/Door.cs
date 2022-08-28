using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactible
{
    bool m_Open;
    Animator m_Animator;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public override void Interacting()
    {
        m_Open = !m_Open;
        m_Animator.SetBool("Open", m_Open);
    }
}
