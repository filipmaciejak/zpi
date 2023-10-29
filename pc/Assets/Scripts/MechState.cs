using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class MechState : MonoBehaviour
{
    [SerializeField]
    int currentHealth;

    [SerializeField]
    int lives;

    [SerializeField]
    int shield;

    [SerializeField]
    int maxHealth;

    [SerializeField]
    int maxShield;

    ArrayList damageTracker;

    [SerializeField]
    private int hullBreakTime;

    [SerializeField]
    private int hullBreakDamage;
    private Vector3 spawnPoint;

    private void Start()
    {
        spawnPoint = transform.position;
        damageTracker = new ArrayList();
    }

    public void GetDamaged(int damage)
    {
        int shieldPrediction = shield - damage;
        if(shieldPrediction < 0)
        {
            damage = damage - shield;
            shield = 0;         //TODO: przetestowac ladowanie tarczy i dostawanie damage jednoczesnie czy nie wchodzi w konflikt
            currentHealth -= damage;
            if(currentHealth <= 0)
            {
                Die();
            }
            else
            {
                TrackRecentDamage(damage);
            }

        }
        else
        {
            shield = shieldPrediction;
        }
        Debug.Log(currentHealth);
    }
    public void TrackRecentDamage(int damage)
    {
        ArrayList filteredDamageTracker = new ArrayList();
        damageTracker.Add(Tuple.Create( damage, Time.time ));
        for (int i = 0; i < damageTracker.Count; i++)
        {
            var record = (Tuple<int, float>) damageTracker[i];
            var (_, damageTime) = record;
            if (damageTime + hullBreakTime >= Time.time) filteredDamageTracker.Add(record);
        }
        damageTracker = filteredDamageTracker;
        int damageSum = damageTracker.Cast<Tuple<int,float>>().Select(tuple => tuple.Item1).Sum();
        if(damageSum >= hullBreakDamage)
        {
            Debug.Log("HULL DAMAGED, REPAIRS NEEDED");
            damageTracker.Clear();
            //TODO: Stworzenie wyrwy w mechu
        }
    }
    public void Die()
    {
        if(lives > 1)
        {
            lives--;
            //TODO: stworzenie wraku mecha w miejsce smierci
            shield = maxShield/2;
            currentHealth = maxHealth;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            gameObject.GetComponentInChildren<Rigidbody2D>().angularVelocity = 0;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.position = spawnPoint;
        }
        else
        {
            Debug.Log("Przegrales");
            Destroy(gameObject);
            //TODO: GameOver
        }
    }
    public void SetShield(int shield) { this.shield = shield; }
    public int GetShield() { return shield; }
    public int GetHealth() { return currentHealth; }
    public void SetSpawnPoint(Vector3 spawnPoint) { this.spawnPoint = spawnPoint; }

    public Vector3 GetSpawnPoint() { return spawnPoint; }

    public int GetCurrentHealth() { return currentHealth; }
}
