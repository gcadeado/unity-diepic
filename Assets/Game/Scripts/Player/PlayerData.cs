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

    [ReadOnly]
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

    [Header("Info")]
    public string playerName = "Player";
    public string lastKilledBy;

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        RuntimePlayerState = InitialPlayerState;
    }
}
