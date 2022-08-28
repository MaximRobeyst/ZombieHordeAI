using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    [SerializeField] private HealthComponent m_TargetHealthComponent;
    Slider m_Slider;

    private void Start()
    {

        if (m_TargetHealthComponent == null) return;

        m_Slider = GetComponent<Slider>();
        m_Slider.maxValue = m_TargetHealthComponent.GetMaxHealth();
    }

    private void Update()
    {
        if (m_Slider == null || m_TargetHealthComponent == null) return;

        m_Slider.value = m_TargetHealthComponent.GetHealth();
    }
}
