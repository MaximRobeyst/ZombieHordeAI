using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawnSystem : NetworkBehaviour
{
    [SerializeField] private GameObject m_PlayerPrefab = null;
    private static List<Transform> m_SpawnPoints = new List<Transform>();
    private int m_NextIndex = 0;

    private AIDirector m_AIDirector;

    public static void AddSpawnPoint(Transform transform)
    {
        m_SpawnPoints.Add(transform);
        m_SpawnPoints = m_SpawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform transform)
    {
        m_SpawnPoints.Remove(transform);
    }

    public override void OnStartServer()
    {
        if (m_AIDirector == null) m_AIDirector = GameObject.FindGameObjectWithTag("Manager").GetComponent<AIDirector>();

        NetworkMangerLobby.OnServerReadied += SpawnPlayer;
    }

    [ServerCallback]
    private void OnDestroy()
    {
        NetworkMangerLobby.OnServerReadied -= SpawnPlayer;
    }

    [Server]
    public void SpawnPlayer(NetworkConnection conn)
    {
        Transform spawnPoint = m_SpawnPoints.ElementAtOrDefault(m_NextIndex);

        if (spawnPoint == null)
        {
            Debug.Log("Missing spawn point for player");
            return;
        }

        GameObject playerInstance = Instantiate(m_PlayerPrefab, m_SpawnPoints[m_NextIndex].position, m_SpawnPoints[m_NextIndex].rotation);
        NetworkServer.Spawn(playerInstance, conn);

        m_AIDirector.CurrentNPCTargets.Add(playerInstance.GetComponent<HealthComponent>());

        ++m_NextIndex;
    }

}
