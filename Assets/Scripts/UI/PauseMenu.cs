using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public string sceneLoad;

    public static bool isGamePaused = false;

    public GameObject pauseMenuPanel;

    public GameObject pausemenuOptions;

    [SerializeField] private PlayerGameManager playerGameManager;
    // public GameObject pausemenuSettings;

    void Awake()
    {
        playerGameManager = FindObjectOfType<PlayerGameManager>();
    }

    public void QuitGame()
    {
        isGamePaused = false;
        Debug.Log("Exiting Game");
        Application.Quit();
    }

    public void MainMenu()
    {
        Time.timeScale = 1.0f;
        playerGameManager.ChangeGameStateToPaused();
        // PlayerData.isGameStarted = false;
        // isGamePaused = false;
        SceneManager.LoadScene(sceneLoad);
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        pauseMenuPanel.SetActive(false);
        playerGameManager.ChangeGameStateToGamePlay();
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        pauseMenuPanel.SetActive(false);
        playerGameManager.ChangeGameStateToGamePlay();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        pauseMenuPanel.SetActive(true);
        pausemenuOptions.SetActive(true);
        // pausemenuSettings.SetActive(false);
        playerGameManager.ChangeGameStateToPaused();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (playerGameManager)
            {
                // if game is paused
                if (playerGameManager.PlayerGameState == GameState.Paused)
                {
                    Resume();
                }
                // if game is not paused
                else
                {
                    Pause();
                }
            }
        }
    }
}
