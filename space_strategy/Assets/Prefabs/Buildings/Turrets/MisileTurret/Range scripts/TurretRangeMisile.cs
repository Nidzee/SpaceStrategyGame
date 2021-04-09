using UnityEngine;

public class TurretRangeMisile : MonoBehaviour
{
    private Turette myTurret;

    private void Awake()
    {
        myTurret = gameObject.transform.parent.GetComponent<Turette>();
    }


    private void OnTriggerEnter2D(Collider2D collider) // Detects enemy when it arrives in combat range
    {
        if (collider.gameObject.tag == TagConstants.enemyTag)
        {
            if (myTurret.enemiesInsideRange.Count == 0)
            {
                myTurret.target = collider.GetComponent<Enemy>();
                myTurret.attackState = true;
            }
            myTurret.enemiesInsideRange.Add(collider.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D collider) // Detects if enemy left the combat range (Death or passing through)
    {
        if (collider.gameObject.tag == TagConstants.enemyTag)
        {
            myTurret.enemiesInsideRange.Remove(collider.GetComponent<Enemy>());

            if (myTurret.target == collider.GetComponent<Enemy>())
            {
                myTurret.isFacingEnemy = false;

                if (myTurret.enemiesInsideRange.Count == 0)
                {
                    myTurret.attackState = false;
                }
                else
                {
                    myTurret.target = myTurret.enemiesInsideRange[(myTurret.enemiesInsideRange.Count-1)];
                }
            }
        }
    }
}
