using System.Collections;
using UnityEngine;

public class TurretLaser : Turette
{
    public float barrelTurnSpeed = 200f;
    public bool isLasersEnabled = false; 

    public int damagePoints;
    public float damageTimer;

    public bool isAttackStart = false;

    public override void DestroyBuilding()
    {
        ResourceManager.Instance.laserTurretsList.Remove(this);

        base.DestroyBuilding();
    }

    public override void Attack()
    {
        Debug.Log("Start");
        StartCoroutine("LaserDamageLogic");
    }

    public void TurnOffLaserDamage()
    {
        Debug.Log("Stop");
        StopCoroutine("LaserDamageLogic");
    }

    IEnumerator LaserDamageLogic()
    {
        while(true)
        {
            while (damageTimer < 0.5f)
            {
                damageTimer += Time.deltaTime;
                yield return null;
            }
            damageTimer = 0;

            target.TakeDamage(damagePoints);
        }
    }
}