using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyTurret : MonoBehaviour
{
    public GameObject heavyTurretProjectile;
    public GameObject heavyTurretProjectileExplosion;
    public GameObject heavyTurretProjectileExplosionLVL2;
    public GameObject heavyTurretProjectileExplosionLVL3;
    public float heavyTurretRange = 6f;
    public float heaveTurretFireRate = 3f;
    public GameObject enemy;
    public GameObject upgradeRing;
    public GameObject damageRing;
    private TurretAudioManager turretAudioManager;
    // not too sure how this is going to work with the scriptable objects
    private EnemyManager _enemyScript;
    public Animator animator;
    public Vector3 direction;
    private bool engaged = false;
    private bool lockOn = false;
    private Vector2 target;
    private float bulletSpeed = 8f;
    private int dmg = 5;
    private Vector2 turretPos;
    public int level = 1;
    public bool isDamaged = false;

    /*
    private void OnMouseDown()
    {
        UpgradeTower();
        Debug.Log("damage = " + dmg);
    }
    */
    void Start()
    {
        turretAudioManager = FindObjectOfType<TurretAudioManager>();
        turretPos = transform.position;
        upgradeRing.SetActive(false);
        damageRing.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (!lockOn)
        {
            Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, heavyTurretRange);
            if (collider.Length >= 1)
            {
                for (int i = 0; i < collider.Length; i++)
                {
                    if (collider[i].gameObject.CompareTag("Enemy"))
                    {
                        enemy = collider[i].gameObject;
                        _enemyScript = enemy.GetComponent<EnemyManager>();
                        lockOn = true;
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
            if (Mathf.Abs(Vector2.Distance(enemy.transform.position, transform.position)) > heavyTurretRange)
            {
                Debug.Log("enemy out of range!");
                lockOn = false;
            }
            
        }

    }
    public void SetDamage()
    {
        isDamaged = true;
        damageRing.SetActive(true);
        heaveTurretFireRate *= 2;
    }

    public void FixTurret()
    {
        isDamaged = false;
        damageRing.SetActive(false);
        heaveTurretFireRate /= 2;
    }
    public void UpgradeTower()
    {
        ++level;
        dmg += 5;
        upgradeRing.SetActive(true);
    }

    IEnumerator FireWeapon()
    {
        
        while (lockOn)
        {
            animator.SetTrigger("Fire");
            turretAudioManager.PlayTurretSound("Clam Shoot");
            //target = enemy.transform.position;
            GameObject bullet = (GameObject)Instantiate(heavyTurretProjectile, transform.position, Quaternion.identity);
            bullet.GetComponent<HTProjectile>().LevelUp(dmg * level); 
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = direction * bulletSpeed;
            yield return new WaitForSeconds(heaveTurretFireRate);
            /*
            float elapsedTime = 0f;
            while (elapsedTime < timeUntilImpact)
            {
                bullet.transform.position = Vector2.Lerp(turretPos, target, (elapsedTime / timeUntilImpact));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Vector2 explosionPos = bullet.transform.position;
            Instantiate(heavyTurretProjectileExplosion, explosionPos, Quaternion.identity);
            Destroy(bullet);
            */

        }
        engaged = false;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Gizmos.DrawWireSphere(transform.position, heavyTurretRange);
    }
}
