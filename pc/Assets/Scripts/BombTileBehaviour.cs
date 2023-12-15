using UnityEngine;

public class BombTileBehaviour: MonoBehaviour
{

    [SerializeField]
    private int damage;

    [SerializeField]
    private int explosionRange;

    private void Explode()
    {
        if (explosionRange > 0)
        {
            Collider2D[] explosionRadius = Physics2D.OverlapBoxAll(transform.position, new Vector2(explosionRange, explosionRange), 0f);
            for(int i = 0; i < explosionRadius.Length; i++)
            {
                IDamagable damagedObject = explosionRadius[i].gameObject.GetComponent(typeof(IDamagable)) as IDamagable;

                if (damagedObject == null)
                {
                    Transform parent = explosionRadius[i].gameObject.transform.parent;
                    if (parent != null)
                    {
                        damagedObject = parent.GetComponent(typeof(IDamagable)) as IDamagable;
                    }
                }
                damagedObject?.GetDamaged(damage);
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Explosion!");
            Explode();
            Destroy(gameObject);
        }
    }
}
