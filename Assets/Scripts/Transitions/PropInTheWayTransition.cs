using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropInTheWayTransition : IFSMTransition
{
    public override bool ToTransition(GameAgent agent)
    {
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
