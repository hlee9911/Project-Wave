using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public string sceneLoad;

    public void playGame()
    {
        Time.timeScale = 1.0f;
        // playerGameManager.ChangeGameStateToPaused();
        // PlayerData.isGameStarted = false;
        // isGamePaused = false;
        Invoke("MoveToGame", 0.5f);
    }

    void MoveToGame()
    {
        SceneManager.LoadScene(sceneLoad);
    }

}
