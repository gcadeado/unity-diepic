using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerLocalController : PlayerController
{
    [Header("References")]

    [SerializeField]
    PlayerData playerData;
    [SerializeField]
    IntVariable scoreObject = null;
    PlayerInputHandler m_InputHandler;

    protected override void Awake()
    {
        base.Awake();
        m_InputHandler = GetComponent<PlayerInputHandler>();
        DebugUtility
            .HandleErrorIfNullGetComponent
            <PlayerInputHandler, PlayerController>(m_InputHandler,
            this,
            base.gameObject);
    }

    protected override void Start()
    {
        base.Start();
        playerData.PlayerState = PlayerStateEnum.ALIVE;
    }

    protected override void Update()
    {
        base.Move(m_InputHandler.GetMouseLookAngle() - 90f,
            m_InputHandler.GetMoveInput(),
            playerData.maxSpeed, playerData.movementSharpness);

        m_weaponController.isFiring = m_InputHandler.GetFireInputDown() || m_InputHandler.GetFireInputHeld();

        m_weaponController.switchWeaponInput = m_InputHandler.GetSelectWeaponInput();
    }

    protected override void OnDie(GameObject source)
    {
        if (source)
            playerData.lastKilledBy = source.name;
        playerData.PlayerState = PlayerStateEnum.DEAD;
        Destroy(gameObject);
    }

    public void AddScore(int value)
    {
        if (value > 0)
            scoreObject.Value += value;
    }
}
