using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    public int damagePoints;

    private float timer = 1f;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
