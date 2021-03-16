using UnityEngine;

public class TurretPowerOffState : ITurretState
{
    public ITurretState DoState(Turette turret)
    {
        DoMyState(turret);
        

        if (turret.turretData.isPowerON)
        {
            if (turret.turretData.attackState)
            {
                return turret.turretData.combatState;
            }
            else
            {
                return turret.turretData.idleState;
            }
        }

        else
        {
            return turret.turretData.powerOffState;
        }
    }

    private void DoMyState(Turette turret)
    {
        Debug.Log("Power Off Logic!");
    }
}
