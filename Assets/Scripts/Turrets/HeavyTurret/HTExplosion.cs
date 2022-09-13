using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTExplosion : MonoBehaviour
{
    public float explosionLifeSpan = 2f;
    public float dmg = 5;
    private TurretAudioManager turretAudioManager;
    void Start()
    {
        turretAudioManager = FindObjectOfType<TurretAudioManager>();
        StartCoroutine(Die());
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(explosionLifeSpan);
        Destroy(gameObject);
    }
    public void SetDmg(int dmgLvl)
    {
        dmg = dmgLvl;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // can have specific tags for each enemy if we want them to have different health
        if (collision.tag == "Enemy")
        {
            turretAudioManager.PlayTurretSound("Clam Hit");
            collision.gameObject.GetComponent<EnemyManager>().TakeSingleDamage(dmg);
            //collision.gameObject.GetComponent<EnemyManager>().ProcessDying();
        }
    }
}

