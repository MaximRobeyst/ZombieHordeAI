using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Weapon m_Weapon01;
    [SerializeField] private Transform m_WeaponPosition;

    private void Start()
    {
        if (m_WeaponPosition == null) return;

        m_Weapon01 = m_WeaponPosition.GetComponentInChildren<Weapon>();
        m_Weapon01.GetComponent<Rigidbody>().isKinematic = true;
        m_Weapon01.GetComponent<Collider>().isTrigger = true;
    }

    private void Update()
    {
        if (m_Weapon01 == null) return;

        if (Input.GetMouseButton(0)) m_Weapon01.Fire();

    }

    public void EquipWeapon(Weapon weapon)
    {
        if (m_Weapon01 != null) UnequipWeapon(m_Weapon01);

        weapon.transform.SetParent(m_WeaponPosition);
        weapon.transform.position = m_WeaponPosition.position;
        weapon.transform.localRotation = Quaternion.identity;

        weapon.GetComponent<Rigidbody>().isKinematic = true;
        weapon.GetComponent<Collider>().isTrigger = true;
        m_Weapon01 = weapon;
    }
    
    public void UnequipWeapon(Weapon weapon)
    {
        weapon.transform.SetParent(null);
        weapon.transform.position = transform.position + transform.up + transform.forward;
        weapon.GetComponent<Rigidbody>().isKinematic = false;
        weapon.GetComponent<Collider>().isTrigger = false;

        m_Weapon01 = null;
    }
}
