using UnityEngine;

public class TurretPowerOffState : ITurretState
{
    private bool isIndicatorInitialized = false;

    public ITurretState DoState(Turette turret)
    {
        DoMyState(turret);
        

        if (turret.isPowerON)
        {
            if (turret.attackState)
            {
                isIndicatorInitialized = false;
                
                turret.powerOffIndicator.SetActive(false);

                return turret.combatState;
            }
            else
            {
                isIndicatorInitialized = false;

                turret.powerOffIndicator.SetActive(false);

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
        
        if (!isIndicatorInitialized)
        {
            isIndicatorInitialized = true;
            turret.powerOffIndicator.SetActive(true);
        }
    }
}
