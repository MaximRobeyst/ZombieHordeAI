using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDirector : MonoBehaviour
{
    public List<HealthComponent> CurrentNPCTargets { get; } = new List<HealthComponent>();

    // Start is called before the first frame update
    void Start()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < players.Length; ++i)
        {
            CurrentNPCTargets.Add(players[i].GetComponent<HealthComponent>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
