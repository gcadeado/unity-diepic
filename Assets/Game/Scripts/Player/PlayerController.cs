using UnityEngine;

[RequireComponent(typeof(PlayerWeaponsManager))]
public abstract class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    protected Transform gunSlot;
    protected PlayerWeaponsManager m_weaponController;
    protected Health m_Health;

    protected Vector3 m_CharacterVelocity;

    [Header("Movement")]
    [Tooltip("Max movement speed")]
    public float maxSpeed = 5f;
    [Tooltip("Movement sharpness")]
    public float movementSharpness = 15f;
    protected virtual void Awake()
    {
        m_Health = GetComponent<Health>();
        DebugUtility.HandleErrorIfNullGetComponent<Health, PlayerController>(m_Health, this, gameObject);

        m_weaponController = GetComponent<PlayerWeaponsManager>();
        DebugUtility.HandleErrorIfNullGetComponent<PlayerWeaponsManager, PlayerController>(m_Health, this, gameObject);
    }

    protected virtual void Start() { }

    protected virtual void OnEnable()
    {
        m_Health.onDie += OnDie;
    }

    protected abstract void Update();

    protected virtual void OnDie(GameObject source)
    {
        Destroy(gameObject);
    }

    protected virtual void Move(float angle, Vector3 moveInput, float playerSpeed, float playerMovementSharpness)
    {
        // Plane XY gun rotation
        gunSlot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Character movement handling
        Vector3 targetVelocity = moveInput * playerSpeed;

        // interpolate between current velocity and the target velocity based on acceleration speed
        m_CharacterVelocity =
            Vector3
                .Lerp(m_CharacterVelocity,
                targetVelocity,
                playerMovementSharpness * Time.deltaTime);
        transform.Translate(m_CharacterVelocity * Time.deltaTime, Space.World);
    }

    protected virtual void Look(float angle)
    {
        gunSlot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public virtual void LookTo(Vector3 position)
    {
        Vector3 moveInput = Vector3.ClampMagnitude(position - transform.position, 1);
        Look(Vector3.SignedAngle(Vector3.up, moveInput, Vector3.forward));
    }

    public virtual void MoveTo(Vector3 position, float playerSpeed, float playerMovementSharpness)
    {
        Vector3 moveInput = Vector3.ClampMagnitude(position - transform.position, 1);
        Move(Vector3.SignedAngle(Vector3.up, moveInput, Vector3.forward), moveInput, playerSpeed, playerMovementSharpness);
    }

    public virtual void OnFire(bool isFiring)
    {
        m_weaponController.isFiring = isFiring;
    }

    protected virtual void OnDisable()
    {
        m_Health.onDie -= OnDie;
    }
}
