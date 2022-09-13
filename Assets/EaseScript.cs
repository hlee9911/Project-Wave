using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EaseScript : MonoBehaviour
{
    public Vector3 start;
    public Vector3 finish;
    public float spd;
    public float bobspd;
    private int timer;
    public float magnitude;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = start;
        timer = 0;
        StartCoroutine(run());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
        

        
    IEnumerator run()
    {
        for (float alpha = 1f; true; alpha -= 0.1f)
            {
            transform.position += spd*(finish-transform.position);
            timer++;
            if(timer>500){
                transform.position = finish+Vector3.up*(Mathf.Sin((timer-500)/bobspd))*magnitude;
            }
            yield return new WaitForSeconds(.016f);
        }

    }
}
