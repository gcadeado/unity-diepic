using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [Tooltip("Sensitivity multiplier for the mouse")]
    public float lookSensitivity = 1f;

    [Tooltip("Additional sensitivity multiplier for WebGL")]
    public float webglLookSensitivityMultiplier = 0.25f;

    [Tooltip("Limit to consider an input when using a trigger on a controller")]
    public float triggerAxisThreshold = 0.4f;

    [Tooltip("Should hide mouse")]
    public bool mouseVisibility = false;

    PlayerController m_PlayerController;

    private bool m_canProcessInput = true;
    public bool CanProcessInput
    {
        get { return m_canProcessInput; }
        set { m_canProcessInput = value; }
    }

    bool m_FireInputWasHeld;

    private void Start()
    {
        m_PlayerController = GetComponent<PlayerController>();
        DebugUtility
            .HandleErrorIfNullGetComponent
            <PlayerController, PlayerInputHandler>(m_PlayerController,
            this,
            gameObject);
    }

    private void LateUpdate()
    {
        m_FireInputWasHeld = GetFireInputHeld();
    }

    public Vector3 GetMoveInput()
    {
        if (!CanProcessInput)
            return Vector3.zero;

        Vector3 move =
            new Vector3(Input.GetAxisRaw(GameConstants.k_AxisNameHorizontal),
                Input.GetAxisRaw(GameConstants.k_AxisNameVertical),
                0f);

        move = Vector3.ClampMagnitude(move, 1);

        return move;
    }

    public bool GetFireInputDown()
    {
        return GetFireInputHeld() && !m_FireInputWasHeld;
    }

    public bool GetFireInputReleased()
    {
        return !GetFireInputHeld() && m_FireInputWasHeld;
    }

    public bool GetFireInputHeld()
    {
        if (!CanProcessInput)
            return false;

        bool isGamepad =
            Input.GetAxis(GameConstants.k_ButtonNameGamepadFire) != 0f;
        if (isGamepad)
        {
            return Input.GetAxis(GameConstants.k_ButtonNameGamepadFire) >=
            triggerAxisThreshold;
        }
        else
        {
            return Input.GetButton(GameConstants.k_ButtonNameFire);
        }
    }

    public int GetSwitchWeaponInput()
    {
        if (!CanProcessInput)
            return 0;

        bool isGamepad = Input.GetAxis(GameConstants.k_ButtonNameGamepadSwitchWeapon) != 0f;
        string axisName = isGamepad ? GameConstants.k_ButtonNameGamepadSwitchWeapon : GameConstants.k_ButtonNameSwitchWeapon;

        if (Input.GetAxis(axisName) > 0f)
            return -1;
        else if (Input.GetAxis(axisName) < 0f)
            return 1;
        else if (Input.GetAxis(GameConstants.k_ButtonNameNextWeapon) > 0f)
            return -1;
        else // Input.GetAxis(GameConstants.k_ButtonNameNextWeapon) < 0f
            return 1;
    }

    public int GetSelectWeaponInput()
    {

        if (!CanProcessInput)
            return 0;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            return 1;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            return 2;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            return 3;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            return 4;
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            return 5;
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            return 6;
        else
            return 0;

    }

    public float GetMouseLookAngle()
    {
        if (!CanProcessInput)
            return 0f;

        Vector2 look =
            Input.mousePosition -
            Camera.main.WorldToScreenPoint(transform.position);

        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        return angle;
    }
}
