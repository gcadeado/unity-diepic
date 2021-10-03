using UnityEngine;

public class Damageable : MonoBehaviour
{
    public Health health { get; private set; }

    void Start()
    {
        health = GetComponent<Health>();
    }

    public void InflictDamage(float damage, GameObject damageSource)
    {
        if (health)
            health.TakeDamage(damage, damageSource);
    }
}
