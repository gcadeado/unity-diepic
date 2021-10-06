﻿using TMPro;
using UnityEngine;

// TODO refactor this mess to use GameEvents
[RequireComponent(typeof(TextMeshProUGUI))]
public class BulletCountUI : MonoBehaviour
{
    PlayerWeaponsManager m_PlayerWeaponsManager;

    [SerializeField]
    WeaponController m_Weapon;

    void Update()
    {
        m_PlayerWeaponsManager = FindObjectOfType<PlayerLocalController>().GetComponent<PlayerWeaponsManager>();
        int currentProjectilesLeft = m_PlayerWeaponsManager.GetAmmo(m_Weapon);
        TextMeshProUGUI textMeshPro = GetComponent<TextMeshProUGUI>();
        textMeshPro.text = currentProjectilesLeft.ToString();
    }
}
