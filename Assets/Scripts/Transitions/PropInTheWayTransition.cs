using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PropInTheWayTransition : IFSMTransition
{
    NavMeshAgent m_NavmeshAgent;

    public override bool ToTransition(GameAgent agent)
    {
        if(m_NavmeshAgent == null) m_NavmeshAgent = agent.GetComponent<NavMeshAgent>();
        if (m_NavmeshAgent.velocity.sqrMagnitude < 0.1f) return false; // if still moving no point in check if something is in front of the agent

        var agentTransform = agent.transform;

        var attackRay = new Ray(agentTransform.position + (agentTransform.forward * 0.25f) + (agentTransform.up), agentTransform.forward);  // Forward with a bit of an offset
        RaycastHit hit;

        if (Physics.Raycast(attackRay, out hit, agent.AttackRange, agent.AttackMask))
        {
            Debug.DrawRay(attackRay.origin, attackRay.direction * hit.distance, Color.red, 1.0f);
            HealthComponent healthComponent = hit.transform.GetComponent<HealthComponent>();
            if (healthComponent != null)
            {
                agent.CurrentCharacterTarget = healthComponent;
                return true;
            }
        }

        return false;
    }
}
