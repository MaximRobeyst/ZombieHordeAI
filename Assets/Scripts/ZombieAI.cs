using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZombieAI : GameAgent
{
    private TextMeshProUGUI m_Text;

    [SerializeField] private Transform m_HandTransform;

    // Start is called before the first frame update
    void Awake()
    {
        Setup();

        m_Text = GetComponentInChildren<TextMeshProUGUI>();

        var wanderState = new WanderState(this);
        var chaseState = new ChaseState(this);
        var attackState = new AttackState(this);
        var deatthState = new DeathState(this);

        var characterInViewTransition = new CharacterInViewTransition();
        var playerDeathTransition = new PlayerDeathTransition();
        var playerInAttackRange = new CharacterInAttackRangeTransition();
        var propInTheWayTransition = new PropInTheWayTransition();
        var IsDeadTransition = new CharacterIsDeadTransition();

        m_StateMachine = GetComponent<FiniteStateMachine>();
        m_StateMachine.CurrentState = wanderState;
        m_StateMachine.m_Agent = this;

        m_StateMachine.AddTransition(chaseState, wanderState, playerDeathTransition);
        m_StateMachine.AddTransition(chaseState, attackState, playerInAttackRange);
        m_StateMachine.AddTransition(wanderState, chaseState, characterInViewTransition);
        m_StateMachine.AddTransition(attackState, chaseState, new InverseTransition(playerInAttackRange));
        m_StateMachine.AddTransition(chaseState, attackState, propInTheWayTransition);
        m_StateMachine.AddTransition(chaseState, wanderState, playerDeathTransition);

        m_StateMachine.AddTransition(chaseState, deatthState, IsDeadTransition);
        m_StateMachine.AddTransition(attackState, deatthState, IsDeadTransition);
        m_StateMachine.AddTransition(wanderState, deatthState, IsDeadTransition);

        m_StateMachine.ChangeState(wanderState);
    }

    // Update is called once per frame
    void Update()
    {
        m_StateMachine.UpdateStateMachine();

        m_Text.text = m_StateMachine.GetCurrentStateName();
    }

    private void OnDrawGizmos()
    {
        if(m_StateMachine != null)
            m_StateMachine.Gizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_HandTransform.position, 0.25f);
    }

    // Animation Event
    public void Ragdoll()
    {
        ActivateRigidbody(true);
    }

    public void Attack()
    {
        var attackRay = new Ray(transform.position + (transform.forward * 0.25f) + (transform.up), transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(attackRay, out hit, AttackRange, AttackMask))
        {
            Debug.DrawRay(attackRay.origin, attackRay.direction * hit.distance, Color.red, 1.0f);
            HealthComponent healthComponent = hit.transform.GetComponent<HealthComponent>();
            if (healthComponent != null)
                healthComponent.DoDamage(Damage);
        }
        else
        {
            Debug.DrawRay(attackRay.origin, attackRay.direction * AttackRange, Color.red, 1.0f);
        }
    }


}
