using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : MonoBehaviour
{
    public float lifeSpan = 1f;
    private float disappearanceTime = .75f;
    float slowMult = 1;
    // half speed
    float slow = .6f;
    float slowDuration = 5f;

    void Start()
    {
        StartCoroutine(Disappear());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyManager>().DecreaseSpeed(slowDuration, slow * slowMult);
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
            float newA = Mathf.Lerp(a, 0, elapsedTime / disappearanceTime);
            Color color = new Color(r, g, b, newA);
            gameObject.GetComponent<SpriteRenderer>().color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
    
}
