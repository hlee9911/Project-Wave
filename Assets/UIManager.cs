using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class UIManager : MonoBehaviour
{
    public GameObject blackScreen;

    public string sceneToLoad;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void quitGame(){
        Application.Quit();
    }

    public void startGame()
    {
        Debug.Log("test");
        StartCoroutine(loadGame());
    }

    public void playGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator loadGame()
    {
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            yield return new WaitForSeconds(.1f);
        }
        StartCoroutine(loadGame2());
        /*GameObject instance = Instantiate(blackScreen);
        instance.GetComponent<BlackScreenScript>().start = 0;
        instance.GetComponent<BlackScreenScript>().finish = 1;*/

    }
    IEnumerator loadGame2()
    {
        for (float alpha = 1f; alpha >= 0; alpha -= 0.01f)
        {
            yield return new WaitForSeconds(.01f);
        }
        SceneManager.LoadScene("Assets/Scenes/TempGameScene.unity", LoadSceneMode.Single);
    }

}
