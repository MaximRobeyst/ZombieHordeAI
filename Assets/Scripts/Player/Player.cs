using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static event Action<Player> OnPlayerSpawned;
    public static event Action<Player> OnPlayerDespawned;

    [SyncVar(hook = nameof(HandleOwnerSet))]
    private uint ownerId;

    public uint OwnerId => ownerId;

    private void OnDestroy()
    {
        OnPlayerDespawned?.Invoke(this);
    }

    private void HandleOwnerSet(uint oldValue, uint newValue)
    {
        OnPlayerSpawned?.Invoke(this);
    }
}
