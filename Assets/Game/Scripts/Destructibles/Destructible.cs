using UnityEngine;

public class Destructible : MonoBehaviour
{
    Health m_Health;

    void Start()
    {
        m_Health = GetComponent<Health>();
        DebugUtility.HandleErrorIfNullGetComponent<Health, Destructible>(m_Health, this, gameObject);

        // Subscribe to damage & death actions
        m_Health.onDie += OnDie;
        m_Health.onDamaged += OnDamaged;
    }

    void OnDamaged(float damage, GameObject damageSource)
    {
        // TODO
    }

    void OnDie()
    {
        // this will call the OnDestroy function
        Destroy(gameObject);
    }
}
