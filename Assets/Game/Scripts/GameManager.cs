using Cinemachine;
using UnityEngine;

public class GameManager : GameEventListener
{
    [Header("References")]
    [SerializeField]
    private GameData gameData;

    [SerializeField]
    private CinemachineVirtualCamera vmCam;

    [SerializeField]
    private GameObject playerPrefab;

    void Update()
    {
        if (gameData.GameState == GameStateEnum.STATE_GAMEOVER)
        {
            if (Input.GetKeyUp(KeyCode.Return))
            {
                ResetGame();
            }
        }
    }

    public void StartGame()
    {
        gameData.GameState = GameStateEnum.STATE_PLAYING;
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab);
        vmCam.m_Follow = player.transform;
    }

    public void ResetGame()
    {
        gameData.GameState = GameStateEnum.STATE_WELCOME;
    }

    void UpdateGameState()
    {
        PlayerStateEnum playerState = ((PlayerData)Event).PlayerState;
        if (playerState == PlayerStateEnum.DEAD)
            gameData.GameState = GameStateEnum.STATE_GAMEOVER;
    }

    public override void OnEventRaised()
    {
        base.OnEventRaised();

        UpdateGameState();
    }
}