using UnityEngine;

public class BulletPickup : MonoBehaviour
{
    [Header("Parameters")]
    public IntVariable projectileInventory;

    [Tooltip("Minimum amount of bullets")]
    public int bulletAmountMin = 6;
    [Tooltip("Maximum amount of bullets")]
    public int bulletAmountMax = 12;

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
        if (playerWeaponsManager && playerWeaponsManager)
        {
            projectileInventory.Value += Random.Range(bulletAmountMin, bulletAmountMax + 1);
            Destroy(gameObject);
        }
    }
}
