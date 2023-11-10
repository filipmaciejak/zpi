using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 20; 
    public float cooldown = 0.5f;
    private float lastShotTime = 0;
    public void ShootBullet()
    {
        if (lastShotTime + cooldown < Time.time)
        {
            lastShotTime = Time.time;
            GameObject bullet = Instantiate(bulletPrefab, gameObject.transform.position, gameObject.transform.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = rb.transform.up * bulletSpeed;
        }
    }
}
