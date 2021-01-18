using UnityEngine;

public class UnitResourceLeavingState : IUnitState
{
    private bool isCoolDownOver = false;
    private float unitCoolDownTimer = 2f;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);


        if (!unit.home) // means that we dont have job and home
        {
            // we dont have resource, job, home
            return unit.unitIHomelessState;
        }

        if (isCoolDownOver)
        {
            isCoolDownOver = false;

            if (unit.workPlace) // we still have job - go to work
            {
                unit.destination = unit.workPlace.dispenserPosition;
                return unit.unitIGoToState;
            }
            else // we dont have job - go home
            {
                unit.destination = unit.home.angarPosition;
                return unit.unitIGoToState;
            }
        }

        else 
            return unit.unitResourceLeavingState;
    }

    private void DoMyState(Unit unit) // sleeping
    {
        if (unit.resource) // Resource destruction
        {
            GameObject.Destroy(unit.resource);
        }

        CoolDownLogic();
    }

    private void CoolDownLogic()
    {
        unitCoolDownTimer -= Time.deltaTime;
        if (unitCoolDownTimer <= 0)
        {
            unitCoolDownTimer = 2f;
            isCoolDownOver = true;
        }
    }
}
