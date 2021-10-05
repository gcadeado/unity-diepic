using UnityEngine;

public class UIManager : GameEventListener
{

    [SerializeField]
    private GameObject welcomePanel;

    [SerializeField]
    private GameObject gamePanel;

    [SerializeField]
    private GameObject gameOverPanel;

    void Start()
    {
        UpdateUIState();
    }

    void UpdateUIState()
    {
        GameStateEnum state = ((GameData)Event).GameState;
        welcomePanel.SetActive(state == GameStateEnum.STATE_WELCOME);
        gamePanel.SetActive(state == GameStateEnum.STATE_PLAYING);
        gameOverPanel.SetActive(state == GameStateEnum.STATE_GAMEOVER);
    }

    //When the attached IntVariable is modified, call this function
    public override void OnEventRaised()
    {
        base.OnEventRaised();

        UpdateUIState();
    }

}
