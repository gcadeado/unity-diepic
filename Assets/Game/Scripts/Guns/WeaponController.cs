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
    private bool m_wantsToShoot = false;

    public UnityAction onShoot;

    float m_LastTimeShot = Mathf.NegativeInfinity;

    public GameObject owner { get; set; }

    public GameObject sourcePrefab { get; set; }

    public bool isWeaponActive { get; private set; }

    public Vector3 muzzleWorldVelocity { get; private set; }

    void UpdateAmmo(int amount)
    {
        if (amount == 0)
        {
            return;
        }

        weaponData.projectileInventory.Value += amount;
        weaponData.projectileInventory.Value = Mathf.Clamp(
            (int)weaponData.projectileInventory.Value,
            0,
            weaponData.projectileInventoryMax.Value);
    }

    public void UseAmmo(int amount)
    {
        UpdateAmmo(-amount);
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

    public bool HandleShootInputs(bool inputDown, bool inputHeld, bool inputUp)
    {
        m_wantsToShoot = inputDown || inputHeld;

        if (m_wantsToShoot)
        {
            return TryShoot();
        }
        return false;
    }

    bool TryShoot()
    {
        if (
            weaponData.projectileInventory.Value >= 1 &&
            m_LastTimeShot + weaponData.delayBetweenShots < Time.time
        )
        {
            HandleShoot();
            weaponData.projectileInventory.Value -= 1;

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
