using UnityEngine;

public class UnitIGoToState : IUnitState
{
    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);


        // if(!unit.home)
        // {
        //     // TODO
        //     return unit.unitIHomelessState;
        // }

        if (unit.isApproachShaft)
        {
            unit.isApproachShaft = false;
            return unit.unitIGatherState;
        }

        else if (unit.isApproachSklad)
        {   
            unit.isApproachSklad = false;
            return unit.unitResourceLeavingState;
        }

        else if (unit.isApproachHome)
        {
            unit.isApproachHome = false;
            return unit.unitIdleState;
        }

        else 
            return unit.unitIGoToState;
    }

    private void DoMyState(Unit unit)
    {
        unit.transform.position = Vector3.MoveTowards(unit.transform.position, 
                                unit.destination, Unit._moveSpeed*Time.deltaTime);
    }
}

