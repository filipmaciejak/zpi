using UnityEngine;

public class DestructibleWallBehaviour : MonoBehaviour, IDamagable
{
    [SerializeField]
    private int startingHitPoints = 30;

    [SerializeField]
    private Sprite lightlyDamagedSprite;

    [SerializeField]
    private Sprite averageDamagedSprite;

    [SerializeField]
    private Sprite heavilyDamagedSprite;

    private SpriteRenderer spriteRenderer;

    private float hitPoints;


    public void GetDamaged(int damage)
    {
        hitPoints -= damage;
        if (hitPoints < startingHitPoints)
        {
            spriteRenderer.sprite = lightlyDamagedSprite;
        }
        else if (hitPoints <= 0.7 * startingHitPoints)
        {
            spriteRenderer.sprite = averageDamagedSprite;
        }
        else if (hitPoints <= 0.4 * startingHitPoints)
        {
            spriteRenderer.sprite = heavilyDamagedSprite;
        }
        else if( hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        hitPoints = startingHitPoints;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    
}
