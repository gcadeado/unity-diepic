using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Tooltip("Maximum amount of health")]
    public float maxHealth = 100f;

    public UnityAction<float, GameObject> onDamaged;
    public UnityAction<float> onHealed;
    public UnityAction<GameObject> onDie;

    public float currentHealth { get; set; }

    public bool invincible { get; set; }

    bool m_IsDead;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        m_IsDead = false;
    }

    public void Heal(float healAmount)
    {
        float healthBefore = currentHealth;
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        float trueHealAmount = currentHealth - healthBefore;
        if (trueHealAmount > 0f && onHealed != null)
            onHealed.Invoke(trueHealAmount);
    }

    public void TakeDamage(float damage, GameObject damageSource)
    {
        if (invincible)
            return;

        float healthBefore = currentHealth;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        float trueDamageAmount = healthBefore - currentHealth;
        if (trueDamageAmount > 0f && onDamaged != null)
            onDamaged.Invoke(trueDamageAmount, damageSource);

        HandleDeath(damageSource);
    }

    public void Kill(GameObject killSource)
    {
        currentHealth = 0f;

        if (onDamaged != null)
            onDamaged.Invoke(maxHealth, null);

        HandleDeath(killSource);
    }

    private void HandleDeath(GameObject source)
    {
        if (m_IsDead || currentHealth > 0f)
            return;

        m_IsDead = true;

        if (onDie != null)
            onDie.Invoke(source);
    }
}
