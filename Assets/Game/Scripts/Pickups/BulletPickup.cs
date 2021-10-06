using UnityEngine;

public class BulletPickup : MonoBehaviour
{
    [Tooltip("Minimum amount of bullets")]
    public int bulletAmountMin = 6;
    [Tooltip("Maximum amount of bullets")]
    public int bulletAmountMax = 6;

    public WeaponController weaponController; // TODO this is bad, CHANGE THIS

    Pickup m_Pickup;

    void Start()
    {
        m_Pickup = GetComponent<Pickup>();
        DebugUtility.HandleErrorIfNullGetComponent<Pickup, BulletPickup>(m_Pickup, this, gameObject);

        // Subscribe to pickup action
        m_Pickup.onPick += OnPicked;
    }

    void OnPicked(PlayerController player)
    {
        PlayerWeaponsManager playerWeaponsManager = player.GetComponent<PlayerWeaponsManager>();
        if (playerWeaponsManager && playerWeaponsManager.AddAmmo(weaponController, Random.Range(bulletAmountMin, bulletAmountMax + 1)))
        {
            Destroy(gameObject);
        }
    }
}
