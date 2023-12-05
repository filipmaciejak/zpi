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
       

        MechHealth mechHealth = GetComponentInChildren<MechHealth>();
        MechShield mechShield = GetComponent<MechShield>();

        //TODO: stworzenie wraku mecha w miejsce smierci
        mechShield.SetShield(mechShield.GetMaxShield() /2);
        mechHealth.SetHealth(mechHealth.GetMaxHealth()); 

        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        gameObject.GetComponentInChildren<Rigidbody2D>().angularVelocity = 0;
        gameObject.transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
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
