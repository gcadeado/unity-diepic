using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]

    [SerializeField]
    PlayerData playerData;
    [SerializeField]
    IntVariable scoreObject = null; // TODO rethink score architecture
    PlayerInputHandler m_InputHandler;
    Vector3 m_CharacterVelocity;
    Health m_Health;
    [SerializeField]
    Transform gunSlot;

    void Awake()
    {
        m_InputHandler = GetComponent<PlayerInputHandler>();
        DebugUtility
            .HandleErrorIfNullGetComponent
            <PlayerInputHandler, PlayerController>(m_InputHandler,
            this,
            gameObject);

        m_Health = GetComponent<Health>();
        DebugUtility.HandleErrorIfNullGetComponent<Health, PlayerController>(m_Health, this, gameObject);
    }

    void Start()
    {
        playerData.PlayerState = PlayerStateEnum.ALIVE;
    }

    void OnEnable()
    {
        m_Health.onDie += OnDie;
    }

    void Update()
    {
        HandleCharacterMovement(m_InputHandler.GetMouseLookAngle() - 90f, m_InputHandler.GetMoveInput());
    }

    void OnDie(GameObject source)
    {
        playerData.lastKilledBy = source.name;
        playerData.PlayerState = PlayerStateEnum.DEAD;
        Destroy(gameObject);
    }

    public void AddScore(int value)
    {
        if (value > 0)
            scoreObject.Value += value;
    }

    void HandleCharacterMovement(float angle, Vector3 moveInput)
    {
        // Plane XY gun rotation
        gunSlot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Character movement handling
        Vector3 targetVelocity = moveInput * playerData.maxSpeed;

        // interpolate between current velocity and the target velocity based on acceleration speed
        m_CharacterVelocity =
            Vector3
                .Lerp(m_CharacterVelocity,
                targetVelocity,
                playerData.movementSharpness * Time.deltaTime);
        transform.Translate(m_CharacterVelocity * Time.deltaTime, Space.World);
    }

    void OnDisable()
    {
        m_Health.onDie -= OnDie;
    }
}
