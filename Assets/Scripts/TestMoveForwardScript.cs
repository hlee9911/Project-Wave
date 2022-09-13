using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveForwardScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float health = 3;
    public float speed = 4;
    private bool isPoisoned = false;
    private bool isSlowed = false;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-transform.right * Time.deltaTime * speed);
        //Debug.Log("monster health = " + health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("TurretBullet") && gameObject != null)
        {
            --health;
        }
        if (collision.gameObject.CompareTag("HTExplosion") && gameObject != null)
        {
            //Debug.Log("Collision occurs!");
            --health;
        }
        if (collision.gameObject.CompareTag("Water Splash") && gameObject != null && !isSlowed)
        {
            isSlowed = true;
            StartCoroutine(DecreasedSpeed());
        }
        if (collision.gameObject.CompareTag("OilSpill") && gameObject != null && !isPoisoned)
        {
            isPoisoned = true;
            StartCoroutine(Poisoned());
        }
    }

    IEnumerator DecreasedSpeed()
    {
        speed -= 3;
        yield return new WaitForSeconds(3f);
        isSlowed = false;
        speed += 3;
    }
    IEnumerator Poisoned()
    {
        int count = 0;
        while (count < 4)
        {
            health -= 1;
            yield return new WaitForSeconds(1f);
            ++count;
        }
        isPoisoned = false;
       
    }
}
