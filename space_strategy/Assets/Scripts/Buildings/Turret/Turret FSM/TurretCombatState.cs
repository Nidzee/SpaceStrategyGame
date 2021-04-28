using UnityEngine;

public class TurretCombatState : ITurretState
{
    private float turnSpeed = 200f;
    
    public ITurretState DoState(Turette turret)
    {
        DoMyState(turret);


        if (!turret.isPowerON)
        {
            turret.ResetCombatMode();
            return turret.powerOffState;
        }

        else if (!turret.isAttackState)
        {
            turret.ResetCombatMode();
            return turret.idleState;
        }

        else
        {
            return turret.combatState;
        }
    }

    private void DoMyState(Turette turret)
    {
        TurnTowardsEnemy(turret);
        
        if (turret.isFacingEnemy)
        {
            turret.Attack();
        }
    }

    private void TurnTowardsEnemy(Turette turret)
    {
        if (turret.target)
        {
            Vector3 targetPosition = turret.target.transform.position;
            targetPosition.z = 0f;
    
            Vector3 turretPos = turret.center.transform.position;
            targetPosition.x = targetPosition.x - turretPos.x;
            targetPosition.y = targetPosition.y - turretPos.y;
    
            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

            turret.targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            turret.center.transform.rotation = Quaternion.RotateTowards(turret.center.transform.rotation, turret.targetRotation, turnSpeed * Time.deltaTime);

            if (turret.center.transform.rotation == turret.targetRotation && !turret.isFacingEnemy)
            {
                turret.isFacingEnemy = true;
            }
        }
    }
}
