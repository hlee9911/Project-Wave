using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Managaes the Ghost Crab enemy
public class GhostCrabManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem enemyPopOutVFX;
    [SerializeField] private float burrowDelay = 0.5f;
    [SerializeField] private EnemyManager myEnemyManager;
    [SerializeField] private float minTeleportDistance = 2f;
    [SerializeField] private float maxTeleportDistance = 10f;
    [SerializeField] private float minTeleportDelay = 3f;
    [SerializeField] private float maxTeleportDelay = 5f;
    [Range(0.0f, 1.0f)][SerializeField] private float teleportProbability = 0.3f;
    [SerializeField] private bool hasTeleportedOnce;
    private Animator myAnimtor;
    private float AnimationDelay = 1.5f;
    private float timer = 0f;
    // [SerializeField] private float enemyBaseSpeed;

    void Awake()
    {
        myEnemyManager = GetComponent<EnemyManager>();
        myAnimtor = GetComponent<Animator>();
        hasTeleportedOnce = false;
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        HandleTeleportation();
    }

    // handles the teleportation work during the runtime
    void HandleTeleportation()
    {
        if (timer > Random.Range(minTeleportDelay, maxTeleportDelay))
        {
            if (!hasTeleportedOnce && Random.Range(0f, 1f) > teleportProbability && myEnemyManager.CastleTarget)
            {
                Teleport();
                hasTeleportedOnce = true;
            }
        }
    }

    // Process the digging animation
    void Teleport()
    {
        myEnemyManager.EnemyMoveSpeed = 0f;
        GetComponent<BoxCollider2D>().enabled = false;
        myAnimtor.SetBool("isDig", true);
        Invoke("TranslateEnemy", AnimationDelay);
    }

    // method for the actual teleportation
    void TranslateEnemy()
    {
        transform.Translate(Vector2.left * Random.Range(minTeleportDistance, maxTeleportDistance));
        GetComponent<BoxCollider2D>().enabled = true;
        if (enemyPopOutVFX)
        {
            myEnemyManager.PlayDeathandSpawnEffect(enemyPopOutVFX);
        }
        myAnimtor.SetBool("isDig", false);
        myEnemyManager.EnemyMoveSpeed = myEnemyManager.EnemyBaseSPeed;
    }


}
