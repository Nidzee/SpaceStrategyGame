using UnityEngine;

public class UnitIdleState : IUnitState
{
    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);


        if(!unit.home)
        {
            return unit.unitIHomelessState;
        }

        else if (unit.workPlace)
        {
            unit.destination = unit.workPlace.transform.position;
            return unit.unitIGoToState;
        }

        else 
            return unit.unitIdleState;
    }

    private void DoMyState(Unit unit)
    {
        // Logic
    }
}
