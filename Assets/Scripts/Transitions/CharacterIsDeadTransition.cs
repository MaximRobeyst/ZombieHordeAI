using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIsDeadTransition : IFSMTransition
{
    private HealthComponent m_HealthComponent;

    public override bool ToTransition(GameAgent agent)
    {
        if(m_HealthComponent == null) m_HealthComponent = agent.GetComponent<HealthComponent>();

        if (m_HealthComponent.Dead)
            return true;

        return m_HealthComponent.Dead;
    }
}
