using UnityEngine;

public class UnitResourceLeavingState : IUnitState
{
    private bool isStateEnd = false;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);


        if(!unit.home)
        {
            return unit.unitIHomelessState;
        }

        else if (isStateEnd)
        {
            if(unit.workPlace)
            {
                unit.destination = unit.workPlace.transform.position;
                isStateEnd = false;
                return unit.unitIGoToState;
            }
            else
            {
                unit.destination = unit.home.transform.position;
                isStateEnd = false;
                return unit.unitIGoToState;
            }
        }

        else 
            return unit.unitResourceLeavingState;
    }

    private void DoMyState(Unit unit)
    {
        // Logic
    }
}
