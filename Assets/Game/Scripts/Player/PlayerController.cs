using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerData playerData;
    [SerializeField]
    private IntVariable scoreObject = null; // TODO re-check score architecture
    PlayerInputHandler m_InputHandler;
    Vector3 m_CharacterVelocity;
    Health m_Health;
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
        m_Health.onDie += OnDie;
        playerData.PlayerState = PlayerStateEnum.ALIVE;
    }

    void Update()
    {
        HandleCharacterMovement();
    }

    void OnDie(GameObject source)
    {
        playerData.lastKilledBy = source.name;
        playerData.PlayerState = PlayerStateEnum.DEAD;
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Handle collision damages
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        // Handle repel forces
    }

    public void AddScore(int value)
    {
        if (value > 0)
            scoreObject.Value += value;
    }

    void HandleCharacterMovement()
    {
        // Plane XY character rotation
        {
            transform.rotation =
                Quaternion
                    .AngleAxis(m_InputHandler.GetMouseLookAngle() - 90f,
                    Vector3.forward);
        }

        // Character movement handling
        {
            // Converts move input to a worldspace vector based on our character's transform orientation
            Vector3 worldspaceMoveInput =
                transform.TransformVector(m_InputHandler.GetMoveInput());
            Vector3 targetVelocity =
                m_InputHandler.GetMoveInput() * playerData.maxSpeed;

            // smoothly interpolate between our current velocity and the target velocity based on acceleration speed
            m_CharacterVelocity =
                Vector3
                    .Lerp(m_CharacterVelocity,
                    targetVelocity,
                    playerData.movementSharpness * Time.deltaTime);
            transform
                .Translate(m_CharacterVelocity * Time.deltaTime, Space.World);
        }
    }
}
