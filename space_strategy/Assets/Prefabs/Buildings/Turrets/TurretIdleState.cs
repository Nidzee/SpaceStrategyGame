using UnityEngine;

public class TurretIdleState : ITurretState
{
    private float turnSpeed = 100;


    public ITurretState DoState(Turette turret)
    {
        DoMyState(turret);


        if (!turret.turretData.isPowerON)
        {
            turret.turretData.isTurnedInIdleMode = true;
            turret.turretData.coolDownTurnTimer = 3f;
            return turret.turretData.powerOffState;
        }

        else if (turret.turretData.attackState)
        {
            turret.turretData.isTurnedInIdleMode = true;
            turret.turretData.coolDownTurnTimer = 3f;
            return turret.turretData.combatState;
        }

        else
        {
            return turret.turretData.idleState;
        }
    }

    private void DoMyState(Turette turret)
    {
        RandomIdleTurn(turret);
    }

    private void RandomIdleTurn(Turette turret)
    {
        if (turret.turretData.isTurnedInIdleMode)
        {
            turret.turretData.coolDownTurnTimer -= Time.deltaTime;

            if (turret.turretData.coolDownTurnTimer <= 0)
            {
                turret.turretData.coolDownTurnTimer = 3f;
                turret.turretData.isTurnedInIdleMode = false;
                turret.turretData.idleRotation = Quaternion.Euler(new Vector3(0, 0, (int)Random.Range(0, 360)));
            }
        }
        
        else
        {
            turret.turretData.center.transform.rotation = Quaternion.RotateTowards(turret.turretData.center.transform.rotation, turret.turretData.idleRotation, turnSpeed * Time.deltaTime);

            if (turret.turretData.center.transform.rotation == turret.turretData.idleRotation)
            {
                turret.turretData.isTurnedInIdleMode = true;
            }
        }
    }
}
