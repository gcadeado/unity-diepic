using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Weapons Data", fileName = "new Weapons Data")]
public class WeaponData : GameEvent
{
    [Header("Information")]
    [Tooltip("The name that will be displayed in the UI for this weapon")]
    public string weaponName;

    [Header("Shoot Parameters")]
    [Tooltip("The projectile prefab")]
    public ProjectileBase projectilePrefab;

    [Tooltip("Minimum duration between two shots")]
    public float delayBetweenShots = 0.5f;

    [
        Tooltip(
            "Angle for the cone in which the bullets will be shot randomly (0 means no spread at all)")
    ]
    public float bulletSpreadAngle = 0f;
}
