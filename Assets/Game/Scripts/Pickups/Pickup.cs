using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Pickup : MonoBehaviour
{
    public UnityAction<PlayerController> onPick;

    public UnityEvent OnPickupEvent = new UnityEvent();

    public Rigidbody2D pickupRigidbody { get; private set; }

    Collider2D m_Collider;

    private void Start()
    {
        pickupRigidbody = GetComponent<Rigidbody2D>();
        DebugUtility.HandleErrorIfNullGetComponent<Rigidbody2D, Pickup>(pickupRigidbody, this, gameObject);
        m_Collider = GetComponent<Collider2D>();
        DebugUtility.HandleErrorIfNullGetComponent<Collider2D, Pickup>(m_Collider, this, gameObject);

        // ensure the physics setup is a kinematic rigidbody trigger
        pickupRigidbody.isKinematic = true;
        m_Collider.isTrigger = true;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pickingPlayer = other.GetComponent<PlayerController>();

        if (pickingPlayer != null && onPick != null)
        {
            OnPickupEvent.Invoke();
            onPick.Invoke(pickingPlayer);
        }
    }
}
