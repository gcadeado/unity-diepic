using UnityEngine;

[RequireComponent(typeof(Destructible))]
public class LootContainer : MonoBehaviour
{
    [Header("Parameters")]
    public GameObject[] pickups;

    Health m_Health; // TODO Remove this dependency

    [Range(0, 1)]
    public float dropProbability = 0.7f;

    void Start()
    {
        m_Health = GetComponent<Health>();
        DebugUtility.HandleErrorIfNullGetComponent<Health, Destructible>(m_Health, this, gameObject);

        // Subscribe to pickup action
        m_Health.onDie += OnDie;
    }

    void OnDie(GameObject killerSource)
    {
        if (Random.Range(0f, 1f) <= dropProbability)
        {
            GameObject instance = Instantiate(pickups[Random.Range(0, pickups.Length)]);
            instance.transform.position = transform.position;
        }
    }
}
