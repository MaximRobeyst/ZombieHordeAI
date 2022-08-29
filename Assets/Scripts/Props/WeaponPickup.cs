using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Weapon, Interactible
{
    [SerializeField] private Transform m_MuzzleTransform;

    [SerializeField] private float m_Damage = 10.0f;
    [SerializeField] private float m_Range = 100.0f;

    [SerializeField] private float m_FireRate = 5.0f;
    private bool m_AllowFire = true;


    public void Interacting(GameObject interactingObject)
    {
        WeaponController weaponController = interactingObject.GetComponent<WeaponController>();

        if (weaponController != null)
            weaponController.EquipWeapon(this);
    }

    public override void Fire()
    {
        if (!m_AllowFire) return;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, m_Range))
        {
            if(hit.transform.tag == "Prop" || hit.transform.tag == "Enemy")
            {
                HealthComponent healthComponent = hit.transform.GetComponent<HealthComponent>();
                if (healthComponent != null)
                    healthComponent.DoDamage(m_Damage);
            }

            Debug.DrawRay(m_MuzzleTransform.position, Camera.main.transform.forward * hit.distance, Color.red, 1f);
        }
        else
        {
            Debug.DrawRay(m_MuzzleTransform.position, Camera.main.transform.forward * m_Range, Color.red);
        }

        StartCoroutine(FireRate());
    }

    IEnumerator FireRate()
    {
        m_AllowFire = false;
        yield return new WaitForSeconds(m_FireRate);
        m_AllowFire = true;
    }
}
