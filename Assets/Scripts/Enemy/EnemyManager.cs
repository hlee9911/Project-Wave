using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manages the Enemy data
public class EnemyManager : MonoBehaviour
{
    // enemy data field
    [SerializeField] private GameObject target;
    [SerializeField] private EnemySO enemyData;
    private Animator animatorController;
    private string enemyNameType;
    private float enemyMaxHP;
    private float enemyCurrentHP;
    [SerializeField] private float enemyMoveSpeed;
    private float enemyDamage;
    private float enemyAttackDelay;
    private int enemyScoreKilling;
    [SerializeField] private List<RuntimeAnimatorController> enemyAnimatorControllers;
    [SerializeField] private ParticleSystem enemyDeathVFX;
    [SerializeField] private ParticleSystem enemyHitVFX;
    [SerializeField] private ParticleSystem enemySpawnVFX;
    [SerializeField] private ParticleSystem CastleHitVFX;
    [SerializeField] private GameObject healthBarGO;
    [SerializeField] private float timer;
    [SerializeField] private float dyingDelayAfterCastleDestoryed = 20f;
    [SerializeField] private float awakeDelay = 0.2f;
    [SerializeField] PlayerGameManager playerGameManager;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float deathDelay = 0.05f;
    private float enemyBaseSpeed;
    private Slider healthBarSlider;
    private bool isScoredApplied;
    private Color currentSpriteColor;

    // getter methods
    public string EnemyNameType { get { return enemyNameType; } }
    public float EnemyMaxHP { get { return enemyMaxHP; } }
    public float EnemyCurrentHP { get { return enemyCurrentHP; } }
    public float EnemyMoveSpeed { get { return enemyMoveSpeed; } set { enemyMoveSpeed = value; } }
    public float EnemyBaseSPeed { get { return enemyBaseSpeed; } }
    public float EnemyDamage { get { return enemyDamage; } }
    public float EnemyAttackDelay { get { return enemyAttackDelay; } }
    public int EnemyScoreKilling { get { return enemyScoreKilling; } }
    public ParticleSystem EnemySpawnVFX { get {return enemySpawnVFX; } }
    public GameObject CastleTarget { get { return target; } }

    // setter method for enemydata scriptable object
    public EnemySO EnemyData { set { enemyData = value; } }

    // Find the target when the object gets instantiate in the scene
    void Awake()
    {
        target = GameObject.FindWithTag("Castle");
        spriteRenderer = GetComponent<SpriteRenderer>();
        animatorController = GetComponent<Animator>();
        healthBarSlider = healthBarGO.GetComponent<Slider>();
        playerGameManager = FindObjectOfType<PlayerGameManager>();
        if (gameObject.GetComponent<GhostCrabManager>() != null)
        {
            gameObject.GetComponent<GhostCrabManager>().enabled = false;
        }
        if (spriteRenderer)
        {
            currentSpriteColor = spriteRenderer.color;
        }
        timer = 0f;
        isScoredApplied = false;
    }

    void Start()
    {
        InitializeEnemyData();
        StopForMoment();
    }

    // Initialize the enemy data by getting the information from Enemy Scriptable Object
    void InitializeEnemyData()
    {
        if (animatorController != null)
        {
            // enemy color variants based on enemy's name
            switch (enemyData.EnemyName)
            {
                case "Crab":
                    // spriteRenderer.color = Color.red;
                    animatorController.runtimeAnimatorController = enemyAnimatorControllers[0];
                    break;
                case "Burrowing Crab":
                    // spriteRenderer.color = Color.cyan;
                    animatorController.runtimeAnimatorController = enemyAnimatorControllers[1];
                    if (gameObject.GetComponent<GhostCrabManager>() != null)
                    {
                        gameObject.GetComponent<GhostCrabManager>().enabled = true;
                    }
                    break;
                case "Hermit Crab":
                    // spriteRenderer.color = Color.yellow;
                    animatorController.runtimeAnimatorController = enemyAnimatorControllers[2];
                    break;
                case "Lobster":
                    // spriteRenderer.color = Color.green;
                    animatorController.runtimeAnimatorController = enemyAnimatorControllers[3];
                    transform.localScale = new Vector3(0.85f, 0.85f, 1f);
                    break;
                case "Seagull":
                    // spriteRenderer.color = Color.blue;
                    animatorController.runtimeAnimatorController = enemyAnimatorControllers[4];
                    transform.localScale = new Vector3(0.67f, 0.67f, 1f);
                    break;
                default:
                    // spriteRenderer.color = Color.white;
                    break;
            }
        }
        gameObject.name = enemyData.EnemyName;
        enemyNameType = enemyData.EnemyName;
        enemyMaxHP = enemyData.MaxHP;
        enemyCurrentHP = enemyData.CurrentHP;
        enemyDamage = enemyData.Damage;
        enemyMoveSpeed = enemyData.MovementSpeed;
        enemyAttackDelay = enemyData.AttackSpeed;
        enemyBaseSpeed = enemyMoveSpeed;
        enemyScoreKilling = enemyData.SocreForKilling;
        healthBarSlider.value = enemyCurrentHP / enemyMaxHP;
        PlayDeathandSpawnEffect(enemySpawnVFX);
    }

