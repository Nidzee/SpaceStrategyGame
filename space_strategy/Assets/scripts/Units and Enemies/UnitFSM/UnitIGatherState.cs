using UnityEngine;

public class UnitIGatherState : IUnitState
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
            unit.destination = Unit.sklad;
            isStateEnd = false;
            return unit.unitIGoToState;
        }

        else 
            return unit.unitIGatherState;
    }

    private void DoMyState(Unit unit)
    {
        // Logic
    }
}

