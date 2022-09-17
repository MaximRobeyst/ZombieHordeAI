using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Interactible : NetworkBehaviour
{
    [Command(requiresAuthority = false)]
    public virtual void CmdInteract(GameObject interactingObject)
    {
        RPCInteract(interactingObject);
    }

    [ClientRpc]
    public virtual void RPCInteract(GameObject interactingObject)
    {

    }
}
