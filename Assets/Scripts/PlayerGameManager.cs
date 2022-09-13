using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Manages player's game data
public class PlayerGameManager : MonoBehaviour
{

    [SerializeField] private GameManagementSO gameManagementSO;
    [SerializeField] private int playeCurrentScore;
    [SerializeField] private float playerSurvivedtimer;
    [SerializeField] private PlayerCastle playerCastle;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private TMP_Text victoryScoreTimeText;
    [SerializeField] private TMP_Text deathScoreTimeText;
    [SerializeField] private EnemiesSpawner enemiesSpawner;

    public GameManagementSO GameManagementSO { get { return gameManagementSO; } }
    public int PlayeCurrentScore { get { return playeCurrentScore; } set { playeCurrentScore = value; } }
    public float PlayerSurvivedtimer { get { return playerSurvivedtimer; } set { playerSurvivedtimer = value; } }
    public GameState PlayerGameState { get { return gameManagementSO.gameState; } }

    void Awake()
    {
        Time.timeScale = 1.0f;
        playerCastle = FindObjectOfType<PlayerCastle>();
        enemiesSpawner = FindObjectOfType<EnemiesSpawner>();
        GameManagementSO.gameState = GameState.Gameplay;
        playeCurrentScore = 0;
        playerSurvivedtimer = 0f;
    }

    void Update()
    {
        if (playerCastle && gameManagementSO.gameState == GameState.Gameplay && !enemiesSpawner.isVictory)
        {
            playerSurvivedtimer += Time.deltaTime;
        }
        if (scoreText)
        {
            scoreText.text = "Current Score: " + playeCurrentScore.ToString("N0");
        }
        if (enemiesSpawner.isVictory)
        {
            ProcessVictorySequence();
        }
        if (playerCastle == null)
        {
            Invoke("ProcessDeathSequence", 1f);
        }
    }

    void ProcessDeathSequence()
    {
        ChangeGameStateToPaused();
        Time.timeScale = 0.0f;
        if (deathPanel)
        {
            deathPanel.SetActive(true);
            deathScoreTimeText.text = $"Survived Time: {ChangeTimerToString()} \n Score: {playeCurrentScore.ToString("N0")}";
        }
    }

    public void ProcessVictorySequence()
    {
        ChangeGameStateToPaused();
        Time.timeScale = 0.0f;
        if (victoryPanel)
        {
            victoryPanel.SetActive(true);
            victoryScoreTimeText.text = $"Survived Time: {ChangeTimerToString()} \n Score: {playeCurrentScore.ToString("N0")}";
        }
    }

    string ChangeTimerToString()
    {
        int seconds = (int)(playerSurvivedtimer % 60);
        int mintues = (int)(playerSurvivedtimer / 60) % 60;
        int hours = (int)(playerSurvivedtimer / 3600) % 24;

        string timerString = string.Format("{0:0}:{1:00}:{2:00}", hours, mintues, seconds);
        return timerString;
    }

    // The following three method will be used to change the state of the game inside the GameManagementSO
    public void ChangeGameStateToPlaceHolder()
    {
        gameManagementSO.gameState = GameState.Placeholder;
    }

    public void ChangeGameStateToGamePlay()
    {
        gameManagementSO.gameState = GameState.Gameplay;
    }

    public void ChangeGameStateToPaused()
    {
        gameManagementSO.gameState = GameState.Paused;
    }

}
