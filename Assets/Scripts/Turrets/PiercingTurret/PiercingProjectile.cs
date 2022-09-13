using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingProjectile : MonoBehaviour
{
    private int health = 3;
    public float yBound = 6f;
    public float xBound = 12f;
    private float dmgMult = 8;
    private float bulletSpeed = 12f;
    private TurretAudioManager turretAudioManager;

    private void Start()
    {
        turretAudioManager = FindObjectOfType<TurretAudioManager>();
        switch(gameObject.tag)
        {
            case "sticklvl1":
                health = 3;
                break;
            case "sticklvl2":
                health = 6;
                break;
            case "sticklvl3":
                health = 9;
                break;
        }
    }
    void Update()
    {
        if (transform.position.x < -xBound || transform.position.x > xBound
            || transform.position.y < -yBound || transform.position.y > yBound)
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector2.right * Time.deltaTime * bulletSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // can have specific tags for each enemy if we want them to have different health
        if (collision.gameObject.CompareTag("Enemy") && gameObject != null)
        {
            collision.gameObject.GetComponent<EnemyManager>().TakeSingleDamage(dmgMult);
            turretAudioManager.PlayTurretSound("Stick Hit");
            //collision.gameObject.GetComponent<EnemyManager>().ProcessDying();
            --health;
            if (health == 0) { Destroy(gameObject); }
        }
    }
}
