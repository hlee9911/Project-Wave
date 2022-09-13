using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipScript : MonoBehaviour
{
    GameObject[] materials;
    public static TooltipScript instance;
    GameObject text;
    // Start is called before the first frame update
   void Awake(){
        if(instance==null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
        materials = new GameObject[3];
        int i = 0;
        foreach (Transform eachChild in transform)
        {
            if (eachChild.gameObject.tag == "UIMaterial")
            {
                materials[i] = eachChild.gameObject;
                materials[i].SetActive(false);
                i++;
            }
            if(eachChild.gameObject.tag=="UITooltipText"){
                text = eachChild.gameObject;
            }
        }
        


    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onActivate(string descr, List<KeyValuePair<int,int>> cost){
        for(int i = 0; i < 3; i++){
            if(i>=cost.Count){
                materials[i].SetActive(false);
            }
            else{
                materials[i].SetActive(true);
                materials[i].GetComponent<MaterialScript>().changeTo(cost[i].Key,cost[i].Value);
            }
        }
        text.GetComponent<TextMeshProUGUI>().text = descr;
        

    }


}
