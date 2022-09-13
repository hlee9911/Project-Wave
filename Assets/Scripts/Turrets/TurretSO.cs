using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Turret", menuName = "Turret/Basic")]
public class TurretSO : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private float health;
    [SerializeField] private float currentHP;
    [SerializeField] private float fireRate;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float turretRange;
    [SerializeField] private Sprite artwork;
    [SerializeField] private GameObject bullet;

    // getter and setter methods
    public string TurretName { get { return name; } set { name = value; } }
    public float Health { get { return health; } set { health = value; } }
    public float CurrentHP { get { return currentHP; } set { currentHP = value; } }
    public float FireRate { get { return fireRate; } set { fireRate = value; } }
    public float BulletSpeed { get { return bulletSpeed; } set { bulletSpeed = value; } }
    public float TurretRange { get { return turretRange; } set { turretRange = value; } }
    public GameObject Bullet { get { return bullet; } set { bullet = value; } }
}

