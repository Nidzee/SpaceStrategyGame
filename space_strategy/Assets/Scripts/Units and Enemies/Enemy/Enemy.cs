using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : AliveGameUnit
{
    public Rigidbody2D rigidBodyRef;
    public int bashAdditionalDamage = 0;

    public GameObject canvas;
    public GameObject bars;
    public Slider healthBar; 
    public Slider shieldhBar;
    public GameObject powerOffIndicator;

    
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