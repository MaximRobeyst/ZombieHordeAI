using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInAttackRangeTransition : IFSMTransition
{
    public override bool ToTransition(GameAgent agent)
    {
        // using sqrMagnitude to avoid Squareroot operation seeing as a lot of characters should be able to be on screen this could save a few frames
        return Vector3.SqrMagnitude(agent.CurrentCharacterTarget.transform.position - agent.transform.position) < (agent.AttackRange * agent.AttackRange);
    }
}
