using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyDisplayScript : MonoBehaviour
{
    private GameManagementSO refer;

    private MaterialScript[] materialRefs;


    // Start is called before the first frame update
    void Start()
    {
        refer = GameObject.Find("PlayerDataManager").GetComponent<PlayerGameManager>().GameManagementSO;
        materialRefs = new MaterialScript[4];
        int i = 0;
        foreach (Transform eachChild in transform){
            if (eachChild.tag == "UIMaterial")
            {
                materialRefs[i] = eachChild.gameObject.GetComponent<MaterialScript>();
                i++;
            }
        }
        refer.Currency1 = 50;
    }

    // Update is called once per frame
    void Update()
    {
        int index = 0;
        if(true){
            materialRefs[index].changeTo(0,refer.Currency1);
            index++;
        }
        if(refer.Currency2!=0){
            materialRefs[index].changeTo(1,refer.Currency2);
            index++;
        }
        if(refer.Currency3!=0){
            materialRefs[index].changeTo(2,refer.Currency3);
            index++;
        }
        if(refer.Currency4!=0){
            materialRefs[index].changeTo(3,refer.Currency4);
            index++;
        }
        for(int i = index;i<4;i++){
            materialRefs[i].gameObject.SetActive(false);
        }
    }
}
