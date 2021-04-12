using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : AliveGameUnit
{
    public Rigidbody2D rb;
    public int bashAdditionalDamage = 0;


    
    public delegate void DamageTaken(AliveGameUnit gameUnit);
    public event DamageTaken OnDamageTaken = delegate{};
    public delegate void EnemyDestroy(AliveGameUnit gameUnit);
    public event EnemyDestroy OnEnemyDestroyed = delegate{};


    public GameObject canvas;
    public GameObject bars;
    public Slider healthBar; 
    public Slider shieldhBar;
    public GameObject powerOffIndicator;

    
    public override void TakeDamage(int damagePoints)
    {
        base.TakeDamage(damagePoints + bashAdditionalDamage);

        if (healthPoints <= 0)
        {
            DestroyEnemy();
            return;
        }

        bars.SetActive(true);

        healthBar.maxValue = maxCurrentHealthPoints;
        healthBar.value = healthPoints;
        shieldhBar.maxValue = maxCurrentShieldPoints;
        shieldhBar.value = shieldPoints;

        StopCoroutine("UICanvasmaintaining");
        uiCanvasDissapearingTimer = 0f;
        StartCoroutine("UICanvasmaintaining");

        OnDamageTaken(this);
    }

    float uiCanvasDissapearingTimer = 0f;
    IEnumerator UICanvasmaintaining()
    {
        while (uiCanvasDissapearingTimer < 3)
        {
            uiCanvasDissapearingTimer += Time.deltaTime;
            yield return null;
        }
        uiCanvasDissapearingTimer = 0;
        
        bars.SetActive(false);
    }

    public virtual void DestroyEnemy()
    {
        Instantiate(PrefabManager.Instance.enemyDeathParticles, transform.position, transform.rotation);

        Destroy(gameObject);
    }

}
