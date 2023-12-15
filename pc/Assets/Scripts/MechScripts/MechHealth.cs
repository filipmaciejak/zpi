using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MechHealth : MonoBehaviour
{
    [SerializeField]
    int currentHealth;

    [SerializeField]
    int maxHealth;

    [SerializeField]
    private StatBarBehaviour healthSlider;

    public void Start()
    {
        healthSlider.SetStatBarLevel(currentHealth);
    }
    public void SetHealth(int health)
    {
        currentHealth = health;
        healthSlider.SetStatBarLevel(health);
    }
    public int GetCurrentHealth() { return currentHealth; }

    public int GetMaxHealth() { return maxHealth; }

}
