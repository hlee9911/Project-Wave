using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    public float yBound = 6f;
    public float xBound = 12f;
    private float dmg = 3;
    private TurretAudioManager turretAudioManager;

    private void Start()
    {
        turretAudioManager = FindObjectOfType<TurretAudioManager>();
    }
    void Update()
    {
       if (transform.position.x < -xBound || transform.position.x > xBound
            || transform.position.y < -yBound || transform.position.y > yBound)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // can have specific tags for each enemy if we want them to have different health
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyManager>().TakeSingleDamage(dmg);
            turretAudioManager.PlayTurretSound("Sand Hit");
            //collision.gameObject.GetComponent<EnemyManager>().ProcessDying();
            Destroy(gameObject);
            //call for damage on enemy, use whisper or something
        }
    }
    
}
