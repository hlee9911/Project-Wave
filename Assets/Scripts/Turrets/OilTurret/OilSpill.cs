using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSpill : MonoBehaviour
{
    private float lifeSpan = 5f;
    private float lifeSpanInc = 2f;
    private float disappearanceTime = 1.5f;
    private float dmg = 1;
    private float dmgFreq = 1f;
    private EnemyManager enemyManager;
    private TurretAudioManager turretAudioManager;
    private float enemyMaxHP;
    private float percentDmg = .1f;
    private void Start()
    {
        StartCoroutine(Disappear());
        turretAudioManager = FindObjectOfType<TurretAudioManager>();
    }
    /*
    public void LevelUp(float percent)
    {
        percentDmg = percent;  
    }
    */
    public void LevelUp()
    {
        lifeSpan += lifeSpanInc;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject enemy = collision.gameObject;
            enemyManager = enemy.GetComponent<EnemyManager>();
            enemyMaxHP = enemyManager.EnemyMaxHP;
            StartCoroutine(Poisoned(enemy));
        }
    }
    IEnumerator Poisoned(GameObject enemy)
    {
        float percentageOfDmg = enemyMaxHP * percentDmg;
        float enemyHp = enemyManager.EnemyCurrentHP;
        float newEnemyHp = enemyHp - percentageOfDmg;  
        while (enemy != null)
        {
            enemyHp = enemyManager.EnemyCurrentHP;
            turretAudioManager.PlayTurretSound("Oil Hit");
            enemyManager.TakeSingleDamage(percentageOfDmg);
            yield return new WaitForSeconds(dmgFreq);
        }
    }
    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(lifeSpan);
        float elapsedTime = 0f;
        float r = gameObject.GetComponent<SpriteRenderer>().color.r;
        float g = gameObject.GetComponent<SpriteRenderer>().color.g;
        float b = gameObject.GetComponent<SpriteRenderer>().color.b;
        float a = gameObject.GetComponent<SpriteRenderer>().color.a;

        while (elapsedTime < disappearanceTime)
        {
            float newA = Mathf.Lerp(a, 0, elapsedTime/disappearanceTime);
            Color color = new Color(r, g, b, newA);
            gameObject.GetComponent<SpriteRenderer>().color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
