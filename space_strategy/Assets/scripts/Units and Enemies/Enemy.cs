using UnityEngine;

public class Enemy : AliveGameUnit
{
    public Rigidbody2D rb;
    public int bashAdditionalDamage = 0;


    
    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};

    public delegate void EnemyDestroy(AliveGameUnit gameUnit);
    public event EnemyDestroy OnEnemyDestroyed = delegate{};



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints + bashAdditionalDamage);

        if (healthPoints <= 0)
        {
            DestroyEnemy();
            return;
        }

        OnDamageTaken(this);
    }

    public virtual void DestroyEnemy()
    {
        Instantiate(PrefabManager.Instance.enemyDeathParticles, transform.position, transform.rotation);

        Destroy(gameObject);
    }

}
