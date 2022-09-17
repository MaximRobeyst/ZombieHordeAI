using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Interactible
{
    [SerializeField] private Transform m_MuzzleTransform;

    [SerializeField] private float m_Damage = 10.0f;
    [SerializeField] private float m_Range = 100.0f;

    [SerializeField] private float m_FireRate = 5.0f;
    private bool m_AllowFire = true;

    private Camera m_CurrentCamera;

    public void SetCurrentCamera(Camera camera)
    {
        m_CurrentCamera = camera;
    }

    [Command(requiresAuthority = false)]
    public override void CmdInteract(GameObject interactingObject)
    {
        WeaponController weaponController = interactingObject.GetComponent<WeaponController>();
        m_CurrentCamera = interactingObject.GetComponentInChildren<Camera>();

        if (weaponController != null)
            weaponController.EquipWeapon(this);

        base.CmdInteract(interactingObject);
    }

    [ClientRpc]
    public override void RPCInteract(GameObject interactingObject)
    {
        WeaponController weaponController = interactingObject.GetComponent<WeaponController>();
        m_CurrentCamera = interactingObject.GetComponentInChildren<Camera>();

        if (weaponController != null)
            weaponController.EquipWeapon(this);
    }

    public void Fire()
    {
        if (!m_AllowFire) return;

        RaycastHit hit;
        if(Physics.Raycast(m_CurrentCamera.transform.position, m_CurrentCamera.transform.forward, out hit, m_Range))
        {
            if(hit.transform.tag == "Prop" || hit.transform.tag == "Enemy")
            {
                HealthComponent healthComponent = hit.transform.GetComponent<HealthComponent>();
                if (healthComponent != null)
                    healthComponent.DoDamage(m_Damage);
            }

            Debug.DrawRay(m_MuzzleTransform.position, m_CurrentCamera.transform.forward * hit.distance, Color.red, 1f);
        }
        else
        {
            Debug.DrawRay(m_MuzzleTransform.position, m_CurrentCamera.transform.forward * m_Range, Color.red);
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
