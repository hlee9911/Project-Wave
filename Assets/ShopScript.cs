using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ShopScript : MonoBehaviour
{

    public GameObject PlaceableBuildingRef;
    public GameObject PlaceableBuildingInstance;
    private int heldid;

    public GameObject Building0;
    public GameObject Building1;
    public GameObject Building2;
    public GameObject Building3;
    public GameObject Building4;

    private AudioSource sfx;
    public AudioClip place;
    public AudioClip select;
    public AudioClip deselect;

    private GameManagementSO refer;



    private bool holdingObject;
    private GameObject[] items;

    private List<KeyValuePair<int,int>>[] costs;
    private string[] descrs;


    // Start is called before the first frame update
    void Start()
    {
        holdingObject = false;
        items = new GameObject[5];
        refer = GameObject.Find("PlayerDataManager").GetComponent<PlayerGameManager>().GameManagementSO;

        descrs = new string[5] {"A basic sand shooter. Cheap and versatile, but lacking in power.","A sand catapult, great for attacking groups of enemies!","A water tower that can slow enemies down.","A stick slingshot that can hit many enemies with one shot!","An oil turret that can launch a barrel of oil wherever you want! First click on the building, then click where ever you want it to shoot. While it's reloading, feel free to upgrade it!"};
        costs = new List<KeyValuePair<int,int>>[5]{
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,10)},
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,25),new KeyValuePair<int, int>(1,2)},
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,15),new KeyValuePair<int, int>(2,1)},
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,40),new KeyValuePair<int, int>(2,2)},
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,70),new KeyValuePair<int, int>(3,2)}
        };
        sfx = GetComponent<AudioSource>();

        int i = 0;
        foreach (Transform eachChild in transform)
        {
            if (eachChild.tag == "UIShopBuilding")
            {
                items[i] = eachChild.gameObject;
                i++;
            }
        }
    }

    bool AttemptPlace(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        List<GameObject> rootObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects( rootObjects );

        for (int i = 0; i < rootObjects.Count; ++i)
        {
            GameObject gm = rootObjects[ i ];
            if(gm.tag=="Basic Turret"||gm.tag=="Heavy Turret"||gm.tag=="Water Turret"||gm.tag=="Piercing Turret"||gm.tag=="Oil Turret"){
                mousePos.z = gm.transform.position.z;
                if(Vector3.Distance(gm.transform.position,mousePos)<1){
                    return false;
                }
            }
        }

        if(mousePos.x<-5){
            return false;
        }
        if(mousePos.x>7){
            return false;
        }

        return true;
    }


    bool CheckCosts(int id){
        bool ans = true;
        foreach(KeyValuePair<int,int> x in costs[id]){
            switch(x.Key){
                case 0:
                    ans=ans&&(refer.Currency1>=x.Value);
                    break;
                case 1:
                    ans=ans&&(refer.Currency2>=x.Value);
                    break;
                case 2:
                    ans=ans&&(refer.Currency3>=x.Value);
                    break;
                case 3:
                    ans=ans&&(refer.Currency4>=x.Value);
                    break;
            }
        }

        return ans;
    }

    void PayCosts(){
        foreach(KeyValuePair<int,int> x in costs[heldid]){
            switch(x.Key){
                case 0:
                    refer.Currency1-=x.Value;
                    break;
                case 1:
                    refer.Currency2-=x.Value;
                    break;
                case 2:
                    refer.Currency3-=x.Value;
                    break;
                case 3:
                    refer.Currency4-=x.Value;
                    break;
            }
        }

    }


    // Update is called once per frame
    void Update()
    {

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit;
        // Casts the ray and get the first game object hit
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);


        
        if(holdingObject){
            if(Input.GetKeyDown(KeyCode.Escape)||Input.GetMouseButtonDown(1)){
                Destroy(PlaceableBuildingInstance);
                holdingObject= false;
                sfx.clip = deselect;
                sfx.Play();
            }
            if(Input.GetMouseButtonDown(0)){
                if(AttemptPlace()){
                    Destroy(PlaceableBuildingInstance);
                    holdingObject= false;
                    //place building at cursor
                    PayCosts();
                    GameObject BuildingRef = Building1;
                    if(heldid == 0){
                        BuildingRef = Building0;
                    }
                    else if(heldid == 1){
                        BuildingRef = Building1;
                    }
                    else if(heldid == 2){
                        BuildingRef = Building2;
                    }
                    else if(heldid == 3){
                        BuildingRef = Building3;
                    }
                    else if(heldid == 4){
                        BuildingRef = Building4;
                    }
                    Vector3 why = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    GameObject x = Instantiate(BuildingRef, new Vector3(why.x,why.y,1), Quaternion.identity);

                    sfx.clip = place;
                    sfx.Play();

                    Debug.Log("Mouse pos"+Camera.main.ScreenToWorldPoint(Input.mousePosition));

                }
                else{
                    //possibly play sound effect
                }
            }
        }
        else{

        
            if (Input.GetMouseButtonDown(0))
            {
                
                if(hit.transform!=null){
                    Debug.Log("testt");
                }

                if (hit.transform != null&& hit.transform.gameObject.tag=="UIShopBuilding")
                {
                    GameObject slot = hit.transform.gameObject;
                    heldid = slot.GetComponent<ShopSlotScript>().id;
                    if(CheckCosts(heldid)){
                        PlaceableBuildingInstance = Instantiate(PlaceableBuildingRef, new Vector3(0, 0, 1), Quaternion.identity);
                        PlaceableBuildingInstance.GetComponent<Image>().sprite = slot.GetComponent<Image>().sprite;
                        holdingObject = true;
                        sfx.clip = select;
                        sfx.Play();
                    }
                }
            }
            if(hit.transform != null)
            {
                GameObject slot = hit.transform.gameObject;
                if(slot.tag == "UIShopBuilding"){
                    int id = slot.GetComponent<ShopSlotScript>().id;
                    TooltipScript.instance.gameObject.SetActive(true);
                    TooltipScript.instance.onActivate(descrs[id],costs[id]);
                }
                else if(slot.tag =="UIUpgradeButton"){

                }
                else{
                    TooltipScript.instance.gameObject.SetActive(false);
                }


            }
            else{
                TooltipScript.instance.gameObject.SetActive(false);
            }
        }
    }
}
