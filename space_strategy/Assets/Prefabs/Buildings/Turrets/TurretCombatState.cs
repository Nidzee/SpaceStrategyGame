using UnityEngine;

public class TurretCombatState : ITurretState
{
    private float turnSpeed = 200f;
    


    public ITurretState DoState(Turette turret)
    {
        DoMyState(turret);


        if (!turret.turretData.isPowerON)
        {
            turret.ResetCombatMode();
            return turret.turretData.powerOffState;
        }

        else if (!turret.turretData.attackState)
        {
            turret.ResetCombatMode();
            return turret.turretData.idleState;
        }

        else
        {
            return turret.turretData.combatState;
        }
    }

    private void DoMyState(Turette turret)
    {
        TurnTowardsEnemy(turret);
        
        if (turret.turretData.isFacingEnemy)
        {
            turret.Attack();
        }
    }

    // Turning turret logic - correct!
    private void TurnTowardsEnemy(Turette turret)
    {
        if (turret.turretData.target)
        {
            Vector3 targetPosition = turret.turretData.target.transform.position;
            targetPosition.z = 0f;
    
            Vector3 turretPos = turret.turretData.center.transform.position;
            targetPosition.x = targetPosition.x - turretPos.x;
            targetPosition.y = targetPosition.y - turretPos.y;
    
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

            turret.turretData.targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            turret.turretData.center.transform.rotation = Quaternion.RotateTowards(turret.turretData.center.transform.rotation, turret.turretData.targetRotation, turnSpeed * Time.deltaTime);

            if (turret.turretData.center.transform.rotation == turret.turretData.targetRotation && !turret.turretData.isFacingEnemy)
            {
                turret.turretData.isFacingEnemy = true;
            }
        }
    }
}
