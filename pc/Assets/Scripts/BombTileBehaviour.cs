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
            Collider2D[] explosionRadius = Physics2D.OverlapBoxAll(transform.position, new Vector2(explosionRange, explosionRange), 0f, LayerMask.GetMask("Ground"));
            for(int i = 0; i < explosionRadius.Length; i++)
            {
                (explosionRadius[i].gameObject.GetComponent(typeof(IDamagable)) as IDamagable)?.GetDamaged(damage);
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
