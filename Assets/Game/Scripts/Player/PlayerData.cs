using UnityEngine;

public enum PlayerStateEnum
{
    ALIVE = 0,
    DEAD = 1
};

[CreateAssetMenu(menuName = "Scriptable Object/Player Data", fileName = "new Player")]
public class PlayerData : GameEvent, ISerializationCallbackReceiver
{

    [Header("State")]
    public PlayerStateEnum InitialPlayerState = PlayerStateEnum.ALIVE;

    public PlayerStateEnum RuntimePlayerState;

    public PlayerStateEnum PlayerState
    {
        get
        {
            return RuntimePlayerState;
        }
        set
        {
            if (RuntimePlayerState != value)
            {
                RuntimePlayerState = value;
                Raise();
            }
        }
    }

    [Header("Movement")]
    [Tooltip("Max movement speed")]
    public float maxSpeed = 5f;
    [Tooltip("Movement sharpness")]
    public float movementSharpness = 15f;

    [Header("Info")]
    public string playerName = "Player";
    public string lastKilledBy;

    Vector3 m_CharacterVelocity;

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        RuntimePlayerState = InitialPlayerState;
    }
}
