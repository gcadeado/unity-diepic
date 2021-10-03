using UnityEngine;

public class Destructible : MonoBehaviour
{
    Health m_Health;

    [Tooltip("Score value given on destruction")]
    public int scoreValue = 10;

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

    void OnDie(GameObject killerSource)
    {
        // this will call the OnDestroy function
        killerSource.GetComponent<PlayerController>().AddScore(scoreValue);
        Destroy(gameObject);
    }
}
