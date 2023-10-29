using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public int damage = 5;
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("HIT");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Damage");
            collision.gameObject.GetComponentInParent<MechState>().GetDamaged(damage);
        }
        Destroy(gameObject);
    }
}
