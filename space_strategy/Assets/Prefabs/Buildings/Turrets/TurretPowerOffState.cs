using UnityEngine;

public class TurretPowerOffState : ITurretState
{
    public ITurretState DoState(Turette turret)
    {
        DoMyState(turret);
        

        if (turret.isPowerON)
        {
            if (turret.attackState)
            {
                return turret.combatState;
            }
            else
            {
                return turret.idleState;
            }
        }

        else
        {
            return turret.powerOffState;
        }
    }

    private void DoMyState(Turette turret)
    {
        Debug.Log("Power Off Logic!");
    }
}
