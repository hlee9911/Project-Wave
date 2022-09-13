using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollectionAnim : MonoBehaviour
{
    public GameObject[] resourceSprites;
    public PlayerGameManager managementSO;
    private TurretAudioManager turretAudioManager;
    private float movementTime = 1.5f;
    Vector2 target = new Vector2(-7f, 4.2f);

    void Start()
    {
        turretAudioManager = FindObjectOfType<TurretAudioManager>();
        managementSO = FindObjectOfType<PlayerGameManager>();
        managementSO.GameManagementSO.Currency2 = 0;
        managementSO.GameManagementSO.Currency3 = 0;
        managementSO.GameManagementSO.Currency4 = 0;
    }
    public void CollectItem(string objectTag, GameObject gameObject)
    {
        turretAudioManager.PlayTurretSound("Collect");
        Destroy(gameObject);
        if (objectTag == "Shell")
        {
            GameObject shell = Instantiate(resourceSprites[0], gameObject.transform.position, Quaternion.identity);
            StartCoroutine(TakeMeHome(shell));
            ++managementSO.GameManagementSO.Currency2;
        }
        if (objectTag == "Stick")
        {
            GameObject stick = Instantiate(resourceSprites[1], gameObject.transform.position, Quaternion.identity);
            StartCoroutine(TakeMeHome(stick));
            ++managementSO.GameManagementSO.Currency3;
        }
        if (objectTag == "Oil")
        {
            GameObject oil = Instantiate(resourceSprites[2], gameObject.transform.position, Quaternion.identity);
            StartCoroutine(TakeMeHome(oil));
            ++managementSO.GameManagementSO.Currency4;
        }
    }

    IEnumerator TakeMeHome(GameObject resource)
    {
        float elapsedTime = 0f;
        Vector3 ogPos = resource.transform.position;
        while (elapsedTime < movementTime)
        {
            resource.transform.position = Vector2.Lerp(ogPos, target, elapsedTime / movementTime);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        Destroy(resource);
    }
}
