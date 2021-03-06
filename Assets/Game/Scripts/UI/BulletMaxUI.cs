using System;
using TMPro;
using UnityEngine;
// TODO refactor this mess to use GameEvents
[RequireComponent(typeof(TextMeshProUGUI))]
public class BulletMaxUI : MonoBehaviour
{
    PlayerWeaponsManager m_PlayerWeaponsManager;

    [SerializeField]
    WeaponController m_Weapon;

    void Update()
    {
        m_PlayerWeaponsManager = FindObjectOfType<PlayerLocalController>().GetComponent<PlayerWeaponsManager>();
        int currentProjectilesMax = m_PlayerWeaponsManager.GetMaxAmmo(m_Weapon);
        TextMeshProUGUI textMeshPro = GetComponent<TextMeshProUGUI>();

        if (currentProjectilesMax == Int32.MaxValue)
            textMeshPro.text = "∞";
        else
            textMeshPro.text = currentProjectilesMax.ToString();
    }
}
