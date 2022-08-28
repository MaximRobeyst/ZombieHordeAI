using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathTransition : IFSMTransition
{
    public override bool ToTransition(GameAgent agent)
    {
        return agent.AIDirector.CurrentNPCTargets[0].Dead;
    }
}
