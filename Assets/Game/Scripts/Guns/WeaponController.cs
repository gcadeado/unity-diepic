using UnityEngine;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
    [Header("Information")]
    [Tooltip("The name that will be displayed in the UI for this weapon")]
    public string weaponName;

    [Header("Internal References")]
    [
        Tooltip(
            "The root object for the weapon, this is what will be deactivated when the weapon isn't active")
    ]
    public GameObject weaponRoot;

    [Tooltip("Tip of the weapon, where the projectiles are shot")]
    public Transform weaponMuzzle;

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

    [Tooltip("Maximum amount of ammo in the gun")]
    public float maxAmmo = 8;

    [Tooltip("Force that will push back the weapon after each shot")]
    [Range(0f, 2f)]
    public float recoilForce = 1;

    private bool m_wantsToShoot = false;

    public UnityAction onShoot;

    float m_CurrentAmmo;

    public float CurrentAmmo
    {
        get
        {
            return m_CurrentAmmo;
        }
        set
        {
            m_CurrentAmmo = value;
        }
    }

    float m_LastTimeShot = Mathf.NegativeInfinity;

    Vector3 m_LastMuzzlePosition;

    public GameObject owner { get; set; }

    public GameObject sourcePrefab { get; set; }

    public bool isWeaponActive { get; private set; }

    public Vector3 muzzleWorldVelocity { get; private set; }

    void Awake()
    {
        m_CurrentAmmo = maxAmmo;
        m_LastMuzzlePosition = weaponMuzzle.position;
    }

    void Update()
    {
        if (Time.deltaTime > 0)
        {
            muzzleWorldVelocity =
                (weaponMuzzle.position - m_LastMuzzlePosition) / Time.deltaTime;
            m_LastMuzzlePosition = weaponMuzzle.position;
        }
    }

    void UpdateAmmo(int amount)
    {
        if (amount == 0)
        {
            return;
        }

        // update weapon
        m_CurrentAmmo += amount;

        // limits ammo to min and max value
        m_CurrentAmmo = Mathf.Clamp(m_CurrentAmmo, 0, maxAmmo);
    }

    public void UseAmmo(int amount)
    {
        UpdateAmmo(-amount);
        m_LastTimeShot = Time.time;
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
            m_CurrentAmmo >= 1f &&
            m_LastTimeShot + delayBetweenShots < Time.time
        )
        {
            HandleShoot();
            m_CurrentAmmo -= 1f;

            return true;
        }

        return false;
    }

    void HandleShoot()
    {
        Vector3 shotDirection = GetShotDirectionWithinSpread(weaponMuzzle);
        ProjectileBase newProjectile =
            Instantiate(projectilePrefab,
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
        float spreadAngleRatio = bulletSpreadAngle / 180f;
        Vector2 randomAngle = UnityEngine.Random.insideUnitCircle;
        Vector3 spreadWorldDirection =
            Vector3
                .Slerp(shootTransform.up,
                new Vector3(randomAngle.x, randomAngle.y, 0f),
                spreadAngleRatio);

        return spreadWorldDirection;
    }
}
