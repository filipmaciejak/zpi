using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MechRespawn : MonoBehaviour
{

    private Vector3 spawnPoint;


    private void Start()
    {
        spawnPoint = transform.position;
    }

    public void Respawn()
    {
       

        MechHealth mechHealth = GetComponent<MechHealth>();
        MechShield mechShield = GetComponent<MechShield>();

        mechHealth.SetLives(mechHealth.GetLives() - 1);

        //TODO: stworzenie wraku mecha w miejsce smierci
        mechShield.SetShield(mechShield.GetMaxShield() /2);
        mechHealth.SetHealth(mechHealth.GetMaxHealth()); 

        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        gameObject.GetComponentInChildren<Rigidbody2D>().angularVelocity = 0;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.position = spawnPoint;
    }

    public void SetSpawnPoint(Vector3 spawnPoint)
    {
        this.spawnPoint = spawnPoint;
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPoint; 
    }

}
