using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurret : MonoBehaviour
{
    private float turretRange = 3f;
    private float turretFireRate = 1f;

    public TurretAudioManager turretAudioManager;
    private bool lockOn = false;
    public bool isDamaged = false;
    public int level = 1;
    public GameObject enemy;
    // not too sure how this is going to work with the scriptable objects
    private EnemyManager _enemyScript;
    public GameObject projectile;
    public GameObject upgradeRing;
    public GameObject damagedRing;
    public Animator animator;

    public Vector3 direction;
    private bool engaged = false;
    private float bulletSpeed = 10f;

    
    // just to test the upgrade system
    /*
    private void OnMouseDown()
    {
        UpgradeTower();
        Debug.Log("fire rate = " + turretFireRate);
        Debug.Log("bullet speed = " + bulletSpeed);
    }
    */
    private void Start()
    {
        turretAudioManager = FindObjectOfType<TurretAudioManager>();
        upgradeRing.SetActive(false);
        damagedRing.SetActive(false);
    }

    void Update()
    {
        //Debug.Log(lockOn);
        if (!lockOn)
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
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Color color = Color.white;
            Debug.DrawRay(transform.position, direction, color);

            if (!engaged)
            {
                engaged = true;
                StartCoroutine(FireWeapon());
            }
            if (enemy == null || _enemyScript.EnemyCurrentHP <= 0)
            {
                Debug.Log("enemy dead!");
                lockOn = false;
            }
            if (Mathf.Abs(Vector2.Distance(enemy.transform.position, transform.position)) > turretRange)
            {
                Debug.Log("enemy out of range!");
                lockOn = false;
            }
            
        }
       

    }

    public void EnemyDead()
    {
        lockOn = false;
    }
    public void UpgradeTower()
    {
        ++level;
        if (level == 2)
        {
            turretFireRate /= 2;
        }
        if (level == 3)
        {
            turretFireRate /= 2;
        }

        upgradeRing.SetActive(true);
    }
    IEnumerator FireWeapon()
    {
        //Debug.Log("turret is locked on!");
        while(lockOn)
        {
            animator.SetTrigger("Fire");
            turretAudioManager.PlayTurretSound("Sand Shoot");
            GameObject bullet = (GameObject) Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = direction * bulletSpeed;
            yield return new WaitForSeconds(turretFireRate);
        }
        engaged = false;
    }

    public void SetDamage()
    {
        //Debug.Log("turret damaged!");
        damagedRing.SetActive(true);
        isDamaged = true;
        turretFireRate *= 2;
    }
    public void FixTurret()
    {
        isDamaged = false;
        damagedRing.SetActive(false);
        turretFireRate /= 2;
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, turretRange);
    }
    
}
