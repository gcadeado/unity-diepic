using UnityEngine;

public class BulletPickup : MonoBehaviour
{
    [Header("Parameters")]
    public IntVariable projectileInventory;
    public IntVariable projectileInventoryMax;

    [Tooltip("Minimum amount of bullets")]
    public int bulletAmountMin = 6;
    [Tooltip("Maximum amount of bullets")]
    public int bulletAmountMax = 6;

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
            projectileInventory.Value = Mathf.Clamp(projectileInventory.Value, 0, projectileInventoryMax.Value);
            Destroy(gameObject);
        }
    }
}
