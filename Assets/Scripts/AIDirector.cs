using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIDirector : NetworkBehaviour
{
    // This variable is not synced and causes issues on the other client
    public List<HealthComponent> CurrentTargets = new List<HealthComponent>();

    [SerializeField] private float m_MinTimeBetweenMobAttacks = 90.0f;
    [SerializeField] private float m_MaxTimeBetweenMobAttacks = 180.0f;
    private float m_TimeTillNextMob;

    [SerializeField] private List<GameAgent> m_DefaultAgents;
    [SerializeField] private List<GameAgent> m_SpecialAgents;
    [SerializeField] private List<GameAgent> m_BossAgents;

    private InfoGrid m_InfoGrid;

    private int m_CurrentSpawnLimit = 30;
    private int m_SpecialAgentNumber = 0;
    private float m_CurrentTimer = 0;

    private bool m_Running = true;

    private int m_CurrentEnemyCount;

    [Command]
    public void RegisterHealthComponent(HealthComponent healthComponent)
    {
        CurrentTargets.Add(healthComponent);
        Debug.Log($"Registered new healthcomponent {healthComponent.gameObject.name}");
    }

    [Command]
    public void UnRegisterHealthComponent(HealthComponent healthComponent)
    {
        CurrentTargets.Remove(healthComponent);
        Debug.Log($"Unregistered new healthcomponent {healthComponent.gameObject.name}");
    }

    // Start is called before the first frame update
    void Start()
    {
        m_InfoGrid = GetComponent<InfoGrid>();

        m_TimeTillNextMob = Random.Range(m_MinTimeBetweenMobAttacks, m_MaxTimeBetweenMobAttacks);
    }

    void Update()
    {
        if (!isServer) return;

        for(int i = 0; i < CurrentTargets.Count;++i)
            m_InfoGrid.UpdateVisibility(CurrentTargets[i]);

        m_CurrentTimer += Time.deltaTime;

        if (m_CurrentTimer >= m_TimeTillNextMob)
        {
            StartMob();
            m_CurrentTimer = 0;
        }
        else
        {
            SpawnEnemiesInArea();
        }
    }

    void SpawnEnemiesInArea()
    {
        if (!isServer) return;
        if (m_CurrentEnemyCount >= m_CurrentSpawnLimit) return;

        GameObject defaultZombie = Instantiate(m_DefaultAgents[Random.Range(0, m_DefaultAgents.Count)], m_InfoGrid.GetSuitableSpawnPosition(), Quaternion.identity).gameObject;
        NetworkServer.Spawn(defaultZombie, connectionToServer);
        ++m_CurrentEnemyCount;
    }

    void StartMob()
    {
        Debug.Log("Starting mob");
        m_TimeTillNextMob = Random.Range(m_MinTimeBetweenMobAttacks, m_MaxTimeBetweenMobAttacks);



    }
}
