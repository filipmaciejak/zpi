using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public int damage = 5;
    void OnCollisionEnter2D(Collision2D collision)
    {
        IDamagable damagedObject = collision.gameObject.GetComponent(typeof(IDamagable)) as IDamagable;

        if (damagedObject == null)
        {
            Transform parent = collision.gameObject.transform.parent;
            if(parent != null)
            {
                damagedObject = parent.GetComponent(typeof(IDamagable)) as IDamagable;
            }
        }
        damagedObject?.GetDamaged(damage);

        Destroy(gameObject);
    }
}
