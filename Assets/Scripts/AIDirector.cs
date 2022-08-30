using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDirector : MonoBehaviour
{
    public List<HealthComponent> CurrentNPCTargets { get; } = new List<HealthComponent>();
    [SerializeField] private float m_MinTimeBetweenMobAttacks = 90.0f;
    [SerializeField] private float m_MaxTimeBetweenMobAttacks = 180.0f;
    private float m_TimeTillNextMob;

    // Start is called before the first frame update
    void Start()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; ++i)
        {
            CurrentNPCTargets.Add(players[i].GetComponent<HealthComponent>());
        }

        m_TimeTillNextMob = Random.Range(m_MinTimeBetweenMobAttacks, m_MaxTimeBetweenMobAttacks);
    }

    void StartMob()
    {

    }

}
