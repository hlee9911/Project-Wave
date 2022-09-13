using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class MaterialScript : MonoBehaviour
{

    GameObject image;
    GameObject text;
    public Sprite c1;
    public Sprite c2;
    public Sprite c3;
    public Sprite c4;

    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetChild(0).gameObject;
        image = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeTo(int type, int amt){
        gameObject.SetActive(true);
        text = transform.GetChild(0).gameObject;
        image = transform.GetChild(1).gameObject;
        //change image and amounts
        switch(type){
            case 0:
                image.GetComponent<Image>().sprite = c1;
                break;
            case 1:
                image.GetComponent<Image>().sprite = c2;
                break;
            case 2:
                image.GetComponent<Image>().sprite = c3;
                break;
            case 3:
                image.GetComponent<Image>().sprite = c4;
                break;

        }
        text.GetComponent<TextMeshProUGUI>().text = amt.ToString();

    }

}
