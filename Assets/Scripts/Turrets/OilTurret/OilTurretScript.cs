using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilTurretScript : MonoBehaviour
{
    private bool targetSelected = false;
    private bool isSelected = false;
    private bool isCrosshairActive = false;
    private bool isFiring = false;
    public bool isDamaged = false;
    public int level = 1;

    private TurretAudioManager turretAudioManager;
    public bool isPlayerSelecting = false;
    public GameObject crosshair;
    public GameObject oilBarrel;
    public GameObject oilSpill;
    public GameObject upgradeRing;
    public GameObject damageRing;
    public Animator animator;
    public float startingBarrelSize = .5f;
    public float maxBarrelSize = 1f;
    private float growthTime = 10f;
    private float lvl2Dmg = .3f;
    private float lvl3Dmg = .5f;

    private GameObject targeter;
    private Vector2 target;
    private Quaternion originalRot;
    private float distanceToTarget;
    private Vector2 directionToFire;
    private Vector2 turretPos;
    //private float rateOfChange;
    private float timeUntilImpact = 3f;
    private float fireRate = 10f;
    // gonna need to set a var in gameManager to keep players from picking multiple turrets simultaneously
    // and a gameActive of course
    // perhaps have a UI button made later specifically for changing target

    /*
    private void OnMouseDown()
    {
        UpgradeTower();
        Debug.Log("upgraded " + level);
    }
    */
    private void Start()
    {
        originalRot = transform.rotation;
        turretPos = transform.position;
        upgradeRing.SetActive(false);
        damageRing.SetActive(false);
        turretAudioManager = FindObjectOfType<TurretAudioManager>();
    }
    private void OnMouseOver()
    {   
        if (Input.GetMouseButtonDown(1))
        {
            if (isSelected)
            {
                isSelected = false;
                targetSelected = false;
                if (targeter != null)
                {
                    destroyCrosshair();
                }
                transform.rotation = originalRot;

            }
            else if (!isSelected)
            {
                if (targetSelected)
                {
                    // put button prompt here for UI
                    // if(button pressed) {
                    // targetSelected = false;
                    //   }
                }

                isSelected = true;
                if (!targetSelected)
                {
                    targeter = Instantiate(crosshair, transform.position, Quaternion.identity);
                    isCrosshairActive = true;
                }
                
            }
        }
       
    }
    void Update()
    {
        //set isPlayerSelecting to a FindObjectOfType<gameManager>(). bool here
        //Debug.Log(isSelected);
        if (isCrosshairActive)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
            if (targeter != null) { targeter.transform.position = mousePos; }
            /*
            Vector2 direction = mousePos - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            */
            if (Input.GetKeyDown("space") && !targetSelected)
            {
                targetSelected = true;
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                distanceToTarget = Vector2.Distance(transform.position, target);
                directionToFire = target - turretPos;
                //rateOfChange = distance / bulletSpeed;
                //line below demonstrates targeting ability
                //Instantiate(crosshair, target, Quaternion.identity);
                isSelected = false;
                StartCoroutine(launchProjectile());
                // need to get rid of crosshair 
                // can also add some lines to make mouse visible/invisiblie
                destroyCrosshair();
                isCrosshairActive = false;
            }
        }

    }
    public void SetDamage()
    {
        isDamaged = true;
        damageRing.SetActive(true);
        fireRate *= 2;
    }
    public void FixTurret()
    {
        isDamaged = false;
        damageRing.SetActive(false);
        fireRate /= 2;
    }
    IEnumerator launchProjectile()
    {   
        if (!isFiring)
        {
            while (targetSelected)
            {
                animator.SetTrigger("Fired");
                turretAudioManager.PlayTurretSound("Oil Shoot");
                isFiring = true;
                GameObject projectile = Instantiate(oilBarrel, turretPos, Quaternion.identity);
                float elapsedTime = 0f;
                float growTime = 0f;
                float shrinkTime = 0f;
                Vector2 maxScale = new Vector2(1f, 1f);
                Vector2 enlargedScale = new Vector2(1f, 1f);
                while (elapsedTime < timeUntilImpact)
                {
                    projectile.transform.position = Vector2.Lerp(turretPos, target, (elapsedTime / timeUntilImpact));

                    float currentBarrelPos = Vector2.Distance(turretPos, projectile.transform.position);
                    float vertex = distanceToTarget / 2;
                    if (currentBarrelPos < vertex)
                    {
                        float newScale = Mathf.Lerp(startingBarrelSize, maxBarrelSize, growTime / growthTime);
                        projectile.transform.localScale = new Vector2(newScale, newScale);
                        growTime += Time.deltaTime;
                    }
                    if (currentBarrelPos == vertex)
                    {
                        enlargedScale = projectile.transform.localScale;
                    }
                    if (currentBarrelPos > vertex)
                    {
                        // this is very stupid and I am dumb, but it works
                        float newScale = Mathf.Lerp(enlargedScale.x, -2f, shrinkTime / growthTime);
                        projectile.transform.localScale = new Vector2(newScale, newScale);
                        shrinkTime += Time.deltaTime;
                    }
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                Destroy(projectile);
                //instantiate a prefab for the area affected by oil
                GameObject spill = Instantiate(oilSpill, target, Quaternion.identity);
                if (level == 2)
                {
                    spill.GetComponent<OilSpill>().LevelUp();
                }
                if (level == 3)
                {
                    spill.GetComponent<OilSpill>().LevelUp();
                    spill.GetComponent<OilSpill>().LevelUp();
                }
                yield return new WaitForSeconds(fireRate);
                isFiring = false;
                
            }
        }
       
    }
    public void UpgradeTower()
    {
        ++level;
        upgradeRing.SetActive(true);
    }

    void destroyCrosshair()
    {
        if (targeter != null)
        {
            Destroy(targeter);
            isCrosshairActive = false;
        }
    }
}
