using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private bool m_canProcessInput = true;
    public bool CanProcessInput
    {
        get { return m_canProcessInput; }
        set { m_canProcessInput = value; }
    }

    bool m_FireInputWasHeld;

    private void LateUpdate()
    {
        m_FireInputWasHeld = GetFireInputHeld();
    }

    public Vector3 GetMoveInput()
    {
        if (!CanProcessInput)
            return Vector3.zero;

        Vector3 move =
            new Vector3(Input.GetAxisRaw(InputConstants.k_AxisNameHorizontal),
                Input.GetAxisRaw(InputConstants.k_AxisNameVertical),
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
        return Input.GetButton(InputConstants.k_ButtonNameFire);
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
