using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum class for state of the game
public enum GameState { Paused, Gameplay, Placeholder };

// Scriptable Object that stores player's game data
[CreateAssetMenu(fileName = "GameManagementSO", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class GameManagementSO : ScriptableObject
{
    [Header("Player Data Field")]
    [SerializeField, Tooltip("Stores player's currency, sand")] private int currency1;
    [SerializeField, Tooltip("Stores player's currency, shell")] private int currency2;
    [SerializeField, Tooltip("Stores player's currency, placeholder atm")] private int currency3;
    [SerializeField, Tooltip("Stores player's currency, placeholder atm")] private int currency4;
    [SerializeField, Tooltip("Stores player's currency, placeholder atm")] private int currency5;
    [SerializeField, Tooltip("Stores the state of the gamestate")] public GameState gameState;

    // Getter and setter methods
    public int Currency1 { get { return currency1; } set { currency1 = value; } }
    public int Currency2 { get { return currency2; } set { currency2 = value; } }
    public int Currency3 { get { return currency3; } set { currency3 = value; } }
    public int Currency4 { get { return currency4; } set { currency4 = value; } }
    public int Currency5 { get { return currency5; } set { currency5 = value; } }
}
