using UnityEngine;

public class TurretIdleState : ITurretState
{
    private float turnSpeed = 100;


    public ITurretState DoState(Turette turret)
    {
        DoMyState(turret);


        if (!turret.isPowerON)
        {
            turret.isTurnedInIdleMode = true;
            turret.coolDownTurnTimer = 3f;
            return turret.powerOffState;
        }

        else if (turret.attackState)
        {
            turret.isTurnedInIdleMode = true;
            turret.coolDownTurnTimer = 3f;
            return turret.combatState;
        }

        else
        {
            return turret.idleState;
        }
    }

    private void DoMyState(Turette turret)
    {
        RandomIdleTurn(turret);
    }

    private void RandomIdleTurn(Turette turret)
    {
        if (turret.isTurnedInIdleMode)
        {
            turret.coolDownTurnTimer -= Time.deltaTime;

            if (turret.coolDownTurnTimer <= 0)
            {
                turret.coolDownTurnTimer = 3f;
                turret.isTurnedInIdleMode = false;
                turret.idleRotation = Quaternion.Euler(new Vector3(0, 0, (int)Random.Range(0, 360)));
            }
        }
        
        else
        {
            turret.center.transform.rotation = Quaternion.RotateTowards(turret.center.transform.rotation, turret.idleRotation, turnSpeed * Time.deltaTime);

            if (turret.center.transform.rotation == turret.idleRotation)
            {
                turret.isTurnedInIdleMode = true;
            }
        }
    }
}
