using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTurretScript : MonoBehaviour
{

    private float turretRange = 1.8f;
    private float turretFireRate = 2f;
    public GameObject projectile;
    public GameObject projectileLVL2;
    private TurretAudioManager turretAudioManager;
    // not too sure how this is going to work with the scriptable objects
    private EnemyManager _enemyScript;
    private GameObject enemy;
    public GameObject upgradeRing;
    public GameObject damageRing;
    public Animator animator;
    private bool engaged = false;
    private bool engagedFire = false;
    //private float bulletSpeed = 10f;
    public int level = 1;
    public bool isDamaged = false;
    private Collider2D collider;


    /*
    private void OnMouseDown()
    {
        UpgradeTurret();
        Debug.Log("upgraded " + turretFireRate);
    }
    */
    private void Start()
    {
        upgradeRing.SetActive(false);
        damageRing.SetActive(false);
        turretAudioManager = FindObjectOfType<TurretAudioManager>();
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(engaged);
        if (!engaged)
        {
            Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, turretRange);
            if (collider.Length >= 1)
            {
                for (int i = 0; i < collider.Length; i++)
                {
                    if (collider[i].gameObject != null && collider[i].gameObject.CompareTag("Enemy"))
                    {
                        enemy = collider[i].gameObject;
                        _enemyScript = enemy.GetComponent<EnemyManager>();
                        if (_enemyScript.EnemyCurrentHP > 0)
                        {
                            engaged = true;
                        }
                    }
                }
            }
        }
        if (engaged)
        {
            if (!engagedFire)
            {
                engagedFire = true;
                StartCoroutine(FireWeapon());
            }
            if (Mathf.Abs(Vector2.Distance(enemy.transform.position, transform.position)) > turretRange)
            {
                engaged = false;
            }
            if (enemy == null || _enemyScript.EnemyCurrentHP <= 0)
            {
                engaged = false;
            }

        }
    }

    IEnumerator FireWeapon()
    {
        while (engaged)
        {
            animator.SetTrigger("Firing");
            turretAudioManager.PlayTurretSound("Water Shoot");
            //if (level == 2) { Instantiate(projectileLVL2, transform.position, Quaternion.identity); }
            Instantiate(projectile, transform.position, Quaternion.identity); 
            yield return new WaitForSeconds(turretFireRate);
        }
        engagedFire = false;
    }

    public void SetDamage()
    {
        isDamaged = true;
        damageRing.SetActive(true);
        turretFireRate *= 2;
    }
    public void FixTurret()
    {
        isDamaged = false;
        damageRing.SetActive(false);
        turretFireRate /= 2;
    }
    public void UpgradeTurret()
    {
        ++level;
        turretFireRate /= 2;
        upgradeRing.SetActive(true);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, turretRange);
    }
}
