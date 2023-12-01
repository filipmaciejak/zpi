using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class MechState : MonoBehaviour, IDamagable
{
    public int teamId;

    ArrayList damageTracker;

    [SerializeField]
    private int hullBreakTime;

    [SerializeField]
    private int hullBreakDamage;

    private MechHealth mechHealth;
    private MechShield mechShield;
    private MechRespawn mechRespawn;

    private void Start()
    {
        mechRespawn = GetComponent<MechRespawn>();
        mechShield = GetComponent<MechShield>();
        mechHealth = GetComponent<MechHealth>();
        damageTracker = new ArrayList();
    }

    public void GetDamaged(int damage)
    {
        int currentShield = mechShield.GetShield();
        int currentHealth = mechHealth.GetHealth();
        int shieldPrediction = currentShield - damage;
        if(shieldPrediction < 0)
        {
            damage = damage - currentShield;
            mechShield.SetShield(0);       //TODO: przetestowac ladowanie tarczy i dostawanie damage jednoczesnie czy nie wchodzi w konflikt
            currentHealth -= damage;
            if(currentHealth <= 0)
            {
                Die();
            }
            else
            {
                TrackRecentDamage(damage);
                mechHealth.SetHealth(currentHealth);
            }

        }
        else
        {
             mechShield.SetShield(shieldPrediction);
        }
        Debug.Log("Health " + mechHealth.GetHealth());
        Debug.Log("Shield " + mechShield.GetShield());
    }
    public void Die()
    {
        if (mechHealth.GetLives() > 1)
        {
            mechRespawn.Respawn();
        }
        else
        {
            Debug.Log("Przegrales");
            Destroy(gameObject);
            //TODO: GameOver
        }
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
    
}
