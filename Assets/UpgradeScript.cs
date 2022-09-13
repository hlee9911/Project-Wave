using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeScript : MonoBehaviour
{

    public GameObject BuildingRef;
    private List<KeyValuePair<int,int>>[][] costs;
    private string[][] descrs;
    private GameManagementSO refer;
    private AudioSource sfx;

    private int buildingLevel;
    private string buildingName;
    private bool buildingState;
    private int id;
    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        BuildingRef = null;
        sfx = GetComponent<AudioSource>();
        refer = GameObject.Find("PlayerDataManager").GetComponent<PlayerGameManager>().GameManagementSO;
        descrs = new string[3][];
        descrs[0] = new string[5]  {"This tower is broken! It will attack slower until you repair it.","This tower is broken! It will attack slower until you repair it.","This tower is broken! It will attack slower until you repair it.","This tower is broken! It will attack slower until you repair it.","This tower is broken! It will attack slower until you repair it."};
        descrs[1] = new string[5]  {"Upgrade this sand shooter to increase its fire rate!","Upgrade this shell catapult to increase its power!","Upgrade this water sprayer to increase its range and attack speed!","Upgrade this stick slingshot to increase its pierce and power!","Upgrade this oil tower to increase its effectiveness!"};
        descrs[2] = new string[5]  {"Upgrade this sand shooter to further increase its fire rate!","Upgrade this shell catapult to further increase its power!","Upgrade this water sprayer to further increase its range and attack speed!","Upgrade this stick slingshot to further increase its pierce and power!","Upgrade this oil tower to further increase its effectiveness!"};
        costs = new List<KeyValuePair<int,int>>[3][];
        isActive = false;
        costs[0] = new List<KeyValuePair<int,int>>[5]{
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,10)},
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,25)},
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,15)},
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,40)},
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,50)}
        };
        costs[1] = new List<KeyValuePair<int,int>>[5]{
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,20)},
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,50),new KeyValuePair<int, int>(1,2)},
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,30),new KeyValuePair<int, int>(2,1)},
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,80),new KeyValuePair<int, int>(2,2)},
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,100),new KeyValuePair<int, int>(3,2)}
        };
        costs[2] = new List<KeyValuePair<int,int>>[5]{
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,40)},
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,100),new KeyValuePair<int, int>(1,2)},
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,60),new KeyValuePair<int, int>(2,1)},
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,160),new KeyValuePair<int, int>(2,2)},
            new List<KeyValuePair<int,int>>{new KeyValuePair<int, int>(0,200),new KeyValuePair<int, int>(3,2)}
        };
    }

    
    bool CheckCosts(int id, int building){
        bool ans = true;
        foreach(KeyValuePair<int,int> x in costs[id][building]){
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


    void PayCosts(int id, int building){
        foreach(KeyValuePair<int,int> x in costs[id][building]){
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


    void UpdateStates(){
        switch(BuildingRef.tag){
            case "Basic Turret":
                buildingLevel = BuildingRef.GetComponent<BasicTurret>().level;
                buildingName = "Sand Shooter";
                buildingState = BuildingRef.GetComponent<BasicTurret>().isDamaged;
                id = 0;
                break;
            case "Heavy Turret":
                buildingLevel = BuildingRef.GetComponent<HeavyTurret>().level;
                buildingName = "Shell Catapult";
                buildingState = BuildingRef.GetComponent<HeavyTurret>().isDamaged;
                id = 1;
                break;
            case "Water Turret":
                buildingLevel = BuildingRef.GetComponent<WaterTurretScript>().level;
                buildingName = "Water Sprayer";
                buildingState = BuildingRef.GetComponent<WaterTurretScript>().isDamaged;
                id = 2;
                break;
            case "Piercing Turret":
                buildingLevel = BuildingRef.GetComponent<PiercingTower>().level;
                buildingName = "Stick Slingshot";
                buildingState = BuildingRef.GetComponent<PiercingTower>().isDamaged;
                id = 3;
                break;
            case "Oil Turret":
                buildingLevel = BuildingRef.GetComponent<OilTurretScript>().level;
                buildingName = "Oil Turret";
                buildingState = BuildingRef.GetComponent<OilTurretScript>().isDamaged;
                id = 4;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
    

       

        if (Input.GetMouseButtonDown(0)){
            transform.GetChild(0).gameObject.SetActive(true);
            isActive = true;
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit;
            // Casts the ray and get the first game object hit
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.transform!=null){

                switch(hit.transform.gameObject.tag){
                    case "Basic Turret":
                        BuildingRef = hit.transform.gameObject;
                        break;
                    case "Heavy Turret":
                        BuildingRef = hit.transform.gameObject;
                        break;
                    case "Water Turret":
                        BuildingRef = hit.transform.gameObject;
                        break;
                    case "Piercing Turret":
                        BuildingRef = hit.transform.gameObject;
                        break;
                    case "Oil Turret":
                        BuildingRef = hit.transform.gameObject;
                        break;
                    case "UIUpgradeButton":
                        onClicked();
                        break;
                    default:
                        transform.GetChild(0).gameObject.SetActive(false);
                        isActive = false;
                        break;
                }
            }
            else{

                transform.GetChild(0).gameObject.SetActive(false);
                isActive = false;
            }
        }

        if(isActive){
            UpdateStates();
            //update text and button
            transform.GetChild(0).gameObject.GetComponent<UpgradePromptScript>().changeTo(buildingName,buildingLevel,buildingState);
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit;
            // Casts the ray and get the first game object hit
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.transform!=null){
                if(hit.transform.gameObject.tag=="UIUpgradeButton"){
                    TooltipScript.instance.gameObject.SetActive(true);
                    if(buildingState){
                        TooltipScript.instance.onActivate(descrs[0][id],costs[0][id]);
                    }
                    else if(buildingLevel<3){
                        TooltipScript.instance.onActivate(descrs[buildingLevel][id],costs[buildingLevel][id]);
                    }
                }
            }


            transform.position -= 0.1f*(transform.position.y+3.56f)*Vector3.up;


        }
        else{
            
            transform.position -= 0.1f*(transform.position.y+8)*Vector3.up;
        }

    }

    public void onClicked(){
        System.Action upgrade = null;
        System.Action fix = null;
        int buildingId = 0;
        switch(BuildingRef.tag){
            case "Basic Turret":
                upgrade = BuildingRef.GetComponent<BasicTurret>().UpgradeTower;
                fix = BuildingRef.GetComponent<BasicTurret>().FixTurret;
                buildingId = 0;
                break;
            case "Heavy Turret":
                upgrade = BuildingRef.GetComponent<HeavyTurret>().UpgradeTower;
                fix = BuildingRef.GetComponent<HeavyTurret>().FixTurret;
                buildingId = 1;
                break;
            case "Water Turret":
                upgrade = BuildingRef.GetComponent<WaterTurretScript>().UpgradeTurret;
                fix = BuildingRef.GetComponent<WaterTurretScript>().FixTurret;
                buildingId = 2;
                break;
            case "Piercing Turret":
                upgrade = BuildingRef.GetComponent<PiercingTower>().UpgradeTower;
                fix = BuildingRef.GetComponent<PiercingTower>().FixTurret;
                buildingId = 3;
                break;
            case "Oil Turret":
                upgrade = BuildingRef.GetComponent<OilTurretScript>().UpgradeTower;
                fix = BuildingRef.GetComponent<OilTurretScript>().FixTurret;
                buildingId = 4;
                break;
        }
        if(buildingState){
            if(CheckCosts(0,buildingId)){
                PayCosts(0,buildingId);
                fix();
                sfx.Play();
                transform.GetChild(0).gameObject.SetActive(false);
                isActive = false;

            }
        }
        else if(buildingLevel<3){
            if(CheckCosts(buildingLevel,buildingId)){
                PayCosts(buildingLevel,buildingId);
                sfx.Play();
                upgrade();
                transform.GetChild(0).gameObject.SetActive(false);
                isActive = false;
            }
        }
    }
}
