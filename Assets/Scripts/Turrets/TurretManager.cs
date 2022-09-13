using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [SerializeField] private TurretSO turretData;
    private string turretName;
    private float turretHealth;
    private float turretCurrentHP;
    private float turretFireRate;
    private float turretBulletSpeed;
    private float turretRange;
    private Sprite turretArtwork;
    private GameObject turretBullet;
    private bool damaged = false;

    // logic for basic turret lock on system
    private bool lockOn = false;
    public GameObject enemy;
    // not too sure how this is going to work with the scriptable objects
    private TestMoveForwardScript _enemyScript;

    public GameObject projectile;
    public Vector3 direction;
    private bool engaged = false;

    public TurretSO TurretData { set { turretData = value; } }

    // Start is called before the first frame update
    void Start()
    {
        InitializeTurretData();
    }

    void InitializeTurretData()
    {
        switch (turretData.TurretName)
        {
            case "Basic Turret":
                break;
        }
        turretName = turretData.name;
        turretHealth = turretData.Health;
        turretCurrentHP = turretData.CurrentHP;
        turretFireRate = turretData.FireRate;
        turretBulletSpeed = turretData.BulletSpeed;
        turretBullet = turretData.Bullet;
        turretRange = turretData.TurretRange;
    }

    // Update is called once per frame
    void Update()
    {

        if (!lockOn)
        {
            Collider2D collider = Physics2D.OverlapCircle(transform.position, turretRange);
            if (collider != null && collider.gameObject.tag == "Enemy")
            {
                enemy = collider.gameObject;
                _enemyScript = enemy.GetComponent<TestMoveForwardScript>();
                lockOn = true;
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

            if (Vector2.Distance(enemy.transform.position, transform.position) > turretRange || enemy == null)
            {
                lockOn = false;
                //engaged = false;
            }


        }
        if (lockOn && !engaged)
        {
            engaged = true;
            StartCoroutine(FireWeapon());
        }
    }
    IEnumerator FireWeapon()
    {
        while (lockOn)
        {
            GameObject bullet = (GameObject)Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = direction * turretBulletSpeed;
            yield return new WaitForSeconds(turretFireRate);
        }
        engaged = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, turretRange);
    }
}
