using UnityEngine;

public class TurretRangeSingleLaser : MonoBehaviour
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
            
            if (myTurret.turretData.enemiesInsideRange.Count == 0)
            {
                myTurret.turretData.target = collider.GetComponent<Enemy>();
                myTurret.turretData.attackState = true;
            }
            myTurret.turretData.enemiesInsideRange.Add(collider.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D collider) // Detects if enemy left the combat range (Death or passing through)
    {
        if (collider.gameObject.tag == TagConstants.enemyTag)
        {
            Debug.Log("Enemy Exit!");

            myTurret.turretData.enemiesInsideRange.Remove(collider.GetComponent<Enemy>());

            if (myTurret.turretData.target == collider.GetComponent<Enemy>())
            {
                myTurret.GetComponent<TurretLaserSingle>().TurnOffLasers();

                if (myTurret.turretData.enemiesInsideRange.Count == 0)
                {
                    myTurret.turretData.attackState = false;
                }
                else
                {
                    myTurret.turretData.target = myTurret.turretData.enemiesInsideRange[(myTurret.turretData.enemiesInsideRange.Count-1)];
                }
            }
        }
    }
}