    void Update()
    {
        MoveTowardsCastle();
        // TakeDamageDebug();
        if ((enemyCurrentHP <= 0f || isCastleDead()) && !isScoredApplied)
        {
            ProcessDying();
            isScoredApplied = true;
        }
        if (target == null)
        {
            timer += Time.deltaTime;
        }
    }
    
    public void StopForMoment()
    {
        StartCoroutine(StopForSecond());
    }

    IEnumerator StopForSecond()
    {
        enemyMoveSpeed = 0f;
        yield return new WaitForSeconds(awakeDelay);
        enemyMoveSpeed = enemyBaseSpeed;
    }

    bool isCastleDead()
    {
        if (timer >= dyingDelayAfterCastleDestoryed)
        {
            return true;
        }
        return false;
    }

    // method for processing dying when enemy's hp goes below or equal to 0
    public void ProcessDying()
    {
        PlayDeathandSpawnEffect(enemyDeathVFX);
        if (target && playerGameManager)
        {
            playerGameManager.PlayeCurrentScore += enemyScoreKilling;
        }
        Debug.Log($"{gameObject.name} has been eliminated!");
        Destroy(gameObject, deathDelay);
    }

    // take damage for debugging purpose
    void TakeDamageDebug()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeSingleDamage(25f);
        }
    }

    // method for taking a single hit
    public void TakeSingleDamage(float dmgMult)
    {
        enemyCurrentHP = Mathf.Clamp(enemyCurrentHP - dmgMult, 0f, enemyMaxHP);
        healthBarSlider.value = enemyCurrentHP / enemyMaxHP;
        ParticleHitEffect(enemyHitVFX);
        Debug.Log("Hit by something!");
        Debug.Log($"{gameObject.name}'s HP: {enemyCurrentHP}");
    }

    // method for slowing down the enemy
    public void DecreaseSpeed(float duration, float slowMult)
    {
        if (EnemyMoveSpeed > float.Epsilon)
        {
            StartCoroutine(SlowSpeed(duration, slowMult));
            Debug.Log($"{gameObject.name} has slowed down!");
        }
    }

    IEnumerator SlowSpeed(float duration, float slowMult)
    {
        // slow the enemy speed (note that it is not additive)
        enemyMoveSpeed *= slowMult;
        spriteRenderer.color = Color.blue;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = currentSpriteColor;
        enemyMoveSpeed /= slowMult;
        // enemyMoveSpeed = enemyBaseSpeed;
    }

    // Collision Detection
    void OnTriggerEnter2D(Collider2D other)
    {
        // When the enemy reaches the castle, stop moving
        if (other.gameObject.tag == "Castle")
        {
            enemyMoveSpeed = 0;
            DoDamageToCastle(other);
            Debug.Log("Collided with castle!");
        }
        if (other.gameObject.tag == "Enemy")
        {

        }
        /*
        if (target == null)
        {
            enemyMoveSpeed = enemyBaseSpeed;
        }
        */
    }

    // Method for damaging the player's castle
    void DoDamageToCastle(Collider2D other)
    {
        StartCoroutine(DamageTheCastle(other));
    }

    IEnumerator DamageTheCastle(Collider2D other)
    {
        PlayerCastle playercastle = other.gameObject.GetComponent<PlayerCastle>();
        if (other.gameObject != null)
        {
            if (playercastle != null)
            {
                while (other != null && other.gameObject.tag == "Castle")
                {
                    playercastle.CastleTakeDamage(enemyDamage);
                    ParticleHitEffect(CastleHitVFX);
                    yield return new WaitForSeconds(enemyAttackDelay);
                }
            }
        }
    }

    // Instantiating death and spwan effect
    public void PlayDeathandSpawnEffect(ParticleSystem particleVFX)
    {
        if (particleVFX != null)
        {
            ParticleSystem instance = Instantiate(particleVFX,
                                                  transform.position,
                                                  Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    // Instantiating hit effect (both for enemies and the player's castle)
    void ParticleHitEffect(ParticleSystem particleVFX)
    {
        if (particleVFX != null)
        {
            ParticleSystem instance = Instantiate(particleVFX,
                                                  transform.position + Vector3.left * 0.5f,
                                                  Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    // Moves towards the castle
    void MoveTowardsCastle()
    {
        float step = enemyMoveSpeed * Time.deltaTime;
        if (target == null)
        {
            enemyMoveSpeed = enemyBaseSpeed;
            step = enemyMoveSpeed * Time.deltaTime;
            transform.Translate(Vector2.left * step);
        }
        else
        {
            transform.Translate(Vector2.left * step);
        }
        /*
        if (target != null)
        {
            // To move forward
            // transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

            // or to move left (might need to figure this out later)
            transform.Translate(Vector2.left * step);
        }
        else
        {
            Debug.Log("Target could not be found!");
        }
        */
    }

}
