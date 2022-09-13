using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TideWaveManager : MonoBehaviour
{
    private float frequency = 40f;
    private float sandDryTime = 5f;
    private float tideIn = 2f;
    private float tideOut = 4f;
    private Vector3 tideReach = new Vector3(.3f, .05f, 1f);
    private Vector2 wetSandLoc = new Vector3(.65f, .05f, .5f);
    private bool waveStarted = false;
    private float spawnXBoundLeft = -.5f;
    private float spawnXBoundRight = 4f;
    private float spawnYBound = 4f;
    private int resourceBoundMax = 6;
    private int resourceBoundMin = 2;
    private GameState gameState;
    private Rigidbody2D rb;
    private TurretAudioManager turretAudioManager;

    public GameObject wave;
    public GameObject wetBeach;
    public GameObject[] resources;

    // Start is called before the first frame update
    void Start()
    {
        turretAudioManager = FindObjectOfType<TurretAudioManager>();
        rb = wave.GetComponent<Rigidbody2D>();
        //rb.velocity = (tideReach - wave.transform.position) * .05f;
        StartCoroutine(Wave());
    }
    private void Update()
    {
        switch(gameState)
        {
            case GameState.Gameplay:
                if (!waveStarted)
                {
                    waveStarted = true;
                    BeginWave();
                }         
                break;
            case GameState.Paused:
                waveStarted = false;
                break;
        }
    }
 
    private void BeginWave()
    {
        waveStarted = true;
        StartCoroutine(Wave());
    }
    public static float Integrate(float x_low, float x_high, int N_steps)
    {
        float h = (x_high - x_low) / N_steps;
        float res = (x_low + x_high) / 2;
        for (int i = 1; i < N_steps; i++)
        {
            res += x_low + i * h;
        }
        return h * res;
    }
    
    IEnumerator Wave()
    {
        yield return new WaitForSeconds(10f);
        // have a while statement to check if game is active while (FindObjectOfType<PlayerGameManager>().)
        // put at beginning of while loop yield return new WaitForSeconds(frequency);
        //rb.velocity = (tideReach - wave.transform.position) * 2f;

        
        while (true)
        {

            turretAudioManager.PlayWaveSound("Wave Approach");
            wave.SetActive(true);
            float elapsedTime = 0;
            Vector2 ogWavePos = wave.transform.position;

            /*
            while (elapsedTime < tideIn)
            {
                wave.transform.position = Vector2.Lerp(ogWavePos, tideReach, elapsedTime / tideIn);
                float eq = -(Mathf.Pow(Time.deltaTime, 2)) + Time.deltaTime + 6;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            */

            while (elapsedTime < tideIn)
            {
                //float eq = -(Mathf.Pow(Time.deltaTime, 2)) + Time.deltaTime + 6;
                float eq = 1 / (1f + Time.deltaTime);
                if (eq >= .95f)
                {
                    rb.velocity = (tideReach - wave.transform.position) * eq;
                }
                
                elapsedTime += Time.deltaTime;
                yield return new WaitForSeconds(.001f);
            }


            Vector2 newWavePos = wave.transform.position;
            int resourceSpawnAmt = Random.Range(resourceBoundMin, resourceBoundMax + 1);
            for (int i = 0; i < resourceSpawnAmt; ++i)
            {
                float x = Random.Range(spawnXBoundLeft, spawnXBoundRight + 1);
                float y = Random.Range(-spawnYBound, spawnYBound + 1);
                float zRot = Random.Range(0, 360);
                Quaternion rot = Quaternion.Euler(0, 0, zRot);
                Vector2 spawnLoc = new Vector2(x, y);
                int num = Random.Range(0, 10);
                if (num < 4)
                {
                    Instantiate(resources[1], spawnLoc, rot);
                }
                if (num > 3 && num < 9)
                {
                    Instantiate(resources[2], spawnLoc, rot);
                }
                if (num > 8)
                {
                    Instantiate(resources[0], spawnLoc, rot);
                }
            }
            Instantiate(wetBeach, wetSandLoc, Quaternion.identity, transform.parent);
            //yield return new WaitForSeconds(.5f);
            elapsedTime = 0;

            turretAudioManager.PlayWaveSound("Wave Recede");
            while (elapsedTime < tideOut)
            {
                wave.transform.position = Vector2.Lerp(newWavePos, ogWavePos, elapsedTime / tideIn);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            wave.SetActive(false);
            yield return new WaitForSeconds(frequency);
        }
       
    }
    
}
