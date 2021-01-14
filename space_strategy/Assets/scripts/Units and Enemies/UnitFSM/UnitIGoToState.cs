using UnityEngine;

public class UnitIGoToState : IUnitState
{
    private bool isApproach = false;

    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);


        if(!unit.home)
        {
            return unit.unitIHomelessState;
        }

        else if (isApproach)
        {
            if(unit.destination == unit.workPlace.transform.position)
            {
                isApproach = false;
                return unit.unitIGatherState;
            }
            else if (unit.destination == unit.home.transform.position)
            {
                isApproach = false;
                return unit.unitIdleState;
            }
            else if (unit.destination == Unit.sklad)
            {
                isApproach = false;
                return unit.unitResourceLeavingState;
            }
            else 
                return unit.unitIGoToState;
        }

        else 
            return unit.unitIGoToState;
    }

    private void DoMyState(Unit unit)
    {
        // Logic
    }
}

