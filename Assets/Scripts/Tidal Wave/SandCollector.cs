using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandCollector : MonoBehaviour
{
    public GameObject[] resourceSprites;
    public PlayerGameManager managementSO;
    private TurretAudioManager soundManager;

    void Start()
    {
        managementSO = FindObjectOfType<PlayerGameManager>();
        managementSO.GameManagementSO.Currency1 = 0;
        soundManager = FindObjectOfType<TurretAudioManager>();
    }
    int i = 0;//time counter
    int c = 0;//click counter
    Rect bounds = new Rect(200, 0, 575, Screen.height);
    void Update()
    {
        if (Input.GetMouseButton(0) && bounds.Contains(Input.mousePosition))
        {
            if (c > 100) {++managementSO.GameManagementSO.Currency1; c = 0; }
            soundManager.PlayTurretSound("Sand Collect");
            c++;//click gains sand faster
        }

        if (i > 200) { ++managementSO.GameManagementSO.Currency1; i = 0; }
        i++;//i counter gathers over time at a slower rate

    }

}
