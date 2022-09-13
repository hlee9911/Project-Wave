using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class BlackScreenScript : MonoBehaviour
{

    public float start;
    public float finish;
    private Image image;
    private int timer;

    // Start is called before the first frame update
    void Start()
    {
        
        timer = 0;
        image = GetComponent<Image>();
        image.color = new Color(image.color.r, image.color.g, image.color.b, start);
        StartCoroutine(run());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

       
    IEnumerator run()
    {
        for (float alpha = 1f; true; alpha -= 0.01f)
            {
            
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a+.01f*(finish-start));
            timer++;
            if(timer>110){
                gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(.01f);
        }

    }



}
