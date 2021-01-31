﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRangeDoubleBullet : MonoBehaviour
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
                myTurret.attackState = true;
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
                myTurret.isFacingEnemy = false;
                myTurret.GetComponent<TurretBulletDouble>().isBarrelFacingEnemy = false;
                myTurret.GetComponent<TurretBulletDouble>().isBarrel1FacingEnemy = false;

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