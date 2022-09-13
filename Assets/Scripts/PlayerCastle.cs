using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manages the data of the player's castle
public class PlayerCastle : MonoBehaviour
{
    [Header("Player Castle Data Field")]
    [SerializeField] private float maxCastleHP = 300f;
    [SerializeField] private float currentCastleHP;
    [SerializeField] private float destroyDelay = 0.2f;
    [SerializeField] private GameObject healthBarGO;
    [SerializeField] private ParticleSystem castleDestoryedVFX;
    private CameraShake cameraShake;
    private string playerCastleName;
    private BoxCollider2D boxCollider;
    private Slider healthSlider;

    // getter and setter method
    public float MaxCastleHP { get { return maxCastleHP; } set { maxCastleHP = value; } }
    public float CurrentCastleHP { get { return currentCastleHP; } set { currentCastleHP = value; } }

    // Process the castle taking damage sequence
    public void CastleTakeDamage(float dmgMult)
    {
        currentCastleHP = Mathf.Clamp(currentCastleHP - dmgMult, 0f, maxCastleHP);
        healthSlider.value = currentCastleHP / maxCastleHP;
        ShakeCamera();
        Debug.Log($"{playerCastleName}'s HP: {currentCastleHP}");
    }

    // stop the camera shake
    void DestroyCastle()
    {
        if (cameraShake != null)
        {
            cameraShake.Stop();
            Debug.Log("Stopping the camera shake");
        }
        /*
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
        */
        DisplayCastleDestroyedParitcleEffect();
        Invoke("DestoryTheCastle", 0.15f);
    }

    void Awake()
    {
        playerCastleName = gameObject.name;
        cameraShake = Camera.main.GetComponent<CameraShake>();
        boxCollider = GetComponent<BoxCollider2D>();
        currentCastleHP = maxCastleHP;
        if (healthBarGO != null)
        {
            healthSlider = healthBarGO.GetComponent<Slider>();
            healthSlider.value = currentCastleHP / maxCastleHP;
        }
    }

    void Update()
    {
        if (currentCastleHP <= 0f)
        {
            DestroyCastle();
        }
    }

    void DestoryTheCastle()
    {
        Destroy(gameObject, destroyDelay);
    }

    void DisplayCastleDestroyedParitcleEffect()
    {
        if (castleDestoryedVFX != null)
        {
            ParticleSystem instance = Instantiate(castleDestoryedVFX,
                                                  transform.position + Vector3.left * 9f,
                                                  Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    // shakes the main camera
    public void ShakeCamera()
    {
        if (cameraShake != null)
        {
            cameraShake.Play();
        }
    }

}
