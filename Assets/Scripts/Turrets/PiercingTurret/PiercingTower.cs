using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingTower : MonoBehaviour
{
    public float turretRange = 4f;
    public float turretFireRate = .75f;
    //private int dmg = 8;
    public int level = 1;

    private bool lockOn = false;
    public bool isDamaged = false;
    public GameObject enemy;
    private TurretAudioManager turretAudioManager;
    // not too sure how this is going to work with the scriptable objects
    private EnemyManager _enemyScript;
    public GameObject projectile;
    public GameObject projectileLVL2;
    public GameObject projectileLVL3;   
    public GameObject upgradeRing;
    public GameObject damageRing;
    public Animator animator;
    public Vector3 direction;
    private bool engaged = false;
    private float bulletSpeed = 12f;
    private Quaternion rot;


    /*
   private void OnMouseDown()
   {
       UpgradeTower();
       Debug.Log("upgraded!");
   }
   */
    private void Start()
    {
        upgradeRing.SetActive(false);
        damageRing.SetActive(false);
        turretAudioManager = FindObjectOfType<TurretAudioManager>();
    }
    void Update()
    {

        if (!lockOn)
        {
            Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, turretRange);
            if (collider.Length >= 1)
            {
                // Im sorry you had to see this
                for (int i = 0; i < collider.Length; i++)
                {
                    if (collider[i].gameObject != null && collider[i].gameObject.CompareTag("Enemy"))
                    {
                        enemy = collider[i].gameObject;
                        _enemyScript = enemy.GetComponent<EnemyManager>();
                        if (_enemyScript.EnemyCurrentHP > 0)
                        {
                            lockOn = true;
                        }
                    }
                }
            }
        }
        if (lockOn)
        {
            //Debug.Log(transform.rotation);
            
            direction = enemy.transform.position - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rot = Quaternion.AngleAxis(angle, Vector3.forward);
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            Color color = Color.white;
            //Debug.DrawRay(transform.position, direction, color);

            if (!engaged)
            {
                StartCoroutine(FireWeapon());
                engaged = true;
            }

            if (enemy == null || _enemyScript.EnemyCurrentHP <= 0)
            {
                lockOn = false;
            }

            if (Mathf.Abs(Vector2.Distance(enemy.transform.position, transform.position)) > turretRange)
            {
                lockOn = false;
                //engaged = false;
            }  

        }

    }
    public void EnemyDead()
    {
        lockOn = false;
    }
    IEnumerator FireWeapon()
    {
        while (lockOn)
        {
            animator.SetTrigger("Firing");
            turretAudioManager.PlayTurretSound("Stick Shoot");
            GameObject bullet;
            if (level == 3) { bullet = (GameObject)Instantiate(projectileLVL3, transform.position, rot); }
            if (level == 2) { bullet = (GameObject)Instantiate(projectileLVL2, transform.position, rot); }
            else { bullet = (GameObject)Instantiate(projectile, transform.position, rot); }
            //Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            //rb.velocity = direction * bulletSpeed;
            yield return new WaitForSeconds(turretFireRate);
        }
        engaged = false;
    }
    public void SetDamage()
    {
        isDamaged = true;
        damageRing.SetActive(true);
        turretFireRate *= 2;
        //Debug.Log(damageRing.activeSelf);
    }

    public void FixTurret()
    {
        isDamaged = false;
        damageRing.SetActive(false);
        turretFireRate /= 2;
    }
    public void UpgradeTower()
    {
        ++level;
        upgradeRing.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, turretRange);
    }

}
