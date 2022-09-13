using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryUp : MonoBehaviour
{
    private float disappearanceTime = 1.5f;
    public float lifeSpan = 6f;
    void Start()
    {
        StartCoroutine(Disappear());
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
