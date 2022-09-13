using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradePromptScript : MonoBehaviour
{
    GameObject nameRef;
    GameObject levelRef;
    GameObject buttonRef;
    // Start is called before the first frame update
    void Awake()
    {
        
        nameRef = transform.GetChild(0).gameObject;
        levelRef = transform.GetChild(1).gameObject;
        buttonRef = transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeTo(string name, int level, bool state){
        nameRef.GetComponent<TextMeshProUGUI>().text = name;
        if(state){
            levelRef.GetComponent<TextMeshProUGUI>().text = "(Broken)";
        }
        else{
            levelRef.GetComponent<TextMeshProUGUI>().text = "(Lv. "+level+")";
        }
    }
}
