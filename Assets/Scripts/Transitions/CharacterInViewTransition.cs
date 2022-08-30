using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterInViewTransition : IFSMTransition
{
    private NavMeshAgent m_NavmeshAgent;

    public override bool ToTransition(GameAgent agent)
    {
        if (m_NavmeshAgent == null) m_NavmeshAgent = agent.GetComponent<NavMeshAgent>();


        var npcTargets = agent.AIDirector.CurrentNPCTargets;
        for (int i = 0; i < npcTargets.Count; ++i)
        {
            if (Vector3.SqrMagnitude(npcTargets[i].transform.position - agent.transform.position) < (agent.DetectionRange * agent.DetectionRange) && !npcTargets[i].Dead)
            {
                agent.CurrentCharacterTarget = npcTargets[i];
                return true;
            }
        }

        return false;
    }
}