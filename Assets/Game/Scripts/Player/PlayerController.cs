using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (PlayerInputHandler))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [Header("General")]
    [Header("Movement")]
    [Tooltip("Max movement speed")]
    public float maxSpeed = 10f;

    [
        Tooltip(
            "Sharpness for the movement, a low value will make the player accelerate and decelerate slowly, a high value will do the opposite")
    ]
    public float movementSharpness = 15f;

    [Tooltip("Speed modifier")]
    public float speedModifier = 1f;

    public Vector3 characterVelocity { get; set; }

    public bool isDead { get; set; }

    PlayerInputHandler m_InputHandler;

    Vector3 m_CharacterVelocity;

    void Start()
    {
        m_InputHandler = GetComponent<PlayerInputHandler>();
        DebugUtility
            .HandleErrorIfNullGetComponent
            <PlayerInputHandler, PlayerController>(m_InputHandler,
            this,
            gameObject);
    }

    void Update()
    {
        HandleCharacterMovement();
    }

    void OnDie()
    {
        isDead = true;
    }

    void HandleCharacterMovement()
    {
        // Plane XY character rotation
        {
            // rotate the transform with the input around its Z axis
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
                m_InputHandler.GetMoveInput() * maxSpeed * speedModifier;

            // smoothly interpolate between our current velocity and the target velocity based on acceleration speed
            characterVelocity =
                Vector3
                    .Lerp(characterVelocity,
                    targetVelocity,
                    movementSharpness * Time.deltaTime);
            transform
                .Translate(characterVelocity * Time.deltaTime, Space.World);
        }
    }
}
