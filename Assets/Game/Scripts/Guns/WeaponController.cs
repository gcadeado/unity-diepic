using System;
using UnityEngine;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    [Header("Internal References")]
    [Tooltip("The root object for the weapon")]
    public GameObject weaponRoot;

    public WeaponData weaponData;

    [Tooltip("Tip of the weapon, where the projectiles are shot")]
    public Transform weaponMuzzle;

    public UnityAction onShoot;

    public int projectilesLeft;
    public int projectileMax = 12;
    public bool infiniteProjectiles = false;

    float m_LastTimeShot = Mathf.NegativeInfinity;

    public GameObject owner
    {
        get; set;
    }

    public GameObject sourcePrefab
    {
        get; set;
    }

    public bool isWeaponActive
    {
        get; private set;
    }

    public Vector3 muzzleWorldVelocity
    {
        get; private set;
    }

    void Awake()
    {
        if (infiniteProjectiles)
            projectileMax = Int32.MaxValue;

        projectilesLeft = projectileMax;
    }

    void UpdateAmmo(int amount)
    {
        if (amount == 0)
        {
            return;
        }

        projectilesLeft += amount;
        projectilesLeft = Mathf.Clamp(projectilesLeft,
            0,
            projectileMax);
    }

    public void UseAmmo(int amount)
    {
        if (!infiniteProjectiles)
        {
            UpdateAmmo(-amount);
        }
        m_LastTimeShot = Time.time;
    }

    public void AddAmmo(int amount)
    {
        UpdateAmmo(amount);
    }

    public void ShowWeapon(bool show)
    {
        weaponRoot.SetActive(show);

        isWeaponActive = show;
    }

    public bool HandleShootInputs(bool isFiring)
    {
        if (!isFiring)
            return false;

        return TryShoot();
    }

    bool TryShoot()
    {
        if (
            projectilesLeft > 0 &&
            m_LastTimeShot + weaponData.delayBetweenShots < Time.time
        )
        {
            HandleShoot();
            UseAmmo(1);
            return true;
        }

        return false;
    }

    void HandleShoot()
    {
        Vector3 shotDirection = GetShotDirectionWithinSpread(weaponMuzzle);
        ProjectileBase newProjectile =
            Instantiate(
                weaponData.projectilePrefab,
                weaponMuzzle.position,
                Quaternion.LookRotation(shotDirection));
        newProjectile.Shoot(this);

        m_LastTimeShot = Time.time;

        // Callback on shoot
        if (onShoot != null)
        {
            onShoot();
        }
    }

    public Vector3 GetShotDirectionWithinSpread(Transform shootTransform)
    {
        float spreadAngleRatio = weaponData.bulletSpreadAngle / 180f;
        Vector2 randomAngle = UnityEngine.Random.insideUnitCircle;
        Vector3 spreadWorldDirection =
            Vector3
                .Slerp(shootTransform.up,
                new Vector3(randomAngle.x, randomAngle.y, 0f),
                spreadAngleRatio);

        return spreadWorldDirection;
    }
}
