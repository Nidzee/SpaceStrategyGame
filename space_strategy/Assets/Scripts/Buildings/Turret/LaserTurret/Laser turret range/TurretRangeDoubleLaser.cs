using UnityEngine;

public class TurretRangeDoubleLaser : MonoBehaviour
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
            Debug.Log("Enemy Enter!");
            
            if (myTurret.enemiesInsideRange.Count == 0)
            {
                myTurret.target = collider.GetComponent<Enemy>();
                myTurret.isAttackState = true;
            }
            myTurret.enemiesInsideRange.Add(collider.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D collider) // Detects if enemy left the combat range (Death or passing through)
    {
        if (collider.gameObject.tag == TagConstants.enemyTag)
        {
            Debug.Log("Enemy Exit!");

            myTurret.enemiesInsideRange.Remove(collider.GetComponent<Enemy>());

            if (myTurret.target == collider.GetComponent<Enemy>())
            {
                myTurret.GetComponent<TurretLaserDouble>().TurnOffLasers();


                if (myTurret.enemiesInsideRange.Count == 0)
                {
                    myTurret.isAttackState = false;
                }
                else
                {
                    myTurret.target = myTurret.enemiesInsideRange[(myTurret.enemiesInsideRange.Count-1)];
                }
            }
        }
    }
}