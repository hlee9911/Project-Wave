using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

// Contains enemy's data
[CreateAssetMenu(fileName = "Enemy Data", menuName = "ScriptableObjects/EnemyData", order = 2)]
public class EnemySO : ScriptableObject
{
    [Header("Enemy Data Field")]
    [SerializeField] private string enemyName;
    [SerializeField] private float maxHP;
    [SerializeField] private float currentHP;
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private int scoreForKilling;

    // getter and setter methods
    public string EnemyName { get { return enemyName; } set { enemyName = value; } }
    public float MaxHP { get { return maxHP; } set { maxHP = value; } }
    public float CurrentHP { get { return currentHP; } set { currentHP = value; } }
    public float Damage { get { return damage; } set { damage = value; } }
    public float AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }
    public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }
    public int SocreForKilling { get { return scoreForKilling; } set { scoreForKilling = value; } }
}
