using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : NetworkBehaviour
{
    private WeaponPickup m_Weapon01;
    [SerializeField] private Transform m_WeaponPosition;

    private Camera m_Camera;

    private void Start()
    {
        if (m_WeaponPosition == null) return;
        m_Camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (m_Weapon01 == null || !hasAuthority) return;

        if (Input.GetMouseButton(0)) m_Weapon01.Fire();
    }

    public void EquipWeapon(WeaponPickup weapon)
    {
        if (m_Weapon01 != null) UnequipWeapon(m_Weapon01);

        weapon.transform.SetParent(m_WeaponPosition);
        weapon.transform.position = m_WeaponPosition.position;
        weapon.transform.localRotation = Quaternion.identity;

        weapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon.GetComponent<Collider>().isTrigger = true;
        m_Weapon01 = weapon;
    }
    
    public void UnequipWeapon(WeaponPickup weapon)
    {
        weapon.transform.SetParent(null);
        weapon.transform.position = transform.position + transform.up + transform.forward;
        weapon.GetComponent<Rigidbody>().isKinematic = false;
        weapon.GetComponent<Collider>().isTrigger = false;
        weapon.SetCurrentCamera(m_Camera);

        m_Weapon01 = null;
    }
}
