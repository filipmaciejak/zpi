using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public class MechState : MonoBehaviour, IDamagable
{

    public UnityEvent<int> onMechDeath;
    public UnityEvent<int> onHullBreach;

    public int teamId;
    public int mechLives;
    ArrayList damageTracker;

    [SerializeField]
    private int hullBreakTime;

    [SerializeField]
    private int hullBreakDamage;

    private MechHealth mechHealth;
    private MechShield mechShield;
    private void Awake()
    {
        onMechDeath = new UnityEvent<int>();
        onHullBreach = new UnityEvent<int>();
    }
    private void Start()
    {
        mechShield = GetComponent<MechShield>();
        mechHealth = GetComponentInChildren<MechHealth>();
        damageTracker = new ArrayList();


    }

    public void GetDamaged(int damage)
    {
        int currentShield = mechShield.GetShield();
        int currentHealth = mechHealth.GetCurrentHealth();
        int shieldPrediction = currentShield - damage;
        if(shieldPrediction < 0)
        {
            damage -= currentShield;
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
    }
    public void Die()
    {
        onMechDeath.Invoke(teamId);
    }
    public void TrackRecentDamage(int damage)
    {
        ArrayList filteredDamageTracker = new();
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
            damageTracker.Clear();
            onHullBreach.Invoke(teamId);
        }
    }
    
}
