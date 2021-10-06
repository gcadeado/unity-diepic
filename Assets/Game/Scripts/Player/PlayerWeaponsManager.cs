using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerWeaponsManager : MonoBehaviour
{
    public enum WeaponSwitchState
    {
        Up,
        Down,
        PutDownPrevious,
        PutUpNew,
    }

    [Tooltip("List of weapon the player will start with")]
    public List<WeaponController> startingWeapons = new List<WeaponController>();

    [Header("References")]
    [Tooltip("Parent transform where all weapon will be added in the hierarchy")]
    public Transform weaponParentSocket;

    public int activeWeaponIndex
    {
        get; private set;
    }
    public UnityAction<WeaponController> onSwitchedToWeapon;
    WeaponController[] m_WeaponSlots = new WeaponController[6]; // 6 available weapon slots

    public bool isFiring;
    public int switchWeaponInput = 0;

    WeaponSwitchState m_WeaponSwitchState;
    int m_WeaponSwitchNewWeaponIndex;

    private void Start()
    {
        activeWeaponIndex = -1;
        m_WeaponSwitchState = WeaponSwitchState.Down;

        onSwitchedToWeapon += OnWeaponSwitched;

        // Add starting weapons
        foreach (var weapon in startingWeapons)
        {
            AddWeapon(weapon);
        }
        SwitchWeapon(true);
    }

    private void Update()
    {
        // shoot handling
        WeaponController activeWeapon = GetActiveWeapon();
        if (activeWeapon && m_WeaponSwitchState == WeaponSwitchState.Up)
        {
            activeWeapon.HandleShootInputs(isFiring);
        }

        // weapon switch handling
        if (switchWeaponInput != 0
            && GetWeaponAtSlotIndex(switchWeaponInput - 1) != null
            && m_WeaponSwitchState == WeaponSwitchState.Up
            || m_WeaponSwitchState == WeaponSwitchState.Down)
            SwitchToWeaponIndex(switchWeaponInput - 1);
    }

    private void LateUpdate()
    {
        UpdateWeaponSwitching();
    }

    // Iterate on all weapon slots to find the next valid weapon to switch to
    public void SwitchWeapon(bool ascendingOrder)
    {
        int newWeaponIndex = -1;
        int closestSlotDistance = m_WeaponSlots.Length;
        for (int i = 0; i < m_WeaponSlots.Length; i++)
        {
            // If the weapon at this slot is valid, calculate its "distance" from the active slot index (either in ascending or descending order)
            // and select it if it's the closest distance yet
            if (i != activeWeaponIndex && GetWeaponAtSlotIndex(i) != null)
            {
                int distanceToActiveIndex = GetDistanceBetweenWeaponSlots(activeWeaponIndex, i, ascendingOrder);

                if (distanceToActiveIndex < closestSlotDistance)
                {
                    closestSlotDistance = distanceToActiveIndex;
                    newWeaponIndex = i;
                }
            }
        }

        // Handle switching to the new weapon index
        SwitchToWeaponIndex(newWeaponIndex);
    }

    // Switches to the given weapon index in weapon slots if the new index is a valid weapon that is different from our current one
    public void SwitchToWeaponIndex(int newWeaponIndex, bool force = false)
    {
        if (force || (newWeaponIndex != activeWeaponIndex && newWeaponIndex >= 0))
        {
            // Store data related to weapon switching animation
            m_WeaponSwitchNewWeaponIndex = newWeaponIndex;

            // Handle case of switching to a valid weapon for the first time (simply put it up without putting anything down first)
            if (GetActiveWeapon() == null)
            {
                m_WeaponSwitchState = WeaponSwitchState.PutUpNew;
                activeWeaponIndex = m_WeaponSwitchNewWeaponIndex;

                WeaponController newWeapon = GetWeaponAtSlotIndex(m_WeaponSwitchNewWeaponIndex);
                if (onSwitchedToWeapon != null)
                {
                    onSwitchedToWeapon.Invoke(newWeapon);
                }
            }
            // otherwise, remember we are putting down our current weapon for switching to the next one
            else
            {
                m_WeaponSwitchState = WeaponSwitchState.PutDownPrevious;
            }
        }
    }

    // Updates the animated transition of switching weapons
    void UpdateWeaponSwitching()
    {
        if (m_WeaponSwitchState == WeaponSwitchState.PutDownPrevious)
        {
            // Deactivate old weapon
            WeaponController oldWeapon = GetWeaponAtSlotIndex(activeWeaponIndex);
            if (oldWeapon != null)
            {
                oldWeapon.ShowWeapon(false);
            }

            activeWeaponIndex = m_WeaponSwitchNewWeaponIndex;

            // Activate new weapon
            WeaponController newWeapon = GetWeaponAtSlotIndex(activeWeaponIndex);
            if (onSwitchedToWeapon != null)
            {
                onSwitchedToWeapon.Invoke(newWeapon);
            }

            if (newWeapon)
            {
                m_WeaponSwitchState = WeaponSwitchState.PutUpNew;
            }
            else
            {
                // if new weapon is null, don't follow through with putting weapon back up
                m_WeaponSwitchState = WeaponSwitchState.Down;
            }
        }
        else if (m_WeaponSwitchState == WeaponSwitchState.PutUpNew)
        {
            m_WeaponSwitchState = WeaponSwitchState.Up;
        }
    }

    // Adds bullets for weapon
    public bool AddAmmo(WeaponController weaponPrefab, int ammount)
    {
        if (!HasWeapon(weaponPrefab))
            return false;
        foreach (var w in m_WeaponSlots)
        {
            if (w != null && w.sourcePrefab == weaponPrefab.gameObject)
            {
                w.AddAmmo(ammount);
                return true;
            }
        }
        return false;

    }

    public WeaponController GetWeapon(WeaponController weaponPrefab)
    {
        foreach (var w in m_WeaponSlots)
        {
            if (w != null && w.sourcePrefab == weaponPrefab.gameObject)
            {
                return w;
            }
        }
        return null;
    }

    public bool HasWeapon(WeaponController weaponPrefab)
    {
        WeaponController weapon = GetWeapon(weaponPrefab);
        return weapon;
    }

    public int GetAmmo(WeaponController weapon)
    {
        WeaponController weaponController = GetWeapon(weapon);
        if (weapon)
        {
            return weaponController.projectilesLeft;
        }
        return 0;
    }

    public int GetMaxAmmo(WeaponController weapon)
    {
        WeaponController weaponController = GetWeapon(weapon);
        if (weapon)
        {
            return weaponController.projectileMax;
        }
        return 0;
    }

    // Adds a weapon to our inventory
    public bool AddWeapon(WeaponController weaponPrefab)
    {
        // if we already hold this weapon type (a weapon coming from the same source prefab), don't add the weapon
        if (HasWeapon(weaponPrefab))
            return false;

        // search our weapon slots for the first free one, assign the weapon to it, and return true if we found one. Return false otherwise
        for (int i = 0; i < m_WeaponSlots.Length; i++)
        {
            // only add the weapon if the slot is free
            if (m_WeaponSlots[i] == null)
            {
                // spawn the weapon prefab as child of the weapon socket
                WeaponController weaponInstance = Instantiate(weaponPrefab, weaponParentSocket);
                weaponInstance.transform.localPosition = Vector3.zero;
                weaponInstance.transform.localRotation = Quaternion.identity;

                // Set owner to this gameObject so the weapon can alter projectile/damage logic accordingly
                weaponInstance.owner = gameObject;
                weaponInstance.sourcePrefab = weaponPrefab.gameObject;
                weaponInstance.ShowWeapon(false);

                m_WeaponSlots[i] = weaponInstance;

                return true;
            }
        }

        // Handle auto-switching to weapon if no weapons currently
        if (GetActiveWeapon() == null)
            SwitchWeapon(true);

        return false;
    }

    public bool RemoveWeapon(WeaponController weaponInstance)
    {
        // Look through our slots for that weapon
        for (int i = 0; i < m_WeaponSlots.Length; i++)
        {
            // when weapon found, remove it
            if (m_WeaponSlots[i] == weaponInstance)
            {
                m_WeaponSlots[i] = null;

                Destroy(weaponInstance.gameObject);

                // Handle case of removing active weapon (switch to next weapon)
                if (i == activeWeaponIndex)
                {
                    SwitchWeapon(true);
                }

                return true;
            }
        }

        return false;
    }

    public WeaponController GetActiveWeapon()
    {
        return GetWeaponAtSlotIndex(activeWeaponIndex);
    }

    public WeaponController GetWeaponAtSlotIndex(int index)
    {
        // find the active weapon in our weapon slots based on our active weapon index
        if (index >= 0 && index < m_WeaponSlots.Length)
        {
            return m_WeaponSlots[index];
        }

        // if we didn't find a valid active weapon in our weapon slots, return null
        return null;
    }

    // Calculates the "distance" between two weapon slot indexes
    // For example: if we had 5 weapon slots, the distance between slots #2 and #4 would be 2 in ascending order, and 3 in descending order
    int GetDistanceBetweenWeaponSlots(int fromSlotIndex, int toSlotIndex, bool ascendingOrder)
    {
        int distanceBetweenSlots = 0;

        if (ascendingOrder)
        {
            distanceBetweenSlots = toSlotIndex - fromSlotIndex;
        }
        else
        {
            distanceBetweenSlots = -1 * (toSlotIndex - fromSlotIndex);
        }

        if (distanceBetweenSlots < 0)
        {
            distanceBetweenSlots = m_WeaponSlots.Length + distanceBetweenSlots;
        }

        return distanceBetweenSlots;
    }

    void OnWeaponSwitched(WeaponController newWeapon)
    {
        if (newWeapon != null)
        {
            newWeapon.ShowWeapon(true);
        }
    }
}