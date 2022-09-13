using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDecay : MonoBehaviour
{
    private float lifeSpan = 5;
    private float despawnInterval = 2.0f;
    private float despawnIntervalDeltaTime = .125f;
    public GameManagementSO managementSO;
    public ResourceCollectionAnim resourceCollectionAnim;
    private TurretAudioManager turretAudioManager;
    void Start()
    {
        managementSO = FindObjectOfType<GameManagementSO>();
        resourceCollectionAnim = FindObjectOfType<ResourceCollectionAnim>();
        turretAudioManager = FindObjectOfType<TurretAudioManager>();
        StartCoroutine(Die());
    }

    private void OnMouseEnter()
    {
        switch(gameObject.tag)
        {
            case "Shell":
                resourceCollectionAnim.CollectItem(gameObject.tag, gameObject);
                break;
            case "Stick":
                resourceCollectionAnim.CollectItem(gameObject.tag, gameObject);
                break;
            case "Oil":
                resourceCollectionAnim.CollectItem(gameObject.tag, gameObject);
                break;
        }
    }

    // Flashing animation
    IEnumerator Die()
    {
        yield return new WaitForSeconds(lifeSpan);
        if (gameObject != null)
        {
            Vector3 originalSize = gameObject.transform.localScale;

            for (float i = 0; i < despawnInterval; i += despawnIntervalDeltaTime)
            {
                if (gameObject != null)
                {
                    if (gameObject.transform.localScale == originalSize)
                    {
                        ScaleModelTo(Vector3.zero, gameObject);
                    }
                    else
                    {
                        ScaleModelTo(originalSize, gameObject);
                    }
                    yield return new WaitForSeconds(despawnIntervalDeltaTime);
                }

            }

            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
        Destroy(gameObject);
    }
    private void ScaleModelTo(Vector3 scale, GameObject gameObject)
    {
        gameObject.transform.localScale = scale;
    }

    /*
    // Test code
private void DieInstant(string objectTag)
{
    Debug.Log("picked up " + objectTag);
    Destroy(gameObject);

    if (objectTag == "Shell")
    {
        Debug.Log("Doing something with shell");
        ++managementSO.Currency2;
    }
    if (objectTag == "Stick")
    {
        Debug.Log("Doing something with stick");
        ++managementSO.Currency3;
    }
    if (objectTag == "Oil")
    {
        Debug.Log("Doing something with oil");
        ++managementSO.Currency4;
    }

}
*/
}
