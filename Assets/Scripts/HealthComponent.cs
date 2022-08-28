using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    private float m_Health;
    [SerializeField] private float m_MaxHealth;

    private int m_CharactersTargeting;

    private void Start()
    {
        m_Health = m_MaxHealth;
    }

    public bool Dead { get { return m_Health <= 0; } }
    public void DoDamage(float damage) { m_Health -= damage; }
    public float GetHealth() { return m_Health; }
    public float GetMaxHealth() { return m_MaxHealth; }
}
