using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAgent : NetworkBehaviour
{
    public Vector3 CurrentTarget { get; set; }
    private AIDirector m_AIDirector;
    public AIDirector AIDirector { get { return m_AIDirector; } }

    private HealthComponent m_CurrentCharacterTarget;
    public HealthComponent CurrentCharacterTarget 
    { 
        get
        {
            return m_CurrentCharacterTarget;
        }
        set
        {
            m_CurrentCharacterTarget = value;
        } 
    }

    [SerializeField] private LayerMask m_AttackMask;
    public LayerMask AttackMask { get { return m_AttackMask; } }

    protected FiniteStateMachine m_StateMachine;

    // Blackboard or dictionary of float values seeing as other values we will not use different types as much
    [SerializeField] private float m_AttackRange = 1.0f;
    [SerializeField] private float m_DetectionRange = 10.0f;
    [SerializeField] private float m_Damage = 10.0f;
    [SerializeField] private float m_WanderSpeed = 1.0f;
    [SerializeField] private float m_ChaseSpeed = 6.0f;

    public float AttackRange { get { return m_AttackRange; } }
    public float DetectionRange { get { return m_DetectionRange; } }
    public float Damage { get { return m_Damage; } }
    public float WanderSpeed { get { return m_WanderSpeed; } }
    public float ChaseSpeed { get { return m_ChaseSpeed;  } }


    // Start is called before the first frame update
    public void Setup()
    {
        ActivateRigidbody(false);
        m_AIDirector = GameObject.FindGameObjectWithTag("Manager").GetComponent<AIDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
    }

    public void ActivateRigidbody(bool active)
    {
        Rigidbody[] ragdollbodies = GetComponentsInChildren<Rigidbody>();

        foreach (var rigidbody in ragdollbodies)
        {
            rigidbody.isKinematic = !active;
        }
    }
}
