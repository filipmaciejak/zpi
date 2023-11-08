using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechHealth : MonoBehaviour
{
    [SerializeField]
    int currentHealth;

    [SerializeField]
    int lives;

    [SerializeField]
    int maxHealth;

    [SerializeField]
    private StatBarBehaviour healthSlider;

    public void SetHealth(int health)
    {
        currentHealth = health;
        healthSlider.SetStatBarLevel(health);
    }
    public int GetHealth() { return currentHealth; }

    public int GetMaxHealth() { return maxHealth; }
    public void SetLives(int value)
    {
        lives = value;
    }
    public int GetLives()
    {
        return lives;
    }



}
