// using UnityEngine;

// public class TurretMisileCombatState : TurretCombatState
// {
//     private float turnSpeed = 200f;
//     private Quaternion targetRotation = new Quaternion();


//     public override ITurretState DoState(Turette turret)
//     {
//         DoMyState(turret);


//         if (!turret.isPowerON)
//         {
//             return turret.powerOffState;
//         }

//         else if (!turret.attackState)
//         {
//             return turret.idleState;
//         }

//         else
//         {
//             return turret.combatState;
//         }
//     }

//     private void DoMyState(Turette turret)
//     {
//         TurnTowardsEnemy(turret);
        
//         if (turret.isFacingEnemy)
//         {
//             turret.Attack();
//         }
//     }

//     // Turning turret logic - correct!
//     private void TurnTowardsEnemy(Turette turret)
//     {
//         if (turret.target)
//         {
//             Vector3 targetPosition = turret.target.transform.position;
//             targetPosition.z = 0f;
    
//             Vector3 turretPos = turret.center.transform.position;
//             targetPosition.x = targetPosition.x - turretPos.x;
//             targetPosition.y = targetPosition.y - turretPos.y;
    
//             float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

//             targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
//             turret.center.transform.rotation = Quaternion.RotateTowards(turret.center.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

//             if (turret.center.transform.rotation == targetRotation && !turret.isFacingEnemy)
//             {
//                 turret.isFacingEnemy = true;
//             }
//         }
//     }
// }
