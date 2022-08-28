using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    private float m_Health;
    [SerializeField] private float m_MaxHealth;
    [SerializeField] private bool m_DisableOnDeath = false;

    private int m_CharactersTargeting;

    private void Start()
    {
        m_Health = m_MaxHealth;
    }

    public bool Dead { get { return m_Health <= 0; } }
    public void DoDamage(float damage) 
    {
        m_Health -= damage;
        if (!m_DisableOnDeath) return;

        //if (m_Health <= 0) Destroy(gameObject); // Commenting till i find a better solution to handle target getting deleted
        if (m_Health <= 0) gameObject.SetActive(false);
    }
    public float GetHealth() { return m_Health; }
    public float GetMaxHealth() { return m_MaxHealth; }
}
