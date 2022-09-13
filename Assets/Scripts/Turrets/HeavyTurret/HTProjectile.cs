using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTProjectile : MonoBehaviour
{
    public float yBound = 6f;
    public float xBound = 12f;
    public GameObject heavyTurretProjectileExplosion;
    public GameObject heavyTurretProjectileExplosionLVL2;
    public GameObject heavyTurretProjectileExplosionLVL3;
    private bool hasExploded = false;
    private int dmg = 5;
    void Update()
    {
        if (transform.position.x < -xBound || transform.position.x > xBound
             || transform.position.y < -yBound || transform.position.y > yBound)
        {
            Destroy(gameObject);
        }
    }
    public void LevelUp(int dmgLvl)
    {
        dmg = dmgLvl;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !hasExploded)
        {
            Vector2 explosionPos = collision.transform.position;
            Destroy(gameObject);
            GameObject explosion = Instantiate(heavyTurretProjectileExplosion, explosionPos, Quaternion.identity);
            explosion.GetComponent<HTExplosion>().SetDmg(dmg);
            hasExploded = true;
            //call for damage on enemy, use whisper or something
        }
    }
}
