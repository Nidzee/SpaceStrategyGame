using UnityEngine;

public class UnitIGoToState : IUnitState
{
    public IUnitState DoState(Unit unit)
    {
        DoMyState(unit);


        if (!unit.home)
        {
            return unit.unitIHomelessState;
        }

        if (!unit.workPlace && unit.destination != unit.home.angarPosition && unit.destination != unit.storage.dispenserPosition) // if we lost job on way
        {
            unit.destination = unit.home.angarPosition;
            return unit.unitIGoToState;
        }

        if (unit.destination == unit.home.angarPosition && unit.workPlace) // if we get job on way to home
        {
            unit.destination = unit.workPlace.dispenserPosition;
            return unit.unitIGoToState;
        }

        if (unit.isApproachShaft)
        {
            unit.isApproachShaft = false;
            return unit.unitIGatherState;
        }

        else if (unit.isApproachStorage)
        {   
            unit.isApproachStorage = false;
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
        // TODO moving logic

        unit.transform.position = Vector3.MoveTowards(unit.transform.position, 
                                unit.destination, Unit.moveSpeed*Time.deltaTime);

    }
}

