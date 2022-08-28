using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderState : IFSMState
{
	Vector3 m_Target;

	float m_OffsetDistance = 4.0f;
	float m_WanderAngle = 0.0f;
	float m_Radius = 2.0f;
	float m_AngleChange = 10.0f;

	NavMeshAgent m_NavmeshAgent;

    public WanderState(GameAgent gameAgent) : base(gameAgent)
    {
    }

    public override void OnEnter()
    {
		if (m_NavmeshAgent == null)
			m_NavmeshAgent = m_Agent.GetComponent<NavMeshAgent>();

		m_NavmeshAgent.isStopped = false;
		m_NavmeshAgent.speed = m_Agent.WanderSpeed;
    }

    public override void Update()
	{
		var agentTransform = m_Agent.transform;

		m_Target = agentTransform.position + agentTransform.forward * m_OffsetDistance + (new Vector3(Mathf.Cos(m_WanderAngle) * m_Radius, Mathf.Sin(m_WanderAngle) * m_Radius));
		m_WanderAngle +=Random.Range(-m_AngleChange, m_AngleChange);

		//Vector3 targetDir = m_Target - agentTransform.position;

		m_NavmeshAgent.SetDestination(m_Target);
	}

    public override void DrawGizmos()
	{
		var agentTransform = m_Agent.transform;
		Gizmos.color = Color.red;
		Gizmos.DrawRay(agentTransform.position, agentTransform.forward * m_OffsetDistance);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(agentTransform.position + agentTransform.forward * m_OffsetDistance, m_Radius);
		Gizmos.color = Color.green;
		Gizmos.DrawRay(agentTransform.position, m_Target - agentTransform.position);
	}

    public override void OnExit()
    {
		m_NavmeshAgent.isStopped = true;
	}
}
