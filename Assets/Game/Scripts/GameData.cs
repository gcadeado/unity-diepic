using UnityEngine;

public enum GameStateEnum
{
    STATE_WELCOME = 0,
    STATE_PLAYING = 1,
    STATE_GAMEOVER = 2
};

[CreateAssetMenu(menuName = "Scriptable Object/Game Data", fileName = "new Game Manager")]
public class GameData : GameEvent, ISerializationCallbackReceiver
{
    public GameStateEnum InitialGameState = GameStateEnum.STATE_WELCOME;
    public GameStateEnum RuntimeGameState;

    public GameStateEnum GameState
    {
        get
        {
            return RuntimeGameState;
        }
        set
        {
            if (RuntimeGameState != value)
            {
                RuntimeGameState = value;
                Raise();
            }
        }
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        RuntimeGameState = InitialGameState;
    }
}
